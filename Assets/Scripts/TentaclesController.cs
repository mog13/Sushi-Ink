using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TentaclesController : MonoBehaviour
{
    
    private List<GrabberController> _tentacles = new List<GrabberController>();
    private AudioSource _source; 
    private int activeTentacle = 0;
    
    
    void Start()
    {
        _tentacles = GetComponentsInChildren<GrabberController>().ToList();
        if (_tentacles.Count > 0) _tentacles[0].active = true;

        _source = GetComponent<AudioSource>();
    }

    public void ShiftActiveTentacle(bool clockwise = true)
    {
        _tentacles[activeTentacle].active = false;
        _tentacles[activeTentacle].UpdateReleaseTimer();
        activeTentacle += clockwise ? 1 : -1;
        if (activeTentacle < 0) activeTentacle = _tentacles.Count - 1;
        if (activeTentacle >= _tentacles.Count) activeTentacle = 0;
        _tentacles[activeTentacle].active = true;
        _source.Play();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShiftActiveTentacle();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ShiftActiveTentacle(false);
        }
    }
}
