
using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WeaponSystem
{
    namespace AreaWeapons
    {
        /// <summary>
        /// Class that has the basic methods for an area and or spray damage based Weapon.
        /// </summary>
        public class AreaWeapon : Weapon
        {
            [Header("Area Bullet Stats")]

            [Tooltip("Layer of what can be hitted by the Weapon if Raycast is used")]
            public LayerMask areaMasks;
            [Tooltip("Force aplied to the objects near the explosion range")]
            public float explosionForce;
            [Tooltip("Radius of the OverlapShepre that checks for area Objects")]
            public float radius;


            // ----------------------------------------------------------------------------------------------- Public Methods

            /// <summary>
            /// Method that generates an explosion in a given position.
            /// </summary>
            /// <param name="pos"> Explosion Vector3 position. </param>
            public void Explode(Vector3 pos)
            {
                Collider[] objs = Physics.OverlapSphere(pos, radius, areaMasks);
                foreach (var obj in objs)
                {
                    Rigidbody tmprb = obj.gameObject.GetComponent<Rigidbody>();
                    if (tmprb != null)
                    {
                        tmprb.AddExplosionForce(explosionForce, pos, distance, 3.0F);
                    }
                    if (obj.gameObject != PlayerRef)
                    {
                        IDamage Dmginterface;
                        if (obj.gameObject.TryGetComponent<IDamage>(out Dmginterface))
                        {
                            Dmginterface.TakeDamage(dmg);
                        }
                    }
                }
            }
            /// <summary>
            /// Function that gets the nearest Transform in a shpere area around the top of the Player.
            /// </summary>
            /// <param name="pos"> Vector 3 of the check shere position. </param>
            /// <returns> Nearest Transform or null. </returns>
            public Transform GetNearest(Vector3 pos)
            {
                Transform tmp = null;
                Collider[] objs = Physics.OverlapSphere(pos, radius, areaMasks);
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

            // ----------------------------------------------------------------------------------------------- Overrided Methods

            /// <summary>
            /// Overrided Shoot Method for GrapplingHook.
            /// </summary>
            [PunRPC]
            public override void PunRPCShoot()
            {
                if (curMagazine > 0)
                {
                    if (curShootS <= 0)
                    {
                        PlayShootAnimation();
                        Explode(PlayerRef.transform.forward * distance);
                        curMagazine--;
                    }
                }
            }
        }
    }
}
