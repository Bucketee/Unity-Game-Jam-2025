using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Ending
{
    public Sprite cutscene;
    public string name;
    public string description;
}

public class EndingListUI : MonoBehaviour
{
    public static EndingListUI Instance;
    public List<Ending> endings;
    public GameObject endingUI;
    public Transform endingListTransform;

    public Transform myEndingContatiner;

    public VerticalLayoutGroup listLayoutGroup;
    public float minideckHeight;
    
    public Image endingFade;
    public Image endingCutscene;
    public TMP_Text endingDescription;
    
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        foreach (Ending ending in endings)
        {
            var go = Instantiate(endingUI, endingListTransform);
            go.GetComponent<EndingUI>().Init(ending);
        }
        
        int chcnt = endings.Count;
        float height =     listLayoutGroup.padding.top 
                           + listLayoutGroup.padding.bottom 
                           + minideckHeight * chcnt
                           + listLayoutGroup.spacing * (chcnt - 1);
        
        RectTransform rectTransform = myEndingContatiner.GetComponent<RectTransform>();

        Vector2 size = rectTransform.sizeDelta;
        size.y = height;            // 높이 변경
        rectTransform.sizeDelta = size;
    }

    public void ShowEndingInfo(Ending ending)
    {
        endingFade.DOFade(1, 0f);
        endingFade.DOFade(0, 0.5f);
        
        endingCutscene.sprite = ending.cutscene;
        endingDescription.text = ending.description;
    }
}
