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

    private int originSiblingIndex;
    
    public void Init(Card card)
    {
        this.card = card.Clone();
        cardNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        cardImage = transform.GetChild(1).GetComponent<Image>();
        cardTextText = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        
        cardNameText.text = card.cardName;
        cardImage.sprite = card.cardImage;
        cardTextText.text = card.cardText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToggleCard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnToggleCard();
    }

    private void ToggleCard()
    {
        //transform.parent.GetComponent<Hand>().OrganizeCards();
        originSiblingIndex = transform.GetSiblingIndex();

        StartCoroutine(ToggleCo());
    }

    private IEnumerator ToggleCo()
    {
        float duration = 0.2f;
        float time = 0;

        transform.rotation = Quaternion.identity;
        
        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(new Vector3(180 * (transform.GetSiblingIndex() -
                                                                      ((float)(transform.parent.childCount - 1) / 2)), 10, 0), new Vector3(210 * (transform.GetSiblingIndex() -
                ((float)(transform.parent.childCount - 1) / 2)), 30, 0), time / duration);
            yield return null;
        }

        transform.localPosition = new Vector3(210 * (transform.GetSiblingIndex() - ((float)(transform.parent.childCount - 1) / 2)), 30, 0);
        transform.SetAsLastSibling();
    }

    private void UnToggleCard()
    {
        StopAllCoroutines();
        
        transform.SetSiblingIndex(originSiblingIndex);
        //transform.parent.GetComponent<Hand>().OrganizeCards();
        transform.SetLocalPositionAndRotation(new Vector3(0, -150, 0), 
            Quaternion.AngleAxis(-(originSiblingIndex - (transform.parent.childCount - 1) / 2) * transform.parent.GetComponent<Hand>().angleSpread, Vector3.forward));
    }
}
