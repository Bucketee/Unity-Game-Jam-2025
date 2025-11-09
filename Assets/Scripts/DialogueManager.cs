using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum DialogueType
{
    Normal = 1,
    Branching = 2,
}

public class DialogueGroup
{
    public int id;
    public DialogueType type;
    public Dictionary<int, Dialogue> dialogues;
}

public class Dialogue
{
    public int id;
    public string text;
    public Dictionary<int, DialogueEffect> effects;
    public int cutimageIdx;
    public int questionType;

    public int wrongansHandler = 8282;
}

public class DialogueEffect
{
    public int nextGroup;
    public int questionType;

    public int likablity;
    public int attackPower;

    public void ApplyEffect()
    {
        // Implement effect application logic here
        HeroStat.Instance.AttackPower += attackPower;
        HeroStat.Instance.Likeability += likablity;
        
        Debug.Log("ATTACK POWER INCREASED BY " + attackPower);
        Debug.Log("LIKABILTY INCREASED BY " + likablity);
    }
}

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    public static DialogueManager Instance;
    void Awake()
    {
        if (Instance != null) DestroyImmediate(this);
        Instance = this;

        LoadDialogue();
    }

    public Dictionary<int, DialogueGroup> dialogueGroups = new();
    private void LoadDialogue()
    {
        TextAsset xmlAsset = Resources.Load<TextAsset>("Dialogue");
        if (xmlAsset == null)
        {
            Debug.LogError("Dialogue XML file not found!");
            return;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlAsset.text);

        XmlNodeList groupNodes = xmlDoc.SelectNodes("/root/DialogueGroup");
        foreach (XmlNode groupNode in groupNodes)
        {
            DialogueGroup group = new DialogueGroup();
            group.id = int.Parse(groupNode.Attributes["id"].Value);
            group.type = (DialogueType)int.Parse(groupNode.Attributes["type"].Value);
            group.dialogues = new Dictionary<int, Dialogue>();

            XmlNodeList dialogueNodes = groupNode.SelectNodes("Dialogue");
            foreach (XmlNode dialogueNode in dialogueNodes)
            {
                Dialogue dialogue = new Dialogue();
                dialogue.id = int.Parse(dialogueNode.Attributes["id"].Value);
                dialogue.text = dialogueNode.Attributes["text"].Value;
                dialogue.cutimageIdx = int.Parse(dialogueNode.Attributes["image"].Value);
                int questionType = int.Parse(dialogueNode.Attributes["questionType"].Value);
                dialogue.effects = new Dictionary<int, DialogueEffect>();
                dialogue.wrongansHandler =
                    dialogueNode.Attributes["wrongans"] != null ?
                    int.Parse(dialogueNode.Attributes["wrongans"].Value) :
                    8282;

                XmlNodeList cardNodes = dialogueNode.SelectNodes("Card");
                foreach (XmlNode cardNode in cardNodes)
                {
                    DialogueEffect effect = new DialogueEffect();
                    int cardId = int.Parse(cardNode.Attributes["id"].Value);
                    effect.nextGroup = int.Parse(cardNode.Attributes["nextGroup"].Value);
                    effect.questionType = questionType;

                    effect.likablity = cardNode.Attributes["likablity"] != null
                        ? int.Parse(cardNode.Attributes["likablity"].Value)
                        : 0;
                    effect.attackPower = cardNode.Attributes["attackPower"] != null
                        ? int.Parse(cardNode.Attributes["attackPower"].Value)
                        : 0;

                    dialogue.effects.Add(cardId, effect);
                }

                group.dialogues.Add(dialogue.id, dialogue);
            }

            dialogueGroups.Add(group.id, group);
            Debug.Log($"group id: {group.id}");
        }
    }
    #endregion

    private void Start()
    {
        SetDialogueGroup(0);
    }

    void Update()
    {
        bool test = false;

        // for test
        if (!test && Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("B TEST START");
            SetDialogueGroup(0);
            test = true;
        }
    }

    private int currentGroupId = -1;
    private DialogueGroup currentGroup = null;
    private int currentDialogueId = -1;
    private Dialogue currentDialogue = null;
    public Image cutsceneImage;
    public Sprite[] cutsceneSprites;

    public event Action OnGroupChanged, OnDialogueChanged;
    
    private void SetDialogueGroup(int groupId, int dialogueId = 0)
    {
        currentGroupId = groupId;
        currentGroup = dialogueGroups[currentGroupId];
        OnGroupChanged?.Invoke();

        currentDialogueId = dialogueId;
        currentDialogue = currentGroup.dialogues[currentDialogueId];
        
        cutsceneImage.sprite = cutsceneSprites[currentDialogue.cutimageIdx];
        OnDialogueChanged?.Invoke();
    }
    private void SetDialogue(int dialogueId)
    {
        currentDialogueId = dialogueId;
        currentDialogue = currentGroup.dialogues[currentDialogueId];
        
        // cutscene image change!
        cutsceneImage.sprite = cutsceneSprites[currentDialogue.cutimageIdx];
        
        OnDialogueChanged?.Invoke();
    }
    public bool IsLastDialogue()
    {
        return !currentGroup.dialogues.ContainsKey(currentDialogueId + 1);
    }

    public void GoToNextDay()
    {
        EndingManager.Instance.CheckEnding();
        
        HeroStat.Instance.InitRCMs();
        SetDialogueGroup(HeroStat.Instance.Date);
    }

    public string GetDialogue() => currentDialogue.text;
    public DialogueType GetDialogueType() => currentGroup.type;

    public void NextDialogue()
    {
        if (currentGroup.dialogues.ContainsKey(currentDialogueId + 1))
            SetDialogue(currentDialogueId + 1);

        // 현재 그룹의 마지막 dialogue라면
        // 분기 텍스트 (type = 2) 일 때는 이전의 그룹으로 돌아간다
        // 일반 텍스트 (type = 1) 일 때는 다음 날로 넘어간다
        else 
        {
            if (currentGroup.type == DialogueType.Branching) PopGroup();
            else if (currentGroup.type == DialogueType.Normal)
            {
                // 다음 날로 넘어가기
                GoToNextDay();
            }
        }
    }

    public void PrevDialogue()
    {
        if (currentGroup.type != DialogueType.Normal) return;
        if (currentGroup.dialogues.ContainsKey(currentDialogueId - 1))
            SetDialogue(currentDialogueId - 1);
    }

    // For backtracking, dialogue group 점프와 돌아오기 처리
    // 마지막에 있었던 (groupId, dialogueId) 쌍을 스택에 저장함
    private Stack<(int, int)> groupHistory = new();

    [SerializeField] private RectTransform cardSubmit;
    public bool SubmitCheck(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(
            cardSubmit,
            eventData.position,
            eventData.pressEventCamera))
        {
            return true;
        }
        return false;
    }

    public Card GetCard(int idx)
    {
        foreach (var cardInfo in DeckManager.Instance.cardInfos)
        {
            if (cardInfo.Card.id == idx) return cardInfo.Card;
        }

        return null;
    }

    // cardId에 해당하는 카드를 제출했을 때의 처리
    public void SubmitCard(int cardId)
    {
        Debug.Log($"Submitted card {cardId}");

        if (!currentDialogue.effects.ContainsKey(cardId))
        {
            // 엉뚱한 대답을 제출한 상황
            // 용사 호감도 내려감, 실망 이펙트 등
            // 엉뚱한 대답에 대한 (미리 정의된) dialogue group으로 점프함
            PushGroup(currentDialogue.wrongansHandler);
            Debug.Log("Wrong card");
            return;
        }

        DialogueEffect effect = currentDialogue.effects[cardId];
        var card = GetCard(cardId);
        
        if (card == null) return;
        
        Debug.Log("!!!ASDGASDG");
        HeroStat.Instance.AnswerQuestion(card, currentDialogue.questionType);
        
        effect.ApplyEffect();
        PushGroup(effect.nextGroup);
    }

    // 다음 그룹으로 점프하기
    // 현재 있었던 곳을 저장하고 점프한다
    public void PushGroup(int nextGroup)
    {
        Debug.Log($"Jump to {nextGroup}");
        int returnAddr;
        if (IsLastDialogue()) returnAddr = currentDialogueId;
        else returnAddr = currentDialogueId + 1;
        groupHistory.Push((currentGroupId, returnAddr));
        SetDialogueGroup(nextGroup);
    }

    // 이전의 그룹으로 돌아오기
    // stack의 마지막 (groupId, dialogueId) 쌍으로 돌아간다
    public void PopGroup() 
    {
        (int groupId, int dialogueId) = groupHistory.Pop();
        SetDialogueGroup(groupId, dialogueId);
    }
}