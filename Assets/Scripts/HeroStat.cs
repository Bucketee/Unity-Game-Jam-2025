using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HeroPlace { Village, Forest, Dungeon }
public enum HeroMonster { Slime, Goblin, Bat, Zombie }
public enum HeroWeapon { Sword, Bow, MorningStar, Dagger }
public enum HeroBehavior { Aggressive, Careful, Farming, TeamMaking, Rest }
public class HeroToday
{
    public HeroPlace place;
    public HeroMonster monster;
    public HeroWeapon weapon;
    public HeroBehavior behavior;
}

public class HeroStat : MonoBehaviour
{
    public static HeroStat Instance;
    public HeroToday today = new HeroToday();
    public void ResetToday() => today = new HeroToday();
    public void SetTodayPlace(HeroPlace place) => today.place = place;
    public void SetTodayMonster(HeroMonster monster) => today.monster = monster;
    public void SetTodayWeapon(HeroWeapon weapon) => today.weapon = weapon;
    public void SetTodayBehavior(HeroBehavior behavior) => today.behavior = behavior;

    public HeroPlace RandomPlace() => (HeroPlace)UnityEngine.Random.Range(0, 3);
    public HeroMonster RandomMonster() => (HeroMonster)UnityEngine.Random.Range(0, 4);
    public HeroWeapon RandomWeapon() => (HeroWeapon)UnityEngine.Random.Range(0, 4);
    public HeroBehavior RandomBehavior() => (HeroBehavior)UnityEngine.Random.Range(0, 5);

    public Color currentColor = Color.white;

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
    public List<Card> questionAnswers;

    public int dungeunCount;
    public int forestCount;

    public Image screenImage;
    public Image characterImage;
    public Image backgroundImage;

    public bool IsRandomPlace
    {
        get => !questionAnswers[0];
    }

    private void Awake()
    {
        Instance = this;
        questionAnswers = new List<Card>(5) { null, null, null, null, null };
    }

    private void InitHeroStat()
    {
        date = 0;
        likeability = 0;
        attackPower = 0;
        currentColor = DeckManager.Instance.colors[(DeckManager.Instance.runCount + DeckManager.Instance.colors.Length - 1) % DeckManager.Instance.colors.Length];

        screenImage.color = currentColor;
        characterImage.color = currentColor;
        backgroundImage.color = currentColor;

        for (int i = 0; i < questionAnswers.Count; i++)
        {
            questionAnswers[i] = null;
        }
    }

    public void InitRCMs()
    {
        date++;

        if (questionAnswers[0] != null && questionAnswers[0].id == 12)
        {
            dungeunCount++;
        }

        for (int i = 0; i < questionAnswers.Count; i++)
        {
            questionAnswers[i] = null;
        }
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