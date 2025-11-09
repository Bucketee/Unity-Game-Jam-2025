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
        if (condition.date == 0 || condition.date == heroStat.Date)
        {
            if (condition.placeCard.Count == 0 || condition.placeCard.Contains(heroStat.questionAnswers[0]))
            {
                if (condition.monsterCard.Count == 0 || condition.monsterCard.Contains(heroStat.questionAnswers[1]))
                {
                    if ((condition.weaponCard.Count == 0 || condition.weaponCard.Contains(heroStat.questionAnswers[2])) && (condition.behaviorCard.Count == 0 || condition.behaviorCard.Contains(heroStat.questionAnswers[3])))
                    {
                        if (condition.emotionCard.Count == 0 || condition.emotionCard.Contains(heroStat.questionAnswers[4]))
                        {
                            if (condition.likabilityMin <= heroStat.Likeability &&
                                heroStat.Likeability <= condition.likabilityMax)
                            {
                                if (condition.attackPowerMin <= heroStat.AttackPower &&
                                    heroStat.AttackPower <= condition.attackPowerMax)
                                {
                                    switch (ending.endingName)
                                    {
                                        case "Flames of the Ancient Dragon":
                                            if (heroStat.dungeunCount == 3)
                                            {
                                                return true;
                                            }
                                            return false;
                                        case "Dragon’s Oath: The Last Dawn":
                                            if (heroStat.dungeunCount == 4)
                                            {
                                                return true;
                                            }
                                            return false;
                                        case "The Innkeeper of Shadows":
                                            if (heroStat.IsRandomPlace)
                                            {
                                                return true;
                                            }
                                            return false;
                                        case "The Fallen Crown":
                                            if (heroStat.forestCount == 4)
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
                }
            }
        }

        return false;
    }
}