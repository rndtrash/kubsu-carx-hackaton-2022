using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public GameObject[] Connections;

    private Waypoint[] _waypoints;
    private float[] _weights;

    private Transform t;

    private void Start()
    {
        if (Connections.Length == 0)
            throw new Exception("У этой точки нет соединений!");

        t = GetComponent<Transform>();

        _waypoints = new Waypoint[Connections.Length];
        _weights = new float[Connections.Length];
        for (var i = 0; i < Connections.Length; i++)
        {
            var go = Connections[i];
            var goT = go.GetComponent<Transform>();

            var weight = Vector3.Distance(t.position, goT.position);
            _weights[i] = weight;

            _waypoints[i] = go.GetComponent<Waypoint>();
        }
    }

    public GameObject[] Navigate(GameObject destination)
    {
        return PrivateNavigate(destination, new GameObject[]{});
    }

    public bool IsNear(GameObject point)
    {
        return Connections.Contains(point);
    }

    private GameObject[] PrivateNavigate(GameObject destination, GameObject[] visited)
    {
        if (destination == this.GameObject())
            return visited;
        
        var visitedH = new HashSet<GameObject>(visited);
        if (visitedH.Contains(this.GameObject()))
            return visited;

        visitedH.Add(this.GameObject());

        GameObject[] min = null;
        for (var i = 0; i < Connections.Length; i++)
        {
            if (visitedH.Contains(Connections[i]))
                continue;
            
            var result = _waypoints[i].PrivateNavigate(destination, visitedH.ToArray());
            if (min is null || result.Length < min.Length)
                min = result;
        }

        return min;
    }

    private void OnMouseDown()
    {
        var p = GameObject.FindWithTag("Player");
        var pl = p.GetComponent<Player>();
        pl.StartMovement(this.GameObject());
    }
}
