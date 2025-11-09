using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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

    public Image fadeImage;
    
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        int cnt = 0;
        fadeImage.DOFade(1, 0);
        fadeImage.DOFade(0,0.5f).OnComplete(()=>fadeImage.gameObject.SetActive(false));
        foreach (Ending ending in endings)
        {
            if (!ending.isClearedOnce) continue;
            
            var go = Instantiate(endingUI, endingListTransform);
            go.GetComponent<EndingUI>().Init(ending);

            cnt++;
        }

        int chcnt = cnt;
        float height =     listLayoutGroup.padding.top 
                           + listLayoutGroup.padding.bottom 
                           + minideckHeight * chcnt
                           + listLayoutGroup.spacing * (chcnt - 1);
        
        RectTransform rectTransform = myEndingContatiner.GetComponent<RectTransform>();

        Vector2 size = rectTransform.sizeDelta;
        size.y = height;            // 높이 변경
        rectTransform.sizeDelta = size;
    }

    public void BackToMain()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(0, 0);
        fadeImage.DOFade(1, 1f).OnComplete(() => SceneManager.LoadScene("Scenes/MainScene"));
    }

    public void ShowEndingInfo(Ending ending)
    {
        SoundManager.Instance.PlaySFX(ESfx.SFX_ENDING_LIST_BUTTON);
        
        endingFade.DOFade(1, 0f);
        endingFade.DOFade(0, 0.5f);
        
        endingCutscene.sprite = ending.endingSprite;
        endingDescription.text = ending.endingDescription;
    }
}
