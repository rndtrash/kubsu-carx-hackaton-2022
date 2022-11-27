using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 0.5f;   
    
    public GameObject PlayerStart;
    
    public GameObject WaypointStart
    {
        get => _waypointStart;
        set
        {
            _waypointStart = value;
            _waypointStartT = value.GetComponent<Transform>();
        }
    }
    public GameObject WaypointFinish
    {
        get => _waypointFinish;
        set
        {
            _waypointFinish = value;
            _waypointFinishT = value.GetComponent<Transform>();
        }
    }

    private DateTime _start;
    private float _timeToReach;
    // private Queue<GameObject> Path;

    private GameObject _waypointStart;
    private GameObject _waypointFinish;

    private Transform _transform;
    private Transform _waypointStartT;
    private Transform _waypointFinishT;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        // Path = new();

        WaypointStart = WaypointFinish = PlayerStart;
        _transform.position = PlayerStart.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        Navigate();
    }

    public void StartMovement(GameObject wp)
    {
        /*var path = WaypointFinish.GetComponent<Waypoint>().Navigate(wp);
        Path.Clear();
        foreach (var p in path)
        {
            Path.Enqueue(p);
        }

        if (Path.Count == 0)
            return;
        
        if (WaypointStart == WaypointFinish)
        {
            WaypointFinish = Path.Dequeue();
        }*/
        if (WaypointStart != WaypointFinish)
        {
            Debug.LogWarning("Ещё не доехал!");
            return;
        }
        
        if (!WaypointFinish.GetComponent<Waypoint>().IsNear(wp))
        {
            Debug.LogWarning("Точка не рядом!");
            return;            
        }

        WaypointFinish = wp;
        
        _start = DateTime.UtcNow;
        _timeToReach = Vector3.Distance(_waypointStartT.position, _waypointFinishT.position) / Speed;
    }

    private void Navigate()
    {
        if (/*Path.Count == 0 || */WaypointStart == WaypointFinish || _timeToReach == 0)
            return;

        var difference = (float) (DateTime.UtcNow - _start).TotalSeconds;

        if (difference >= _timeToReach)
        {
            //var next = Path.Dequeue();
            WaypointStart = WaypointFinish;
            //WaypointFinish = next;
        }
        else
        {
            transform.position =
                Vector3.Lerp(_waypointStartT.position, _waypointFinishT.position, difference / _timeToReach);
        }
    }
}
