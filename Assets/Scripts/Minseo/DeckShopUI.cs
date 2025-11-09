using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class CardInfo
{
    public Card Card;
    public int Count;
}
public class DeckShopUI : MonoBehaviour
{
    public RectTransform shopPagePanel;
    public CardInfo[] cardInfos;

    public List<Transform> pages;
    public Transform shopPages;

    public GameObject cardPrefab;
    public GameObject pagePrefab;
    public List<GameObject> cardGameObjectsInPage;

    private int _pageCount;
    private int _maxPageCount;
    
    private const float PageGap = 1300f;

    public float pageMoveDuration = 0.2f;

    private void Start()
    {
        _maxPageCount = (cardInfos.Length + 7) / 8;
        _pageCount = 0;
        
        CreatePages();
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

    public void CreatePages()
    {
        foreach (var obj in cardGameObjectsInPage)
        {
            if (obj != null)
                Destroy(obj);
        }

        cardGameObjectsInPage.Clear();
        int cardIdx = 0;
        for (int i = 0; i < _maxPageCount + 8; i++)
        {
            var pagego = Instantiate(pagePrefab, shopPages).transform;
            for (int j = 0; j<8; j++)
            {
                cardIdx = i * 8 + j;
                if (cardIdx >= cardInfos.Length) break; 
                var go = Instantiate(cardPrefab, pagego).GetComponent<CardDisplay>();
                go.Init(cardInfos[i * 8 + j].Card);
                cardGameObjectsInPage.Add(go.gameObject);
            }
        }
    }
}
