using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    public int runCount = 1;
    public List<Card> deck = new List<Card>();
    public List<Card> usingDeck = new List<Card>();

    public int money = 0;
    
    public CardInfo[] cardInfos;

    public UnityEvent onDeckChange = new UnityEvent();
    public UnityEvent<CardDisplay> onCardClicked;

    public Color[] colors;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadCardSprites();
    }

    public void InitDeck()
    {
        foreach (Card card in deck)
        {
            usingDeck.Add(card.Clone());
        }
    }

    private Dictionary<int, Sprite> cardSprites = new();
    private void LoadCardSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Card");
        foreach (Sprite sprite in sprites)
        {
            int id = int.Parse(sprite.name[4..]);
            cardSprites[id] = sprite;
        }
    }

    public Sprite GetCardSprite(int id)
    {
        if (cardSprites.ContainsKey(id)) return cardSprites[id];
        return null;
    }
    

    public Card DrawCard()
    {
        if ( usingDeck.Count == 0) return null;
        
        SoundManager.Instance.PlaySFX(ESfx.SFX_DRAW_CARD);
        int r = Random.Range(0,  usingDeck.Count);
        
        Card card =  usingDeck[r];
         usingDeck.RemoveAt(r);
        return card;
    }
    
    public CardInfo GetCardInfo(Card card)
    {
        foreach (var cardinfo in cardInfos)
        {
            if (cardinfo.Card.cardName.Equals(card.cardName)) return cardinfo;
        }

        return null;
    }

    public void AddCard(Card card)
    {
        if ( usingDeck.Count >= 25) return;
         usingDeck.Add(card);
        onDeckChange.Invoke();
    }
    

    public void RemoveCard(Card card)
    {
        for (int i = 0; i <  usingDeck.Count; i++)
        {
            if ( usingDeck[i].cardName == card.cardName)
            {
                 usingDeck.RemoveAt(i);
                onDeckChange.Invoke();
                GetCardInfo(card).Count += 1;
                break;
            }
        }
    }
}
