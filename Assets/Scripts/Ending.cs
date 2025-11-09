using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ending", menuName = "Create Ending")]
public class Ending : ScriptableObject
{
    public string endingName;
    public string endingDescription;
    public int endPrice;
    public bool isClearedOnce;
    
    public Sprite endingSprite;
    

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
    public List<Card> placeCard;
    public List<Card> monsterCard;
    public List<Card> weaponCard;
    public List<Card> behaviorCard;
    public List<Card> emotionCard;
    public int likabilityMin = 0; public int likabilityMax = 10;
    public int attackPowerMin = 0; public int attackPowerMax = 5;
}