using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class GrabableController : MonoBehaviour
    {
        private Camera _camera;
        private Collider2D _collider;
        private GrabberController _grabbedBy = null;
        public bool beingTooled = false;
        private PlateController _plateController = null;
        //make public to set up in editor
        public PlaceableController isPlacedOn = null;
        public bool placeOnStart = false;
        public UnityEvent onGrabbed = new UnityEvent();
        public UnityEvent onPlaced = new UnityEvent();
      
        private void Start()
        {
            _camera = Camera.main;
            _collider = GetComponent<Collider2D>();
            _plateController = GetComponentInChildren<PlateController>();
         
        }

        public bool IsBeingGrabbed()
        {
            return _grabbedBy != null;
        }

        public GrabberController GetTentacleHolding()
        {
            return _grabbedBy;
        }

        public bool IsPlacedOnSomething()
        {
            return isPlacedOn != null;
        }

        private void OnDestroy()
        {
            if (_grabbedBy) _grabbedBy.isGrabbing = false;
            if (isPlacedOn) isPlacedOn.isEmpty = true;
        }

        public void GetGrabbed(GrabberController potentialGrabber)
        {
            if (!beingTooled && !potentialGrabber.isGrabbing && potentialGrabber.active)
            {
                 if(!placeOnStart) SFXController.Instance.PlayPickUp();
                potentialGrabber.isGrabbing = true;
                _grabbedBy = potentialGrabber;
                onGrabbed?.Invoke();
                if (isPlacedOn)
                {
                    isPlacedOn.isEmpty = true;
                    isPlacedOn = null;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isPlacedOn && _plateController)
            {
                _plateController.TryAddIngrediant(other.gameObject);
            }

            PlaceableController potentialSurface = other.GetComponent<PlaceableController>();
            if (placeOnStart && potentialSurface)
            {
                placeOnStart = false;
                Place(potentialSurface);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            //PICKUP
            if (!_grabbedBy && other.CompareTag("grabber") && isMouseDownOnCollider())
            {
                
                GetGrabbed(other.GetComponent<GrabberController>());

            }
            //PUTDOWN
            if (_grabbedBy && other.CompareTag("placeableSurface") && _grabbedBy.ReleasedRecently() && !Input.GetMouseButton(0))
            {
                PlaceableController potentialSurface = other.GetComponent<PlaceableController>();

                Place(potentialSurface);
            }
        }

        void Place(PlaceableController potentialSurface)
        {
            if (potentialSurface.isEmpty)
            {
                SFXController.Instance.PlayPutDown();
                isPlacedOn = potentialSurface;
                isPlacedOn.isEmpty = false;
                onPlaced?.Invoke();
                transform.position = potentialSurface.transform.position;
                    
                if(_grabbedBy) _grabbedBy.isGrabbing = false;
                _grabbedBy = null;
            }
        }
        private void Update()
        {
            if (_grabbedBy)
            {
                transform.position = _grabbedBy.transform.position;
            }

            if (isPlacedOn)
            {
                transform.position = isPlacedOn.transform.position;
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
}