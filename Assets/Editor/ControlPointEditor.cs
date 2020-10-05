using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad()]
public class ContolPointEditor
{
    [DrawGizmo((GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable))]
    public static void OnDrawSceneGizmo(ControlPoint controlPoint, GizmoType gizmoType)
    {
    
        Gizmos.color = Color.cyan * ((gizmoType & GizmoType.Selected) !=0 ? 1 : .5f);

        var position = controlPoint.transform.position;
        Gizmos.DrawSphere(position,.05f);
        Gizmos.color = Color.white *.7f;
        Waypoint parent = controlPoint.GetComponentInParent<Waypoint>();
      
        if(parent) Gizmos.DrawLine(position, parent.transform.position);
        
        
    }
}