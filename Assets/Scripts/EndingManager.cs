using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;
    
    [SerializeField] private GameObject endingScene;
    public List<Ending> endings;
    
    private HeroStat heroStat;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        heroStat = HeroStat.Instance;
        StartEnding(endings[1]);
    }

    [ContextMenu("Check Ending")]
    public bool CheckEnding()
    {
        foreach (Ending ending in endings)
        {
            if (CheckEnd(ending))
            {
                StartEnding(ending);
                return true;
            }
        }

        return false;
    }
    
    private void StartEnding(Ending ending)
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(EBgm.BGM_END);
        
        Debug.Log(ending.name);
        DeckManager.Instance.money += ending.endPrice / (ending.isClearedOnce ? 2 : 1);
        ending.isClearedOnce = true;
        
        StartCoroutine(EndingSceneCo(ending));
    }

    private IEnumerator EndingSceneCo(Ending ending)
    {
        endingScene.SetActive(true);
        
        float time = 0;
        float duration = 3f;
        
        Image image = endingScene.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
        
        endingScene.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        endingScene.transform.GetChild(0).GetComponent<Image>().sprite = ending.endingSprite;
        
        endingScene.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        endingScene.transform.GetChild(2).gameObject.SetActive(false);
        
        while (time < duration)
        {
            image.color = Color.Lerp(image.color, Color.black, time / duration / 30);
            time += Time.deltaTime;
            yield return null;
        }
        
        image.color = Color.black;

        time = 0;
        duration = 5f;
        image = endingScene.transform.GetChild(0).GetComponent<Image>();
        Color target = HeroStat.Instance.currentColor;

        while (time < duration)
        {
            image.color = Color.Lerp(image.color, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        image.color = target;

        string endingText = ending.endingDescription;
        string s = "";
        TextMeshProUGUI text = endingScene.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < endingText.Length; i++)
        {
            s += endingText[i];
            text.text = s;
            yield return new WaitForSeconds(0.1f);
        }
        
        yield return new WaitForSeconds(1f);
        
        endingScene.transform.GetChild(2).gameObject.SetActive(true);
        endingScene.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
        endingScene.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(CloseEnding);
    }

    private void CloseEnding()
    {
        endingScene.SetActive(false);
    }

    private bool CheckEnd(Ending ending)
    {
        EndingCondition condition = ending.condition;
        if (condition.date == 0 || condition.date == heroStat.Date)
        {
            if (condition.placeCard.Count == 0 || condition.heroPlace == HeroStat.Instance.today.place)
            {
                if (condition.monsterCard.Count == 0 || condition.heroMonster == HeroStat.Instance.today.monster)
                {
                    if ((condition.weaponCard.Count == 0 || condition.heroWeapon == HeroStat.Instance.today.weapon) && (condition.behaviorCard.Count == 0 || condition.behavior == HeroStat.Instance.today.behavior))
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

        return false;
    }
}