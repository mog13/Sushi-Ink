using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
public class ConveyorController : MonoBehaviour
{
    LineRenderer _lineRenderer;
    public Waypoint waypointRoot;
    public float width = .4f;
    public float sushiPointLayoutFraction = .7f;
    public int panicThreshold = 9000;

    public float conveyorSpeed = -.001f;
    public float conveyorItemSpeed = .001f;
    private float _offset = 0;

    public GameObject sushiPointPrefab;
    private GameObject pointParent;

    private void Start()
    {
        initLineRenderer();
        if (Application.isPlaying) GenerateAllSushiPoints();
    }

    public void GenerateAllSushiPoints()
    {
        if (!pointParent) pointParent = Instantiate(new GameObject("SushiPointHolder "), transform);
        if (waypointRoot)
        {
            Waypoint curWaypoint = waypointRoot;
            int iterations = 0;
            while (curWaypoint.nextWaypoint)
            {
                GenerateSushiPointsBetweenTwoWaypoints(curWaypoint, curWaypoint.nextWaypoint);
                curWaypoint = curWaypoint.nextWaypoint;
                if (iterations++ > panicThreshold)
                {
                    throw new Exception("cyclical waypoints found and i dont have time to check");
                }
            }

            GenerateSushiPointsBetweenTwoWaypoints(curWaypoint, waypointRoot);
        }
    }

    public void GenerateSushiPointsBetweenTwoWaypoints(Waypoint p1, Waypoint p2)
    {
        float points = getDistanceBetweenBeziers(p1,p2) / (width * sushiPointLayoutFraction);
        for (var i = 1; i <= points; i++)
        {
            float t = 1f / points * i;
            Vector2 pos = getBezierPointFromWaypoints(t, p1, p2);
           GameObject sushiPoint = Instantiate(sushiPointPrefab, pos, Quaternion.identity, pointParent.transform);
           MoveablePointController sushiPointCtrl = sushiPoint.GetComponent<MoveablePointController>();
           sushiPointCtrl.curWaypoint = p1;
           sushiPointCtrl.rootWaypoint = waypointRoot;
           sushiPointCtrl.curBezierPos = t;
           sushiPointCtrl.conveyorSpeed = conveyorItemSpeed;
        }
        // Debug.Log("distanc bewteen " + p1 + " and " + p2);
        // Debug.Log(getDistanceBetweenBeziers(p1,p2));
    }

    void GetLineRender()
    {
        if (!_lineRenderer)
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = true;
        }
    }

    private void Update()
    {
        UpdateLineRenderer();
        if (!Application.isPlaying) initLineRenderer();
    }


    void UpdateLineRenderer()
    {
        _offset += conveyorSpeed;
        if (Application.isPlaying) _lineRenderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(_offset, 0));
    }

    [ContextMenu("Update line renderer")]
    public void initLineRenderer()
    {
        GetLineRender();
        _lineRenderer.startColor = Color.gray;
        _lineRenderer.endColor = Color.gray;
        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth = width;
        List<Vector2> linePositions = GenerateLinePoints();
        _lineRenderer.positionCount = linePositions.Count;
        _lineRenderer.SetPositions(linePositions.Select(v => new Vector3(v.x, v.y, 0)).ToArray());
    }

    List<Vector2> GenerateLinePoints()
    {
        List<Vector2> bezierPoints = new List<Vector2>();

        if (waypointRoot)
        {
            Waypoint curWaypoint = waypointRoot;
            int iterations = 0;
            while (curWaypoint.nextWaypoint)
            {
                //@todo this wont work for branching
                List<Vector2> newPoints = GetBezierSegment(curWaypoint, curWaypoint.nextWaypoint);
                bezierPoints = bezierPoints.Concat(newPoints).ToList();
                curWaypoint = curWaypoint.nextWaypoint;
                if (iterations++ > panicThreshold)
                {
                    throw new Exception("cyclical waypoints found and i dont have time to check");
                }
            }

            //close the loop
            List<Vector2> closePoints = GetBezierSegment(curWaypoint, waypointRoot);
            bezierPoints = bezierPoints.Concat(closePoints).ToList();
        }

        return bezierPoints;
    }

    List<Vector2> GetBezierSegment(Waypoint wp1, Waypoint wp2)
    {
        List<Vector2> bezierPoints = new List<Vector2>();
        for (var t = 0f; t < 1f; t += .05f)
        {
            bezierPoints.Add(
                GetBezierPoint(
                    t,
                    wp1.GetPos(),
                    wp1.cp2.GetPos(),
                    wp2.cp1.GetPos(),
                    wp2.GetPos()
                ));
        }

        return bezierPoints;
    }

    public float getDistanceBetweenBeziers(Waypoint wp1,Waypoint wp2)
    {

        Vector2 p0 = wp1.GetPos();
        Vector2 p1 = wp1.cp2.GetPos();
        Vector2 p2 = wp2.GetPos();
        Vector2 p3 = wp1.cp1.GetPos();
        float chord = Vector2.Distance(p3,p0);
        float cont_net = Vector2.Distance(p0,p1) +
                         Vector2.Distance(p2,p1) +
                         Vector2.Distance(p3,p2);
        return  (cont_net + chord) / 2;
    }
    public Vector2 getBezierPointFromWaypoints(float t, Waypoint wp1, Waypoint wp2)
    {
        return GetBezierPoint(
            t,
            wp1.GetPos(),
            wp1.cp2.GetPos(),
            wp2.cp1.GetPos(),
            wp2.GetPos()
        );
    }

    Vector2 GetBezierPoint(float t,
        Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 p = uuu * p0; //first term
        p += 3 * uu * t * p1; //second term
        p += 3 * u * tt * p2; //third term
        p += ttt * p3; //fourth term

        return p;
    }
}