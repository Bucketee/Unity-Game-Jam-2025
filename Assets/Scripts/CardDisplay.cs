using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    private TextMeshProUGUI cardNameText;
    private Image cardImage;
    private TextMeshProUGUI cardTextText;

    public void Awake()
    {
        Init(card);
    }
    
    public void Init(Card card)
    {
        this.card = card;
        cardNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        cardImage = transform.GetChild(1).GetComponent<Image>();
        cardTextText = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        
        cardNameText.text = card.cardName;
        cardImage.sprite = card.cardImage;
        cardTextText.text = card.cardText;
    }
}
