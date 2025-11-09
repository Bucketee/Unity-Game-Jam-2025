using TMPro;
using UnityEngine;

public class BuyingCardUI : MonoBehaviour
{
    public Card card;

    private TextMeshProUGUI cardNameText;
    private TextMeshProUGUI countText;

    
    private bool _canPointer;
    
    public void Init(Card card)
    {
        this.card = card.Clone();
        cardNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        countText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        cardNameText.text = card.cardName;
    }
    
    public void SetCount(int count)
    {
        countText.text = "You got " + count + " cards";
    }
}
