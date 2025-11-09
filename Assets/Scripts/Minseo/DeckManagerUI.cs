using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManagerUI : MonoBehaviour
{
    public GameObject miniDeckPrefab;
    
    public Transform myDeckContainer;

    public List<GameObject> miniDeckGameObjects = new List<GameObject>();
    
    public VerticalLayoutGroup deckLayoutGroup;

    public float minideckHeight;
    
    private void Start()
    {
        DeckManager.Instance.onDeckChange.AddListener(OnDeckChange);
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
            go.Init(card);
        }
        
        float height =     deckLayoutGroup.padding.top + deckLayoutGroup.padding.bottom + minideckHeight * myDeckContainer.childCount +
                           deckLayoutGroup.spacing * (myDeckContainer.childCount - 1);

        Rect rect = myDeckContainer.GetComponent<RectTransform>().rect;
        myDeckContainer.GetComponent<RectTransform>().rect.Set(rect.x, rect.y, rect.width, height);
    }
}
