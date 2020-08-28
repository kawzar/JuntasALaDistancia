using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField]
    private Waypoint[] waypoints;

    [SerializeField]
    private string pathTag;

    private int currentIndex = 0;

    public string PathTag => pathTag;

    public bool CanContinue()
    {
        return currentIndex < waypoints.Length;
    }

    public Transform NextWaypoint()
    {
        Transform waypoint = waypoints[currentIndex].transform;
        currentIndex++;

        return waypoint;
    }
}
