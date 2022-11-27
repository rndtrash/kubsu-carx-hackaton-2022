using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class EventPoint : MonoBehaviour
{
    public Object EmergentIcon;
    public Object CallIcon;

    public IEvent[] PossibleEvents =
    {
        new StoryEvent(),
        new InvadeEvent(),
        //new ChaseEvent()
    };

    private PoliceManager _policeManager;
    private IEvent _activeEvent = null;
    private DateTime _lastActivation;
    private Object _popup = null;

    private void Start()
    {
        _lastActivation = DateTime.UtcNow;
        _policeManager = GameObject.FindWithTag("PoliceManager").GetComponent<PoliceManager>();
    }

    public void StopEvent()
    {
        Destroy(_popup);
        _activeEvent = null;
        _lastActivation = DateTime.UtcNow;
        _policeManager.EndEvent();
    }

    private void Update()
    {
        if (_activeEvent is null)
        {
            if ((DateTime.UtcNow - _lastActivation).TotalSeconds <= 5f || !_policeManager.HasFreeEvents())
                return;

            _lastActivation = DateTime.UtcNow;
            var activate = Random.Range(0f, 1f) > 0.1f;

            if (activate)
            {

                _activeEvent = PossibleEvents[Random.Range(0, PossibleEvents.Length)];
                if (!_activeEvent.StartEvent(this.GameObject()))
                    return;
                
                _policeManager.AddEvent();

                _popup = Instantiate(_activeEvent.IsEmergent() ? EmergentIcon : CallIcon, transform);
            }
        }
        else
        {
            _activeEvent.Update();
        }
    }

    private void OnMouseDown()
    {
        if (_activeEvent is not null && !_policeManager.EventInProgress)
        {
            if (Vector3.Distance(GameObject.FindWithTag("Player").GetComponent<Transform>().position,
                    transform.position) > 2f)
            {
                Debug.LogWarning("Слишком далеко, подъедь к точке!");
                Debug.LogWarning($"{Vector3.Distance(GameObject.FindWithTag("Player").GetComponent<Transform>().position, transform.position)}");
                return;
            }

            _policeManager.EventInProgress = true;
            _activeEvent.Clicked();
        }
    }
}
