using UnityEngine;

public class DebugTimeChange : MonoBehaviour, IListener
{
    void Start()
    {
        TimeManager.Instance.TimeChangeListener.Add(this);
    }

    private void DebugTime(int i)
    {
        Debug.Log($"Time Changed to {i}");
    }

    public void OnEvent(object param1 = null, object param2 = null)
    {
        DebugTime((int)param1);
    }

    public void OnDestroy()
    {
        TimeManager.Instance.TimeChangeListener.Remove(this);
    }

    public void OnDisable()
    {
        TimeManager.Instance.TimeChangeListener.Remove(this);
    }
}
