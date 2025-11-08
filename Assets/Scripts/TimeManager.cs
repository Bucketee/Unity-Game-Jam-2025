using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    
    [SerializeField] private int timeLineCount;
    [SerializeField] private int currentTimeLine = 0;

    public List<IListener> TimeChangeListener = new List<IListener>();

    private void Awake()
    {
        if (Instance != null) DestroyImmediate(this.gameObject);
        Instance = this;
    }

    public void SwitchTimeLine(int i)
    {
        if (currentTimeLine + i < 0 || currentTimeLine + i >= timeLineCount)
        {
            Debug.Log("No More TimeLine");
            return;
        }
        currentTimeLine = Mathf.Clamp(currentTimeLine + i, 0, timeLineCount - 1);
        
        foreach (IListener t in TimeChangeListener)
        {
            t.OnEvent(currentTimeLine);
        }
    }
}
