using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueTmp;
    [SerializeField] private Button leftButton, rightButton, downButton, nextDayButton;

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
        nextDayButton.gameObject.SetActive(false);
    }
    private void SetText()
    {
        dialogueTmp.text = DialogueManager.Instance.GetDialogue();

        if (DialogueManager.Instance.GetDialogueType() != DialogueType.Normal) return;
        if (DialogueManager.Instance.IsLastDialogue())
        {
            nextDayButton.gameObject.SetActive(true);
            rightButton.gameObject.SetActive(false);
        }
        else
        {
            nextDayButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(true);
        }
    }

    public void NextDialogue() => DialogueManager.Instance.NextDialogue();
    public void PrevDialogue() => DialogueManager.Instance.PrevDialogue();

}