using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Props
{
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Platform Stats")]

        [Tooltip("Direction vector movement of the platform")]
        public Vector3 Dir;
        [Tooltip("Transform positions of Raycast to check if collided with other platform")]
        public Transform[] colliders;
        [Tooltip("Layers of what can be collided with the transform colliders")]
        public LayerMask rayMasks;
        [Tooltip("Velocity value of the platform while moveing")]
        public float speed;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        void Update()
        {
            transform.Translate(Dir * speed * Time.deltaTime);

            if (Collided())
            {
                Dir *= -1;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.transform.parent = transform;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.transform.parent = null;
            }
        }

        // ----------------------------------------------------------------------------------------------- Unity Methods

        // this bollean returns if any of the RayCasts at the 6 faces of the platform returns a valid Transform.
        bool Collided()
        {
            foreach (var obj in colliders)
            {
                if (GetRay(obj).transform)
                {
                    return true;
                }
            }
            return false;
        }
        // This function instantiates a Raycast Hit in a given direction and returns what ever it hists or null
        RaycastHit GetRay(Transform direction)
        {
            RaycastHit tmp = new RaycastHit();
            if (Physics.Raycast(direction.position, direction.forward,
                out RaycastHit hitinfo, 0.1f, rayMasks))
            {
                tmp = hitinfo;
            }
            return tmp;
        }
    }
}
