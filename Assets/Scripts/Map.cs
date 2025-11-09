using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour, IPointerClickHandler
{
    private bool isShowingMap = false;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(ESfx.SFX_START_BUTTON);
        isShowingMap = !isShowingMap;
        transform.parent.GetChild(0).gameObject.SetActive(isShowingMap);
    }
}
