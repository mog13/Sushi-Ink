using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoveablePointController : MonoBehaviour
    {
        public Waypoint curWaypoint, rootWaypoint;
        public float conveyorSpeed, curBezierPos,sectionSpeed =-100;
        private ConveyorController _conveyor;

        private void Start()
        {
            _conveyor = GetComponentInParent<ConveyorController>();
        }

        public void getSpeedModifier(Waypoint p1, Waypoint p2)
        {
            float distance = _conveyor.getDistanceBetweenBeziers(p1, p2);
            sectionSpeed = 1f / distance;
        }


        Vector2 GetNextSpot(float dt)
        {
            Waypoint nextWayPoint = curWaypoint.nextWaypoint? curWaypoint.nextWaypoint : rootWaypoint;
            if (curBezierPos + dt > 1)
            {
                curWaypoint = nextWayPoint;
                curBezierPos = curBezierPos + dt - 1;
            }
            else curBezierPos += dt;
            
            return _conveyor.getBezierPointFromWaypoints(curBezierPos, curWaypoint, nextWayPoint);
        }

        void advanceThroughCurve(float distance)
        {
            float traveled = 0f;
            int panic = 0;
            Vector2 nextPos = Vector2.zero;
            while (traveled<distance)
            {
                nextPos = GetNextSpot(.0001f);
                traveled += Vector2.Distance(transform.position, nextPos);
                if(panic++ >100) break;
            }

            transform.position = nextPos;
        }

        private void FixedUpdate()
        {
            advanceThroughCurve(conveyorSpeed);
        }

        // private void FixedUpdate()
        // {
        //    
        //     Waypoint nextWayPoint = curWaypoint.nextWaypoint? curWaypoint.nextWaypoint : rootWaypoint;
        //     if(sectionSpeed <0) getSpeedModifier(curWaypoint, nextWayPoint);
        //     curBezierPos +=  conveyorSpeed *sectionSpeed;
        //     if (curBezierPos > 1)
        //     {
        //         curBezierPos = 0;
        //         getSpeedModifier(curWaypoint, nextWayPoint);
        //         curWaypoint = nextWayPoint;
        //     }
        //
        //     transform.position =
        //         _conveyor.getBezierPointFromWaypoints(curBezierPos, curWaypoint, nextWayPoint);
        // }
    }
}