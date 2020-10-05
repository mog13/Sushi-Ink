using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GrabberController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Camera _camera;
    
    private float lastMouseDown = 0f;
    public bool isOnMenu = false;
    public bool active = false;
    public bool isPerformingAction = false;
    public Transform performingOn = null;
    public bool isGrabbing = false;
    [Header("movement feel")] public float movementDamping = .1f;
    
    
    public GameObject knifeEffect;
    public GameObject riceEffect;
    private GameObject _effect;
    private AudioSource _audio;
    void Start()
    {
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
        
        TentacleBuilder _base = GetComponentInParent<TentacleBuilder>();
        Vector3 basePos = _base.transform.position;
        Vector3 dir = transform.position - basePos + Vector3.Cross(basePos,Vector3.left)*5f;
        float mag = Vector3.Magnitude(dir);
        //new Vector3(Random.Range(0,3),Random.Range(0,3),0)
        _rb.MovePosition(transform.position  + new Vector3(Random.Range(-3,3),Random.Range(-3,3),0));
    }
    
    // Update is called once per frame
    void Update()
    {
        if ((active && !isPerformingAction && Input.GetMouseButton(0)) || isOnMenu)
        {
            Vector3 mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
            
            _rb.MovePosition(Vector3.Lerp(transform.position,mouse,movementDamping));
        }

        if (isPerformingAction)
        {
            _rb.MovePosition(performingOn.position);
        }

        if (active && Input.GetMouseButtonUp(0)) UpdateReleaseTimer();
    }

    public void UpdateReleaseTimer()
    {
        lastMouseDown = Time.time;
    }
    public void StartActivity(Transform t, string activity)
    {
        isPerformingAction = true;
        performingOn = t;

        switch (activity)
        {
            case "knife":
                if (knifeEffect) _effect = Instantiate(knifeEffect,t.position, knifeEffect.transform.rotation);
                _audio.Play();
                break;
            case "scoop": 
                if (riceEffect) _effect = Instantiate(riceEffect, t.position, riceEffect.transform.rotation);
                break;
        }
      
    }

    public bool ReleasedRecently()
    {
        return Time.time - lastMouseDown < .1f;
    }

    public void EndActivity()
    {
        isPerformingAction = false;
        performingOn = null;
        
        if (_effect)
        {
            Destroy(_effect);
            _effect = null;
        }
        _audio.Stop();
    }
    
    
}
