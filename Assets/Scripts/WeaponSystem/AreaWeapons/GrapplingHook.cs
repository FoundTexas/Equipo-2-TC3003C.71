using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace WeaponSystem
{
    namespace AreaWeapons
    {
        /// <summary>
        /// Weapon Class that targets the nearest Hookable Object in an area and sets a spring rope between it and the player.
        /// </summary>
        public class GrapplingHook : AreaWeapon
        {
            [Header("Grappling Gun Values")]
            [Tooltip("Force of the spring applied to the rope makes it more boucy")]
            [SerializeField] float spring = 100;
            [Tooltip("Damper value on the joint conforming the spring")]
            [SerializeField] float damper = 7;
            [Tooltip("mass scale applied to the rope")]
            [SerializeField] float massScale = 4.5f;
            LineRenderer lr;
            Transform entity;
            SpringJoint joint;

            // ----------------------------------------------------------------------------------------------- Unity Methods

            private void Awake()
            {
                lr = GetComponent<LineRenderer>();
                PlayerInput = new PlayerInputs();
            }
            private void OnEnable()
            {
                ReloadInput = PlayerInput.Game.Reload;
                ReloadInput.Enable();
                FireInput = PlayerInput.Game.Fire;
                FireInput.Enable();
                FireInput.canceled += StopGrapple;
                FireInput.performed += Inputshoot;
            }
            private void OnDisable()
            {
                FireInput.Disable();
            }
            void Update() //Overrided
            {
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up * radius, Color.magenta);
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.left * radius, Color.magenta);
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.right * radius, Color.magenta);
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.forward * radius, Color.magenta);

                if (WeaponManager.hasWeapon)
                {
                    if (ReloadInput.IsPressed()) { Reolad(); }
                }
                else
                {
                    StopGrappleAuto();
                }
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
                if (!entity) return;

                if(lr.positionCount == 0) return;

                float dist = Vector3.Distance(PlayerRef.transform.position, entity.position);

                if (dist > radius*1.3f || dist <= 1){
                    StopGrappleAuto();
                    return;
                }

                lr.SetPosition(0, firePoint.position);
                lr.SetPosition(1, entity.position);
            }
            // Void that handels when the hook is stopped.
            void StopGrapple(InputAction.CallbackContext context)
            {
                StartCoroutine(StopgrappleRoutine());
            }
            IEnumerator StopgrappleRoutine()
            {
                yield return new WaitForSeconds(1.5f);
                entity = null;
                lr.positionCount = 0;
            }
            void Inputshoot(InputAction.CallbackContext context)
            {
                Shoot();
            }

            // Void that handels when the hook is stopped.
            void StopGrappleAuto()
            {
                entity = null;
                lr.positionCount = 0;
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
                            Vector3 dir = entity.position - PlayerRef.transform.position;
                            Debug.Log(dir);
                            PlayerRef.GetComponent<Move>().AddForce(explosionForce,dir.normalized*5 + PlayerRef.transform.forward +Vector3.up,1);
                            if (entity.GetComponent<IDamage>() != null)
                            {
                                entity.GetComponent<IDamage>().TakeDamage(dmg);
                            }
                            //joint = PlayerRef.AddComponent<SpringJoint>();
                            lr.positionCount = 2;
                            Debug.Log(entity.name);
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
