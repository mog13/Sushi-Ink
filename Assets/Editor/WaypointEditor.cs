using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad()]
public class WaypointEditor
{
    [DrawGizmo((GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable))]
    public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
    {
    
        Gizmos.color = Color.green * ((gizmoType & GizmoType.Selected) !=0 ? 1 : .5f);

        var position = waypoint.transform.position;
        Gizmos.DrawSphere(position,.1f);
        Gizmos.color = Color.white;
         if(waypoint.nextWaypoint) Gizmos.DrawLine(position, waypoint.nextWaypoint.transform.position);
 
        
    }
}