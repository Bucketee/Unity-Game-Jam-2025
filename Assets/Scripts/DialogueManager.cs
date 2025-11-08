using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueOption
{
    public string text;      // 선택지 텍스트
    public int nextId;       // 다음 대화 ID
}

[System.Serializable]
public class DialogueNode
{
    public int id;           // 대화 ID
    public string speaker;   // 대화자 이름
    public string text;      // 대화 내용
    public List<DialogueOption> options;  // 선택지
}

[System.Serializable]
public class DialogueList
{
    public List<DialogueNode> conversation;
}

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI chatLog;       // 채팅 로그 UI
    //public TextMeshProUGUI optionsText;   // 선택지 UI
    private int currentId = 1;            // 현재 대화 ID
    private Dictionary<int, DialogueNode> dialogue;

    void Awake()
    {
        // JSON 파일 로드 및 대화 데이터 초기화
        LoadDialogue();
        DisplayDialogue(currentId);
    }

    void LoadDialogue()
    {
        // Resources 폴더에서 JSON 파일을 읽어옴
        TextAsset file = Resources.Load<TextAsset>("dialogue");
        DialogueList dialogueList = JsonUtility.FromJson<DialogueList>(file.text);

        // 대화 데이터를 Dictionary로 변환
        dialogue = new Dictionary<int, DialogueNode>();
        foreach (DialogueNode node in dialogueList.conversation)
        {
            dialogue.Add(node.id, node);
        }
    }

    void DisplayDialogue(int id)
    {
        // 현재 대화 노드를 불러옴
        var node = dialogue[id];
        chatLog.text = node.speaker + ": " + node.text + "\n";

        // 선택지를 UI에 표시
        /*optionsText.text = "";
        foreach (var option in node.options)
        {
            optionsText.text += option.text + "\n";
        }*/
    }

    public void OnOptionSelected(int nextId)
    {
        // 선택한 옵션에 따라 다음 대화로 이동
        currentId = nextId;
        DisplayDialogue(currentId);
    }
}