using System;
using UnityEngine;

public class InvadeEvent : IEvent
{
    public bool IsEmergent() => false;

    private DateTime _timer;
    private GameObject _eventPoint;

    public bool StartEvent(GameObject eventPoint)
    {
        _eventPoint = eventPoint;
        _timer = DateTime.UtcNow;
        return true;
    }

    public void Clicked()
    {
        
    }

    public void Update()
    {
        if ((DateTime.UtcNow - _timer).TotalSeconds >= 30f)
        {
            var ep = _eventPoint.GetComponent<EventPoint>();
            ep.StopEvent();
        }
    }
}