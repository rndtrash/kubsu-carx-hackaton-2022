using System;
using UnityEngine;

public class ChaseEvent : IEvent
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
        PoliceManager.Instance.StartMinigame(0, this);
    }

    public void Update()
    {
        if ((DateTime.UtcNow - _timer).TotalSeconds >= 20f)
        {
            var ep = _eventPoint.GetComponent<EventPoint>();
            ep.StopEvent();
        }
    }
}
