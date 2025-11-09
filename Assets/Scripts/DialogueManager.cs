using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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
    public bool submitted;
    public Card whichCardSubmitted;
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
    public int drawAmounts;
    public int draws;
    public int totalDrawsWillbe;

    public Image fadeImage;
    public Image submittedCardImage;
    
    void Awake()
    {
        if (Instance != null) DestroyImmediate(this);
        Instance = this;
        drawAmounts = DeckManager.Instance.deck.Count / 5;
        totalDrawsWillbe = DeckManager.Instance.deck.Count;

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
                dialogue.questionType = questionType;
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

                    effect.likablity = cardNode.Attributes["likability"] != null
                        ? int.Parse(cardNode.Attributes["likability"].Value)
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
        Hand.Instance.DrawCards(drawAmounts);
        draws += drawAmounts;
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
        
        if (currentDialogue.submitted)
        {
            submittedCardImage.gameObject.SetActive(true);
            submittedCardImage.sprite = DeckManager.Instance.GetCardSprite(currentDialogue.whichCardSubmitted.id);
        }
        else
        {
            submittedCardImage.gameObject.SetActive(false);
        }
        
        cutsceneImage.sprite = cutsceneSprites[currentDialogue.cutimageIdx];
        OnDialogueChanged?.Invoke();
    }
    private void SetDialogue(int dialogueId)
    {
        currentDialogueId = dialogueId;
        currentDialogue = currentGroup.dialogues[currentDialogueId];
        
        // cutscene image change!
        cutsceneImage.sprite = cutsceneSprites[currentDialogue.cutimageIdx];

        if (currentDialogue.submitted)
        {
            submittedCardImage.gameObject.SetActive(true);
            submittedCardImage.sprite = DeckManager.Instance.GetCardSprite(currentDialogue.whichCardSubmitted.id);
        }
        else
        {
            submittedCardImage.gameObject.SetActive(false);
        }
        
        OnDialogueChanged?.Invoke();
    }
    public bool IsLastDialogue()
    {
        return !currentGroup.dialogues.ContainsKey(currentDialogueId + 1);
    }
    public bool IsFirstDialogue()
    {
        return currentDialogueId == 0;
    }
    // place/monster/weapon/behavior/random

    public TextMeshProUGUI eventText;
    public IEnumerator EventCheck()
    {
        int whichEvent = -1;
        try
        {
            if (HeroStat.Instance.dungeunCount == 3 &&
                HeroStat.Instance.questionAnswers[0].id == 12 &&
                HeroStat.Instance.questionAnswers[1].id == 15 &&
                HeroStat.Instance.questionAnswers[2].id == 8 &&
                HeroStat.Instance.questionAnswers[3].id == 18)
            {
                whichEvent = 0;
            }
            else if (HeroStat.Instance.questionAnswers[0].id == 11 &&
                     HeroStat.Instance.questionAnswers[1].id == 13)
            {
                whichEvent = 1;
            }
            else if (HeroStat.Instance.questionAnswers[0].id == 11 &&
                     HeroStat.Instance.questionAnswers[1].id == 14)
            {
                whichEvent = 2;
            }
            else if (HeroStat.Instance.questionAnswers[0].id == 12 &&
                     HeroStat.Instance.questionAnswers[1].id == 16)
            {
                whichEvent = 3;
            }
            else if (HeroStat.Instance.questionAnswers[0].id == 10 &&
                     HeroStat.Instance.questionAnswers[3].id == 19)
            {
                whichEvent = 4;
            }
            else if (HeroStat.Instance.questionAnswers[0].id == 10 &&
                     HeroStat.Instance.questionAnswers[3].id == 21)
            {
                whichEvent = 5;
            }
            else if (HeroStat.Instance.questionAnswers[0].id == 10 &&
                     HeroStat.Instance.questionAnswers[3].id == 20)
            {
                whichEvent = 6;
            }
            else if (HeroStat.Instance.questionAnswers[0].id == 12 &&
                     HeroStat.Instance.questionAnswers[1].id == 15)
            {
                whichEvent = 7;
            }
        }
        catch (NullReferenceException)
        {
            eventText.text = "";
        }

        if (whichEvent != -1)
        {
            eventText.gameObject.SetActive(true);
            switch (whichEvent)
            {
                case 0:
                    eventText.text = "BeFriend with Dragon! ❤️";
                    break;
        
                case 1:
                    eventText.text = "Slay the Slime!";
                    break;
        
                case 2:
                    eventText.text = "Defeat the Goblin!";
                    break;
        
                case 3:
                    eventText.text = "Hunt the Bat!";
                    break;
        
                case 4:
                    eventText.text = "Defeat the Zombie!";
                    break;
        
                case 5:
                    eventText.text = "Help the Village with Farming.";
                    break;
        
                case 6:
                    eventText.text = "Take a Rest in the Village.";
                    break;
                
                case 7:
                    eventText.text = "Kill the ZOMBIE!";
                    break;
            }
            
            yield return new WaitForSeconds(2f);
            eventText.gameObject.SetActive(false);
        }
        
        HeroStat.Instance.InitRCMs();
        SetDialogueGroup(HeroStat.Instance.Date - 1);
        
        if (HeroStat.Instance.Date != 5)
            Hand.Instance.DrawCards(drawAmounts);
        else
            Hand.Instance.DrawCards(totalDrawsWillbe - draws);
        
        fadeImage.DOFade(0, 1f)
            .OnComplete(()=>fadeImage.gameObject.SetActive(false));
        
        yield return null;
    }

    public void GoToNextDay()
    {
        if (EndingManager.Instance.CheckEnding()) return;

        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(0.8f, 2f)
            .OnComplete(()=>
            {
                StartCoroutine(EventCheck());
            });
        
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
    public bool SubmitCard(int cardId)
    {
        Debug.Log($"Submitted card {cardId}");
        
        Debug.Log("ASDG " + currentDialogue.submitted);
        Debug.Log("a112ASDG " + currentDialogue.questionType);

        if (currentDialogue.submitted) return false;
        if (currentGroup.type != DialogueType.Normal) return false;

        if (!currentDialogue.effects.ContainsKey(cardId))
        {
            // 엉뚱한 대답을 제출한 상황
            // 용사 호감도 내려감, 실망 이펙트 등
            // 엉뚱한 대답에 대한 (미리 정의된) dialogue group으로 점프함

            currentDialogue.submitted = true;
            currentDialogue.whichCardSubmitted = GetCard(cardId);
            PushGroup(currentDialogue.wrongansHandler);
            Debug.Log("Wrong card");
            return true;
        }

        DialogueEffect effect = currentDialogue.effects[cardId];
        var card = GetCard(cardId);

        if (card == null) return false;
        
        Debug.Log("!@!$!@$" + currentDialogue.questionType);
        HeroStat.Instance.AnswerQuestion(card, currentDialogue.questionType);
        currentDialogue.submitted = true;
        currentDialogue.whichCardSubmitted = card;
        effect.ApplyEffect();
        PushGroup(effect.nextGroup);
        return true;
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