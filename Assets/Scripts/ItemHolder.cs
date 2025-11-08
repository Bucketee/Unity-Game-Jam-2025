using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public Item item = null;

    private void OnMouseDown()
    {
        Debug.Log("Touch");
        
    }
}
