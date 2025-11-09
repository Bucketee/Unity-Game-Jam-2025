using TMPro;
using UnityEngine;

public class MiniCardUI : MonoBehaviour
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
}
