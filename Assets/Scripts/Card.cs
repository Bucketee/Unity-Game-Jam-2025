using System;
using UnityEngine;

public enum CardType
{
    Yes,
    No,
    Expression,
    RecommendPlace,
    RecommendWeapon,
    RecommendMonster,
    RecommendBehavior,
}

[Serializable]
[CreateAssetMenu(menuName = "Card", fileName = "New Card")]
public class Card : ScriptableObject
{
    public string cardName => this.name;
    public Sprite cardImage;
    public CardType cardType;
    public string cardText;

    public override string ToString()
    {
        return $"Name : {cardName} / Type : {cardType} / Text : {cardText}";
    }

    public Card Clone()
    {
        Card card = ScriptableObject.CreateInstance<Card>();
        card.name = this.cardName;
        card.cardImage = this.cardImage;
        card.cardType = this.cardType;
        card.cardText = this.cardText;
        
        return card;
    }
}
