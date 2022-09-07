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

    public bool startvalue = true;

    // Start is called before the first frame update
    void Start()
    {
        AnimatedRigidbody = GetComponent<Rigidbody>();
        AnimatedColider = GetComponent<CapsuleCollider>();

        SetRagDoll(startvalue);
    }

    public void SetRagDoll(bool enable)
    {
        //AnimatedRigidbody.isKinematic = enabled;
        //AnimatedColider.enabled = !enable;
        anim.enabled = !enable;

        foreach(Rigidbody rb in RagdollRigidBodies)
        {
            rb.isKinematic = !enabled;
        }
        foreach(Collider col in RagdollColliders)
        {
            col.enabled = enabled;
        }
    }

    public void SetTmpRagDoll(float time)
    {

    }
}
