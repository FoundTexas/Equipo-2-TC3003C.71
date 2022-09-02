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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
