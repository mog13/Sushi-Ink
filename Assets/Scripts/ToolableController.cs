using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(GrabableController))]
    public class ToolableController : MonoBehaviour
    {
        private GrabableController _grabberController;
        public GameObject producesPrefab;
        public float timeRequired = 2f;
        public Transform grabbaleParent;
        public string toolTag = "knife";
       
        private void Start()
        {
            _grabberController = GetComponent<GrabableController>();
        
        }
        
        IEnumerator DoAction(GrabberController tentacle)
        {
            // suspend execution for 5 seconds
            yield return new WaitForSeconds(timeRequired);
           NewIngredientCreated(tentacle);
        }

        public void NewIngredientCreated(GrabberController tentacle)
        {
            tentacle.EndActivity();
            GameObject newObj = Instantiate(producesPrefab, transform.position, Quaternion.identity, grabbaleParent);
            GrabableController newGrab = newObj.GetComponent<GrabableController>();
            if (newGrab)
            {
                newGrab.isPlacedOn = _grabberController.isPlacedOn;
            }

          
            _grabberController.isPlacedOn.isEmpty = false;
            Destroy(gameObject);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(toolTag) && _grabberController.IsPlacedOnSomething())
            {
                GrabableController tool = other.GetComponent<GrabableController>();
                if(!tool) tool = other.GetComponentInParent<GrabableController>();
                if (tool && tool.IsBeingGrabbed())
                {
                   GrabberController tentacle =  other.GetComponentInParent<GrabableController>().GetTentacleHolding();
                   if (!tentacle.isPerformingAction)
                   {
                       tentacle.StartActivity(_grabberController.isPlacedOn.transform,toolTag);
                       _grabberController.beingTooled = true;
                       StartCoroutine(DoAction(tentacle));
                   }
                }
                
            }
        }
    }
}