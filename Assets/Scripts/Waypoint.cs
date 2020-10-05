using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;
    public ControlPoint cp1;
    public ControlPoint cp2;

    public Vector2 GetPos()
    {
        return transform.position;
    }
    
}
