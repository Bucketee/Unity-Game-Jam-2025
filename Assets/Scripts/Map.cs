using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("asd");
    }
}
