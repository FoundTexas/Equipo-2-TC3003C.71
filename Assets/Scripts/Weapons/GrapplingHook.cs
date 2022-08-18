using UnityEngine;

public class GrapplingHook : Weapon
{
    private LineRenderer lr;
    Vector3 grapplepoint;
    Transform entity;
    SpringJoint joint;
    public Transform player;
    public GameObject claw;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    void Update()
    {
        if (Input.GetKeyDown("r")) { Reolad(); }
        if (Input.GetMouseButtonDown(0)) { Shoot(); }
        if (Input.GetMouseButtonUp(0)) { StopGrapple(); }
    }
    private void LateUpdate()
    {
        DrawRope();
    }

    public override void Shoot()
    {
        if (curMagazine > 0)
        {
            if (curShootS <= 0)
            {
                curShootS = shootSpeed;
                PlayShootAnimation();
                if (GetRay().transform)
                {
                    grapplepoint = GetRay().point;
                    entity = Instantiate(projectile, grapplepoint, Quaternion.identity, GetRay().transform).transform;

                    entity.LookAt(GetRay().transform);
        
                    if (GetRay().transform.GetComponent<IDamage>() != null)
                    {
                        GetRay().transform.GetComponent<IDamage>().TakeDamage(dmg);
                    }

                    joint = player.gameObject.AddComponent<SpringJoint>();
                    lr.positionCount = 2;
                    curMagazine--;
                    SetJoint();
                }
                else
                {
                    grapplepoint = firePoint.position;
                }
            }
        }
    }

    void SetJoint()
    {
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = entity.position;

        float distanceFromPoint = Vector3.Distance(
            player.position,
            entity.position);
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.3f;

        joint.spring = 12;
        joint.damper = 7;
        joint.massScale = 4.5f;
    }
    void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, firePoint.position);
        lr.SetPosition(1, entity.position);
        
        SetJoint();
    }
    void StopGrapple()
    {
        lr.positionCount = 0;
        if (joint) { Destroy(joint); }
        if (entity) { Destroy(entity.gameObject); }
    }
}
