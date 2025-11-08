using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public float angleSpread;
    [SerializeField] private List<Card> cardsInHand;

    private GameObject cardDisplay;
    private Action DrawAction;

    private void Awake()
    {
        cardDisplay = Resources.Load<GameObject>("CardDisplay");
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

    [ContextMenu("Draw")]
    public void Draw()
    {
        DrawAction?.Invoke();
    }
    
    [ContextMenu("Organize Cards")]
    public void OrganizeCards()
    {
        StopAllCoroutines();
        float cardCount = transform.childCount;
        for (int i = 0; i < cardCount; i++)
        {
            Transform child = transform.GetChild(i);
            StartCoroutine(OrganizeCardCo(child, new Vector3(0, -150, 0),
                Quaternion.AngleAxis(-(i - (cardCount - 1) / 2) * angleSpread, Vector3.forward)));
        }
    }

    public IEnumerator OrganizeCardCo(Transform child, Vector3 target, Quaternion rotation)
    {
        float time = 0;
        float duration = 0.5f;

        while (time < duration)
        {
            time += Time.deltaTime;
            child.transform.localPosition = Vector3.Lerp(child.transform.localPosition, target, time / duration);
            child.transform.localRotation = Quaternion.Lerp(child.transform.localRotation, rotation, time / duration);
            yield return null;
        }
        
        
        child.transform.SetLocalPositionAndRotation(target, rotation);
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
        GameObject go = Instantiate(cardDisplay, transform.position, transform.rotation, transform);
        go.TryGetComponent(out CardDisplay cd);
        if (cd)
        {
            cd.Init(card);
        }
    }
}