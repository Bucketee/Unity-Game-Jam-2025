using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ending", menuName = "Create Ending")]
public class Ending : ScriptableObject
{
    public string EndingName => name;
    public string endingDescription;
    public int endPrice;
    public bool isClearedOnce;

    public EndingCondition condition;
}

/// <summary>
/// 상관 없는 조건은 빈칸으로
/// 배열에 포함되어 있으면 true
/// </summary>
[Serializable]
public class EndingCondition
{
    public int date = 1;
    public Card[] placeCard;
    public Card[] monsterCard;
    public Card[] weaponCard;
    public Card[] behaviorCard;
    public Card[] emotionCard;
    public int likabilityMin = 0; public int likabilityMax = 10;
    public int attackPowerMin = 0; public int attackPowerMax = 5;
}