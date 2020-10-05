using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

public class WaypointManagerWindow : EditorWindow
{
    [MenuItem("Tools/ConveryEditor")]
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform waypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        if (waypointRoot == null)
        {
            EditorGUILayout.HelpBox("Please select a root node", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    private void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }

        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
        {
            if (GUILayout.Button("Create before")) CreateBefore();
            if (GUILayout.Button("Create After")) CreateAfter();
            if (GUILayout.Button("Remove")) Remove();
        }
    }

     GameObject MakeNewWayPoint()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);
        
        GameObject cp1 = new GameObject("cp1 ", typeof(ControlPoint));
        cp1.transform.SetParent(waypointObject.transform,false);
        cp1.transform.position = waypointObject.transform.position + Vector3.up;
        waypointObject.GetComponent<Waypoint>().cp1 = cp1.GetComponent<ControlPoint>();
        
        GameObject cp2 = new GameObject("cp2 ", typeof(ControlPoint));
        cp2.transform.SetParent(waypointObject.transform,false);
        cp2.transform.position = waypointObject.transform.position + Vector3.down;
        waypointObject.GetComponent<Waypoint>().cp2 = cp2.GetComponent<ControlPoint>();
        
        return waypointObject;
    }
    void CreateBefore()
    {
        GameObject waypointObject = MakeNewWayPoint();
        
        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        if (selectedWaypoint.previousWaypoint != null)
        {
            newWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            selectedWaypoint.previousWaypoint.nextWaypoint = newWaypoint;
        }

        newWaypoint.nextWaypoint = selectedWaypoint;
        selectedWaypoint.previousWaypoint = newWaypoint;
        
        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
        Selection.activeGameObject = newWaypoint.gameObject;
    }

    void CreateAfter()
    {
        GameObject waypointObject = MakeNewWayPoint();
        
        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        newWaypoint.previousWaypoint = selectedWaypoint;

        if (selectedWaypoint.nextWaypoint != null)
        {
            selectedWaypoint.nextWaypoint.previousWaypoint = newWaypoint;
            newWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
        }
        
        selectedWaypoint.nextWaypoint = newWaypoint;
        
        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex()+1);
        Selection.activeGameObject = newWaypoint.gameObject;
    }

    void Remove()
    {
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        if (selectedWaypoint.nextWaypoint != null)
        {
            selectedWaypoint.nextWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;

        }

        if (selectedWaypoint.previousWaypoint != null)
        {
            selectedWaypoint.previousWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
            Selection.activeGameObject = selectedWaypoint.previousWaypoint.gameObject;
        }
        
        DestroyImmediate(selectedWaypoint.gameObject);
    }

    private void CreateWaypoint()
    {
        GameObject waypointObject = MakeNewWayPoint();
        
        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        if (waypointRoot.childCount > 1)
        {
            waypoint.previousWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>();
            waypoint.previousWaypoint.nextWaypoint = waypoint;
            var pos = waypoint.transform;
            var prevPos = waypoint.previousWaypoint.transform;
            pos.position = prevPos.position;
            pos.forward = prevPos.forward;
        }

        Selection.activeGameObject = waypoint.gameObject;
    }
}