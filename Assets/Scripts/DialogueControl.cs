using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueTmp;
    [SerializeField] private Button leftButton, rightButton, downButton;

    private void Start()
    {
        DialogueManager.Instance.OnGroupChanged += SetButton;
        DialogueManager.Instance.OnDialogueChanged += SetText;
    }

    private void OnDestroy()
    {
        DialogueManager.Instance.OnGroupChanged -= SetButton;
        DialogueManager.Instance.OnDialogueChanged -= SetText;
    }

    private void SetButton()
    {
        DialogueType type = DialogueManager.Instance.GetDialogueType();
        bool isNormal = type == DialogueType.Normal;
        leftButton.gameObject.SetActive(isNormal);
        rightButton.gameObject.SetActive(isNormal);
        downButton.gameObject.SetActive(!isNormal);
    }
    private void SetText() => dialogueTmp.text = DialogueManager.Instance.GetDialogue();

    public void NextDialogue() => DialogueManager.Instance.NextDialogue();
    public void PrevDialogue() => DialogueManager.Instance.PrevDialogue();

}