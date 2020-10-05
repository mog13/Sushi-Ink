using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class TentacleBuilder : MonoBehaviour
{
    public int parts = 3;

    [Header("tentacle parts")] public GameObject tentaclePrefab;
    public GameObject grabberPrefab;
    public GameObject basePrefab;
    public bool floatingTentacle = false;
    private LineRenderer _lineRenderer;

    private MaterialPropertyBlock _propBlock;

    //on left 1, else -1
    private bool onLeft = false;
    public GrabberController _grabber;
    private bool _previouslyActive = true;
    private void Start()
    {
        
        _lineRenderer = GetComponent<LineRenderer>();
 
        _propBlock = new MaterialPropertyBlock();
        _grabber = GetComponentInChildren<GrabberController>();
        onLeft = _grabber.transform.position.x < transform.position.x;
    }

    private void Update()
    {
        RegenLineRenderer();

        bool shouldBeOnLeft = _grabber.transform.position.x < transform.position.x;
        bool currentlyActive = _grabber.active;
        if (shouldBeOnLeft != onLeft || currentlyActive!= _previouslyActive)
        {
            _lineRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetVector("_MainTex_ST", new Vector4(1, shouldBeOnLeft ? 1 : -1, 0, 0));
            _propBlock.SetColor("_Color", Color.white * (currentlyActive? 1f:.8f));
            _lineRenderer.SetPropertyBlock(_propBlock);
            onLeft = shouldBeOnLeft;
            _previouslyActive = currentlyActive;
        }
    }

    [ContextMenu("why is he so dangly?")]
    public void RegenLineRenderer()
    {
        if (!_lineRenderer) _lineRenderer = GetComponent<LineRenderer>();
        Vector3[] parts = gameObject.GetComponentsInChildren<Rigidbody2D>()
            .Select(p => p.transform.position).ToArray();
    
        _lineRenderer.positionCount = parts.Length;
        _lineRenderer.SetPositions(parts);
    }

    [ContextMenu("Unleash the kraken")]
    public void BuildTentacle()
    {
        float heightOffset = .5f;
        HingeJoint2D hingeJoint = GetComponent<HingeJoint2D>();
        FixedJoint2D fixedJoint = GetComponent<FixedJoint2D>();
        Joint2D _rootHinge;
        if (floatingTentacle)
        {
            hingeJoint.enabled = false;
            fixedJoint.enabled = true;
            _rootHinge = fixedJoint;
        }
        else
        {
            hingeJoint.enabled = true;
            fixedJoint.enabled = false;
            _rootHinge = hingeJoint;
        }
  

        var tempList = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in tempList)
        {
            if (child.gameObject.GetInstanceID() != gameObject.GetInstanceID()) DestroyImmediate(child.gameObject);
        }
    
        if(floatingTentacle) Instantiate(basePrefab, transform);
        
        Joint2D prevPart = _rootHinge.GetComponent<Joint2D>();
        GameObject curPart = null;
        for (int i = 0; i < parts; i++)
        {
            curPart = Instantiate(tentaclePrefab, transform.position, quaternion.identity, transform);
            heightOffset -= curPart.GetComponent<SpriteRenderer>().size.y * curPart.transform.localScale.y;
            curPart.GetComponent<SpriteRenderer>().enabled = false;
            curPart.transform.position += new Vector3(0, heightOffset, 0);
            prevPart.connectedBody = curPart.GetComponent<Rigidbody2D>();
            prevPart = curPart.GetComponent<HingeJoint2D>();
        }

        GameObject grabber = Instantiate(grabberPrefab, transform.position, transform.rotation, transform);
        grabber.transform.position += new Vector3(0, heightOffset - .5f, 0);
        prevPart.connectedBody = grabber.GetComponent<Rigidbody2D>();
        RegenLineRenderer();
    }
}