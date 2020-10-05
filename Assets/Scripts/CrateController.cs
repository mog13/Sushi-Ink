using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    public GameObject contentsPrefab;

    public Transform grabableParent;

    private GameObject _contents;
   
    private Camera _camera;
    private Collider2D _collider;
    private void Start()
    {
        _camera = Camera.main;
        _collider = GetComponent<Collider2D>();
    }


    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("grabber")  && isMouseDownOnCollider())
        {
            GrabberController potentialGrabber = other.GetComponent<GrabberController>();
            if (!potentialGrabber.isGrabbing && potentialGrabber.active)
            {
                _contents = Instantiate(contentsPrefab, transform.position, transform.rotation, grabableParent);
                _contents.GetComponent<GrabableController>().GetGrabbed(potentialGrabber);
            }
        }

    }

    private bool isMouseOnCollider()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        return _collider.OverlapPoint(mousePosition);

    }
    private bool isMouseDownOnCollider()
    {
        return Input.GetMouseButton(0) && isMouseOnCollider();
    }

    private bool isMouseReleasedOnCollider()
    {
        return !Input.GetMouseButton(0) && isMouseOnCollider();
    }
}
