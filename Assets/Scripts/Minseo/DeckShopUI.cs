using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DeckShopUI : MonoBehaviour
{
    public RectTransform shopPagePanel;
    public CardInfo[] cardInfos;

    public List<Transform> pages = new List<Transform>();
    public Transform shopPages;

    public GameObject cardPrefab;
    public GameObject pagePrefab;
    public List<GameObject> cardGameObjectsInPage;

    public TMP_Text moneyText;

    public Image fadeImage;

    private int _pageCount;
    private int _maxPageCount;
    
    private const float PageGap = 1300f;

    public float pageMoveDuration = 0.2f;

    private void Start()
    {
        fadeImage.DOFade(1, 0);
        fadeImage.DOFade(0,0.5f).OnComplete(()=>fadeImage.gameObject.SetActive(false));
        _maxPageCount = (DeckManager.Instance.cardInfos.Length + 7) / 8;
        _pageCount = 0;

        cardInfos = DeckManager.Instance.cardInfos;
        
        CreatePages();
    }

    private void Update()
    {
        moneyText.text = DeckManager.Instance.money.ToString() + " GOLD";
    }

    public void BackToMain()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(0, 0);
        fadeImage.DOFade(1, 1f).OnComplete(() => SceneManager.LoadScene("Scenes/MainScene"));
    }

    public void GoToNextPage()
    {
        if (_pageCount == _maxPageCount - 1) return;
        _pageCount++;
        shopPagePanel.DOLocalMoveX(-PageGap * _pageCount, pageMoveDuration);
    }
    
    public void GoToPrevPage()
    {
        if (_pageCount == 0) return;
        _pageCount--;
        shopPagePanel.DOLocalMoveX(-PageGap * _pageCount, pageMoveDuration);
    }
    

    public CardInfo GetCardInfo(Card card)
    {
        foreach (var cardinfo in cardInfos)
        {
            if (cardinfo.Card.cardName.Equals(card.cardName)) return cardinfo;
        }

        return null;
    }

    public void CreatePages()
    {
        foreach (var obj in cardGameObjectsInPage)
        {
            if (obj != null)
                Destroy(obj);
        }

        cardGameObjectsInPage.Clear();
        int cardIdx = 0;
        Debug.Log(_maxPageCount);
        for (int i = 0; i < _maxPageCount + 8; i++)
        {
            var pagego = Instantiate(pagePrefab, shopPages).transform;
            for (int j = 0; j<8; j++)
            {
                cardIdx = i * 8 + j;
                if (cardIdx >= cardInfos.Length) break; 
                var go = Instantiate(cardPrefab, pagego).GetComponent<CardDisplay>();
                go.Init(cardInfos[i * 8 + j].Card, false);
                cardGameObjectsInPage.Add(go.gameObject);
            }
        }
    }
}
