using UnityEngine;

public class GrapplingHook : Weapon
{
    private LineRenderer lr;
    Vector3 grapplepoint;
    Transform entity;
    SpringJoint joint;
    public Transform player;
    bool hitEnemy;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    public override void Shoot()
    {
        IDamage damage;
        hitEnemy = GetRay().transform.TryGetComponent<IDamage>(out damage);

        entity = GetRay().transform;
        grapplepoint = GetRay().point;
        if (grapplepoint != null)
        {
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplepoint;

            float distanceFromPoint = Vector3.Distance(
                player.position,
                grapplepoint);
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.15f;

            joint.spring = 4.5f;
            joint.damper = 7;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
        else
        {
            grapplepoint = firePoint.position;
        }
    }
    void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, firePoint.position);
        lr.SetPosition(1,
            hitEnemy ? entity.position : grapplepoint);
    }
    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }
}
