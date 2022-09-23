using UnityEngine;
namespace WeaponSystem
{
    namespace Weapons
    {
        public class GrapplingHook : Weapon
        {
            [Header("Grappling Gun Values")]
            public Transform player;
            [SerializeField] float spring = 100, damper = 7, massScale = 4.5f;
            LineRenderer lr;
            Transform entity;
            SpringJoint joint;

            private void Awake()
            {
                lr = GetComponent<LineRenderer>();
            }
            void Update()
            {
                if (WeaponManager.hasWeapon)
                {
                    if (Input.GetKeyDown("r")) { Reolad(); }
                    if (Input.GetMouseButtonDown(0)) { Shoot(); }
                    if (Input.GetMouseButtonUp(0)) { StopGrapple(); }
                }
                else { StopGrapple(); }
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
                        entity = GetNearest();
                        curMagazine--;
                        if (entity)
                        {
                            //grapplepoint = GetRay().point;
                            //entity = Instantiate(projectile, grapplepoint, Quaternion.identity, GetRay().transform).transform;

                            //entity.LookAt(GetRay().transform);

                            if (entity.GetComponent<IDamage>() != null)
                            {
                                entity.GetComponent<IDamage>().TakeDamage(dmg);
                            }

                            joint = player.gameObject.AddComponent<SpringJoint>();
                            lr.positionCount = 2;
                            SetJoint();
                        }
                    }
                }
                else
                {
                    Reolad();
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
                joint.minDistance = distance * .8f;

                joint.spring = spring;
                joint.damper = damper;
                joint.massScale = massScale;
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
                //if (entity) { Destroy(entity.gameObject); }
            }

            Transform GetNearest()
            {
                Transform tmp = null;
                Collider[] objs = Physics.OverlapSphere(transform.position + Vector3.up, distance, rayMasks);
                float objdist = Mathf.Infinity;
                foreach (var obj in objs)
                {
                    float tmpdist = Vector3.Distance(obj.transform.position, transform.position);
                    if (objdist > tmpdist)
                    {
                        objdist = tmpdist;
                        tmp = obj.transform;
                    }
                }
                return tmp;
            }
        }
    }
}
