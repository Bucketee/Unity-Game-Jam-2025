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

[CreateAssetMenu(menuName = "Card", fileName = "New Card")]
public class Card : ScriptableObject
{
    public string cardName => this.name;
    public Sprite cardImage;
    public CardType cardType;
    public string cardText;
}
