using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Card card;

    private TextMeshProUGUI cardNameText;
    private Image cardImage;
    private TextMeshProUGUI cardTextText;
    private Vector3 originalPosition;
    private int originalSiblingIndex;
    private RectTransform cardRect;

    public void Init(Card card)
    {
        this.card = card.Clone();
        cardNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        cardImage = transform.GetChild(1).GetComponent<Image>();
        cardTextText = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();

        cardNameText.text = card.cardName;
        cardImage.sprite = card.cardImage;
        cardTextText.text = card.cardText;

        originalPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
        originalSiblingIndex = transform.GetSiblingIndex();

        cardRect = gameObject.GetComponent<RectTransform>();
        cardRect.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    public void SynchPosition()
    {
        if (cardTransitionCo != null) StopCoroutine(cardTransitionCo);
        cardTransitionCo = null;
        originalPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData) => ToggleCard();
    public void OnPointerExit(PointerEventData eventData) => UntoggleCard();

    private void ToggleCard()
    {
        if (cardTransitionCo != null) StopCoroutine(cardTransitionCo);
        cardTransitionCo = StartCoroutine(CardTransition(true));
    }

    private void UntoggleCard()
    {
        transform.SetSiblingIndex(originalSiblingIndex);

        if (cardTransitionCo != null) StopCoroutine(cardTransitionCo);
        cardTransitionCo = StartCoroutine(CardTransition(false));
    }

    private Coroutine cardTransitionCo = null;
    private IEnumerator CardTransition(bool isAppear = true)
    {
        cardRect = gameObject.GetComponent<RectTransform>();
        Vector2 startPos = cardRect.anchoredPosition;
        float originX = originalPosition.x, originY = originalPosition.y;
        Vector2 endPos = isAppear ? new Vector2(originX, originY + 80f) : new Vector2(originX, originY);

        float duration = 0.25f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            cardRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        cardRect.anchoredPosition = endPos;
        cardTransitionCo = null;

        if (isAppear) transform.SetAsLastSibling();
    }
}
