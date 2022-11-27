using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryEvent : IEvent
{
    private static bool _hasEvent = false;
    public bool IsEmergent() => true;

    public bool StartEvent(GameObject eventPoint)
    {
        if (_hasEvent)
            return false;
        
        _hasEvent = true;
        return true;
    }

    public void Clicked()
    {
        SceneManager.LoadScene("Story");
    }

    public void Update()
    {
    }
}
