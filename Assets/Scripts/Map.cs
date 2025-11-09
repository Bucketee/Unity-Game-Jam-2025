using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour, IPointerClickHandler
{
    private bool isShowingMap = false;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        isShowingMap = !isShowingMap;
        transform.parent.GetChild(0).gameObject.SetActive(isShowingMap);
    }
}
