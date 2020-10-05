using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class KnifeController : MonoBehaviour
    {
        private GrabableController _grabable;

        private void Start()
        {
            _grabable = GetComponentInParent<GrabableController>();
        }

        public bool IsBeingHeld()
        {
            return _grabable.IsBeingGrabbed();
        }
    }
}