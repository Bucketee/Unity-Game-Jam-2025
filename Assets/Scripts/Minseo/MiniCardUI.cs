using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniCardUI : MonoBehaviour, IPointerClickHandler
{
    public Card card;

    private TextMeshProUGUI cardNameText;
    
    private bool _canPointer;
    
    public void Init(Card card)
    {
        this.card = card.Clone();
        cardNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        cardNameText.text = card.cardName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DeckManager.Instance.RemoveCard(card);
        }
    }
}
