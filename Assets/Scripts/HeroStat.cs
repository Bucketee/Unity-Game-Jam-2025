using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroStat : MonoBehaviour
{
    public static HeroStat Instance;

    public int Date
    {
        get => date;
        set => date = Mathf.Clamp(value, 1, 5);
    }
    private int date = 0;

    public int Likeability
    {
        get => likeability;
        set => likeability = Mathf.Clamp(value, 0, 10);
    }
    private int likeability = 0;

    public int AttackPower
    {
        get => attackPower;
        set => attackPower = Mathf.Clamp(value, 0, 5);
    }
    private int attackPower = 0;

    [Header("Question Answers")]
    private List<Card> questionAnswers;
    [SerializeField] private Card placeRcm = null;
    [SerializeField] private Card monsterRcm = null;
    [SerializeField] private Card weaponRcm = null;
    [SerializeField] private Card behaviorRcm = null;
    [SerializeField] private Card randomQuestion = null;

    public int dungeunCount;

    public bool IsRandomPlace
    {
        get => !placeRcm;
    }

    private void Awake()
    {
        if (Instance != null) DestroyImmediate(this);
        Instance = this;
        
        questionAnswers = new List<Card>()
        {
            placeRcm,
            monsterRcm,
            weaponRcm,
            behaviorRcm,
            randomQuestion
        };
    }

    private void InitHeroStat()
    {
        date = 0;
        likeability = 0;
        attackPower = 0;
        
        placeRcm = null;
        monsterRcm = null;
        weaponRcm = null;
        behaviorRcm = null;
        randomQuestion = null;
    }
    
    /// <param name="card"></param>
    /// <param name="index">0~4, place/monster/weapon/behavior/random</param>
    public void AnswerQuestion(Card card, int index)
    {
        questionAnswers[index] = card.Clone();
    }

    public Card GetAnswer(int index)
    {
        return questionAnswers[index];
    }
}

/*public enum Place
{
    Village,
    Forest,
    Dungeon
}

public enum Monster
{
    Slime,
    Goblin,
    Bat,
    Zombie,
    //Dragon
    //Diablo
}

public enum Weapon
{
    Sword,
    Bow,
    MorningStar,
    Dagger
}

public enum Behavior
{
    
}*/