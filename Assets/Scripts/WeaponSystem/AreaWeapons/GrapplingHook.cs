using UnityEngine;
namespace WeaponSystem
{
    namespace misc
    {
        public class GrapplingHook : AreaWeapon
        {
            [Header("Grappling Gun Values")]
            [SerializeField] float spring = 100, damper = 7, massScale = 4.5f;
            LineRenderer lr;
            Transform entity;
            SpringJoint joint;

            // ----------------------------------------------------------------------------------------------- Unity Methods

            private void Awake()
            {
                lr = GetComponent<LineRenderer>();
            }
            void Update() //Overrided
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

            // ----------------------------------------------------------------------------------------------- Private Methods
            // Funtion in charge of setting the spring joint points between the hooked position and player.
            void SetJoint()
            {
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = entity.position;

                float distanceFromPoint = Vector3.Distance(
                    PlayerRef.transform.position,
                    entity.position);
                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distance * .8f;

                joint.spring = spring;
                joint.damper = damper;
                joint.massScale = massScale;
            }
            // Void that handels the line renderer to set the rope visualization.
            void DrawRope()
            {
                if (!joint) return;
                lr.SetPosition(0, firePoint.position);
                lr.SetPosition(1, entity.position);

                SetJoint();
            }
            // Void that handels when the hook is stopped.
            void StopGrapple()
            {
                lr.positionCount = 0;
                if (joint) { Destroy(joint); }
            }

            // ----------------------------------------------------------------------------------------------- Overrided Methods

            /// <summary>
            /// Overrided Shoot Method for GrapplingHook.
            /// </summary>
            public override void Shoot()
            {
                if (curMagazine > 0)
                {
                    if (curShootS <= 0)
                    {
                        curShootS = shootSpeed;
                        PlayShootAnimation();
                        entity = GetNearest(transform.position + Vector3.up);
                        curMagazine--;
                        if (entity)
                        {
                            if (entity.GetComponent<IDamage>() != null)
                            {
                                entity.GetComponent<IDamage>().TakeDamage(dmg);
                            }

                            joint = PlayerRef.AddComponent<SpringJoint>();
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
        }
    }
}
