using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;
    
    public List<Card> deck = new List<Card>();

    private void Awake()
    {
        if (Instance != null) DestroyImmediate(this);
        Instance = this;
    }

    public Card DrawCard()
    {
        if (deck.Count == 0) return null;
        int r = Random.Range(0, deck.Count);
        
        Card card = deck[r];
        deck.RemoveAt(r);
        return card;
    }
}
