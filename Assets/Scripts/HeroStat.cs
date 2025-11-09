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
    private int date = 1;

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
    public List<Card> questionAnswers = new List<Card>(5);

    public int dungeunCount;
    public int forestCount;

    public bool IsRandomPlace
    {
        get => !questionAnswers[0];
    }

    private void Awake()
    {
        Instance = this;
    }

    private void InitHeroStat()
    {
        date = 0;
        likeability = 0;
        attackPower = 0;

        for (int i = 0; i < questionAnswers.Count; i++)
        {
            questionAnswers[i] = null;
        }
    }
    
    public void InitRCMs()
    {
        date++;

        questionAnswers = new List<Card>(5);
    }
    
    /// <param name="card"></param>
    /// <param name="index">0~4, place/monster/weapon/behavior/random</param>
    public void AnswerQuestion(Card card, int index)
    {
        questionAnswers[index] = card.Clone();
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