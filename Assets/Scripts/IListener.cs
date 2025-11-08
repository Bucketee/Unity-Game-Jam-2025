using Unity.VisualScripting;
using UnityEngine;

public interface IListener
{
    void OnEvent(object param1 = null, object param2 = null);
}
