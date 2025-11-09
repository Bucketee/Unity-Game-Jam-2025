using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;
    
    public List<Card> deck = new List<Card>();

    public int money = 0;
    
    public CardInfo[] cardInfos;

    public UnityEvent onDeckChange = new UnityEvent();
    public UnityEvent<CardDisplay> onCardClicked;

    private void Awake()
    {
        if (Instance != null) DestroyImmediate(this);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    

    public Card DrawCard()
    {
        if (deck.Count == 0) return null;
        int r = Random.Range(0, deck.Count);
        
        Card card = deck[r];
        deck.RemoveAt(r);
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
        if (deck.Count >= 25) return;
        deck.Add(card);
        onDeckChange.Invoke();
    }
    

    public void RemoveCard(Card card)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].cardName == card.cardName)
            {
                deck.RemoveAt(i);
                onDeckChange.Invoke();
                GetCardInfo(card).Count += 1;
                break;
            }
        }
    }
}
