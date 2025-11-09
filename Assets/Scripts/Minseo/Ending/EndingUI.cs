using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndingUI : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI endingNameText;
    public Ending ending;
    public void Init(Ending endinga)
    {
        endingNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        ending = endinga; 
        endingNameText.text = ending.name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EndingListUI.Instance.ShowEndingInfo(ending);
    }
}
