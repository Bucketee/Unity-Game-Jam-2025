using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private float angleSpread;
    
    [ContextMenu("Organize Cards")]
    public void OrganizeCards()
    {
        float cardCount = transform.childCount;
        for (int i = 0; i < cardCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.transform.SetLocalPositionAndRotation(new Vector3(0, -150, 0), Quaternion.AngleAxis(-(i - (cardCount - 1) / 2) * angleSpread, Vector3.forward));
        }
    }
}