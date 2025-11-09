using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckManagerUI : MonoBehaviour
{
    public GameObject miniDeckPrefab;
    
    public Transform myDeckContainer;

    public List<GameObject> miniDeckGameObjects = new List<GameObject>();
    
    public VerticalLayoutGroup deckLayoutGroup;

    public float minideckHeight;

    public TMP_Text countText;
    
    private void Start()
    {
        DeckManager.Instance.onDeckChange.AddListener(OnDeckChange);
        OnDeckChange();
    }
    
    private void OnDisable()
    {
        DeckManager.Instance.onDeckChange.RemoveListener(OnDeckChange);
    }

    public void OnDeckChange()
    {
        foreach (GameObject miniDeck in miniDeckGameObjects)
        {
            Destroy(miniDeck);
        }
        miniDeckGameObjects.Clear();
        
        foreach (var card in DeckManager.Instance.deck)
        {
            var go = Instantiate(miniDeckPrefab, myDeckContainer).GetComponent<MiniCardUI>();
            miniDeckGameObjects.Add(go.gameObject);
            go.Init(card.Clone());
        }

        int chcnt = DeckManager.Instance.deck.Count;
        float height =     deckLayoutGroup.padding.top 
                           + deckLayoutGroup.padding.bottom 
                           + minideckHeight * chcnt
                           + deckLayoutGroup.spacing * (chcnt - 1);

        Debug.Log( myDeckContainer.childCount );
        Debug.Log(deckLayoutGroup.spacing);

        RectTransform rectTransform = myDeckContainer.GetComponent<RectTransform>();

        Vector2 size = rectTransform.sizeDelta;
        size.y = height;            // 높이 변경
        rectTransform.sizeDelta = size;

        countText.text = DeckManager.Instance.deck.Count + " / " + 25;
    }
}
