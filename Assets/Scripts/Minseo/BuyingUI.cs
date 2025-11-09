using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BuyingUI : MonoBehaviour
{
    public GameObject buyingUI;
    public BuyingCardUI cardUI;
    public Image focusPanel;

    public RectTransform sourceRt;
    
    public DeckShopUI deckShopUI;

    public Card currentSelectedCard;
    
    private void Start()
    {
        DeckManager.Instance.onCardClicked.AddListener(FocusIn);
    }
    
    private void FocusIn(CardDisplay cardDisplay)
    {
        buyingUI.SetActive(true);
        focusPanel.DOFade(.8f, 0.4f);
        sourceRt = cardDisplay.GetComponent<RectTransform>();
        cardUI.GetComponent<RectTransform>().anchoredPosition = cardUI.transform.parent.GetComponent<RectTransform>().InverseTransformPoint(sourceRt.parent.TransformPoint(sourceRt.anchoredPosition));
        cardUI.GetComponent<RectTransform>()
            .DOAnchorPos(Vector2.up * 85f, 0.5f);
        cardUI.Init(cardDisplay.card);
        
        currentSelectedCard = cardDisplay.card;
        
        var cardinfo = deckShopUI.GetCardInfo(currentSelectedCard);
        cardUI.SetCount(cardinfo.Count);
    }
    
    public void FocusOut()
    {
        focusPanel.DOFade(0f, 0.4f);
        cardUI.GetComponent<RectTransform>()
            .DOAnchorPos(cardUI.transform.parent.GetComponent<RectTransform>().InverseTransformPoint(sourceRt.parent.TransformPoint(sourceRt.anchoredPosition)), 0.2f)
            .OnComplete(() => buyingUI.SetActive(false));
    }

    public void BuyCard()
    {
        var cardinfo = deckShopUI.GetCardInfo(currentSelectedCard);

        if (cardinfo.price > DeckManager.Instance.money) return; 
        cardinfo.Count += 1;
        DeckManager.Instance.money -= cardinfo.price;
        cardUI.SetCount(cardinfo.Count);
    }

    public void EquipCard()
    {
        var cardinfo = deckShopUI.GetCardInfo(currentSelectedCard);
        if (cardinfo.Count <= 0) return;
        
        cardinfo.Count -= 1;
        
        DeckManager.Instance.AddCard(currentSelectedCard);
        cardUI.SetCount(cardinfo.Count);
    }
}
