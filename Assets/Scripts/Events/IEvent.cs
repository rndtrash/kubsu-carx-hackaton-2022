using UnityEngine;

public interface IEvent
{
    public bool IsEmergent();
    public bool StartEvent(GameObject eventPoint);
    public void Clicked();
    public void Update();
}