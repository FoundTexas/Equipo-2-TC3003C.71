using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 Dir;
    public Transform[] colliders;
    public LayerMask rayMasks;
    public float speed;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Dir* speed*Time.deltaTime);

        if (Collided())
        {
            Dir *= -1;
        }
    }

    bool Collided()
    {
        foreach(var obj in colliders)
        {
            if (GetRay(obj).transform)
            {
                return true;
            }
        }
        return false;
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
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
}
