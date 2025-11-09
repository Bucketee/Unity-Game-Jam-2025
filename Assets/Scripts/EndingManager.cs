using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public List<Ending> endings;
    private HeroStat heroStat;

    private void Start()
    {
        heroStat = HeroStat.Instance;
    }

    [ContextMenu("Check Ending")]
    public void CheckEnding()
    {
        foreach (Ending ending in endings)
        {
            if (CheckEnd(ending))
            {
                Debug.Log(ending.name);
            }
        }
    }

    private bool CheckEnd(Ending ending)
    {
        EndingCondition condition = ending.condition;
        if (condition.date == 0 || condition.date == HeroStat.Instance.Date)
        {
            if (condition.placeCard.Length == 0 || condition.placeCard.Contains(heroStat.GetAnswer(0)))
            {
                if (condition.monsterCard.Length == 0 || condition.monsterCard.Contains(heroStat.GetAnswer(1)))
                {
                    if ((condition.weaponCard.Length == 0 || condition.weaponCard.Contains(heroStat.GetAnswer(2))) && (condition.behaviorCard.Length == 0 || condition.behaviorCard.Contains(heroStat.GetAnswer(3))))
                    {
                        if (condition.emotionCard.Length == 0 || condition.emotionCard.Contains(heroStat.GetAnswer(4)))
                        {
                            if (condition.likabilityMin <= heroStat.Likeability &&
                                heroStat.Likeability <= condition.likabilityMax)
                            {
                                if (condition.attackPowerMin <= heroStat.AttackPower &&
                                    heroStat.AttackPower <= condition.attackPowerMax)
                                {
                                    switch (ending.EndingName)
                                    {
                                        case "Killed by Dragon":
                                            if (heroStat.dungeunCount == 3)
                                            {
                                                return true;
                                            }
                                            return false;
                                        case "Kill The Diablo With Dragon":
                                            if (heroStat.dungeunCount == 4)
                                            {
                                                return true;
                                            }
                                            return false;
                                        case "Bad Innkeeper":
                                            if (heroStat.IsRandomPlace)
                                            {
                                                return true;
                                            }
                                            return false;
                                    }
                                    return true;
                                }
                            }
                        }
                    }

                    switch (ending.EndingName)
                    {
                        case "Killed by Slime":
                            break;
                    }
                }
            }
        }

        return false;
    }
}