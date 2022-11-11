using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollHandler : MonoBehaviour
{
    [SerializeField] Rigidbody[] RagdollRigidBodies;
    [SerializeField] Collider[] RagdollColliders;
    Rigidbody AnimatedRigidbody;
    CapsuleCollider AnimatedColider;
    [SerializeField] Animator anim;

    public bool startvalue = false;

    // Start is called before the first frame update
    void Start()
    {
        AnimatedRigidbody = GetComponent<Rigidbody>();
        AnimatedColider = GetComponent<CapsuleCollider>();
        SetRagDollOFF();
        //SetRagDoll(startvalue);
    }

    public void SetRagDollON()
    {
        //AnimatedRigidbody.isKinematic = enabled;
        //AnimatedColider.enabled = !enable;
        anim.enabled = false;

        foreach(Rigidbody rb in RagdollRigidBodies)
        {
            rb.isKinematic = true;
        }
        foreach(Collider col in RagdollColliders)
        {
            col.enabled = true;
        }
    }

    public void SetRagDollOFF()
    {
        //AnimatedRigidbody.isKinematic = enabled;
        //AnimatedColider.enabled = !enable;
        anim.enabled = true;

        foreach(Rigidbody rb in RagdollRigidBodies)
        {
            rb.isKinematic = false;
        }
        foreach(Collider col in RagdollColliders)
        {
            col.enabled = false;
        }
    }

    public void SetTmpRagDoll(int time)
    {
        bool enabled = time == 1;
        anim.enabled = !enabled;

        foreach(Rigidbody rb in RagdollRigidBodies)
        {
            rb.isKinematic = !enabled;
        }
        foreach(Collider col in RagdollColliders)
        {
            col.enabled = enabled;
        }
    }
}
