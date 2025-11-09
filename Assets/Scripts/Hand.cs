using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand Instance;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<Card> cardsInHand;

    private Action DrawAction;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        DrawAction += () =>
        {
            DrawCardFromDeck();
            OrganizeCards();
        };

        /*foreach (Transform child in transform)
        {
            child.TryGetComponent(out CardDisplay display);
            cardsInHand.Add(display.card);
        }*/
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) Draw();
    }

    public void DrawCards(int amount = 5)
    {
        for (int i = 0; i < amount; i++)
        {
            Draw();
        }
    }

    [ContextMenu("Draw")]
    public void Draw()
    {
        DrawAction?.Invoke();
    }

    [ContextMenu("Organize Cards")]
    public void OrganizeCards()
    {
        float cardCount = transform.childCount;

        float cardSpacing = Utils.CardSpacing;
        if (Utils.CardSpacing * (cardCount - 1) > Utils.HandWidth)
            cardSpacing = Utils.HandWidth / (cardCount - 1);
        float cardStartX = -((cardCount - 1) * cardSpacing) / 2;

        for (int i = 0; i < cardCount; i++)
        {
            RectTransform child = transform.GetChild(i).gameObject.GetComponent<RectTransform>();
            child.anchoredPosition = new Vector3(cardStartX + i * cardSpacing, 0);
            child.gameObject.GetComponent<CardDisplay>().SynchPosition();
        }
    }

    public IEnumerator OrganizeCardCo(RectTransform child, Vector3 target, Quaternion rotation)
    {
        float time = 0;
        float duration = 0.5f;

        while (time < duration)
        {
            time += Time.deltaTime;
            child.anchoredPosition = Vector3.Lerp(child.anchoredPosition, target, time / duration);
            child.rotation = Quaternion.Lerp(child.rotation, rotation, time / duration);
            yield return null;
        }

        child.anchoredPosition = target;
        child.rotation = rotation;
    }

    public void DrawCardFromDeck()
    {
        Card card = DeckManager.Instance.DrawCard();
        if (!card)
        {
            Debug.LogError("No More Card Left");
            return;
        }
        cardsInHand.Add(card);
        DisplayCardToHand(card.Clone());
    }

    public void DisplayCardToHand(Card card)
    {
        GameObject go = Instantiate(cardPrefab, transform.position, transform.rotation, transform);
        go.TryGetComponent(out CardDisplay cd);
        if (cd)
        {
            cd.Init(card);
        }
    }
}