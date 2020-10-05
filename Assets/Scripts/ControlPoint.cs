using UnityEngine;

namespace DefaultNamespace
{
    public class ControlPoint : MonoBehaviour
    {
        
        public Vector2 GetPos()
        {
            return transform.position;
        }
    }
}