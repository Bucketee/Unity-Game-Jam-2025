using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyingCardUI : MonoBehaviour
{
    public Card card;

    private TextMeshProUGUI cardNameText;
    private TextMeshProUGUI countText;
    private Image cardImage;
    private TextMeshProUGUI goldText;
    
    private bool _canPointer;
    
    public void Init(Card card)
    {
        this.card = card.Clone();
        cardNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        cardImage = transform.GetChild(1).GetComponent<Image>();
        countText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        goldText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        cardNameText.text = card.cardName;
        cardImage.sprite = DeckManager.Instance.GetCardSprite(card.id);
        goldText.text = DeckManager.Instance.GetCardInfo(card).price + " GOLD";
    }
    
    public void SetCount(int count)
    {
        countText.text = "You got " + count + " cards";
    }
}
