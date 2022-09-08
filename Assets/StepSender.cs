using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSender : MonoBehaviour
{
    public LayerMask groundMask;
    public AudioAndVideoManager anim;
    public void SendStep()
    {
        RaycastHit hit;
        anim.StepSound( Physics.Raycast(transform.position, Vector3.down, out hit, 1, groundMask) ? 
            hit.transform.CompareTag("Untagged")? "Concrete": hit.transform.tag : "Concrete" );
    }
}
