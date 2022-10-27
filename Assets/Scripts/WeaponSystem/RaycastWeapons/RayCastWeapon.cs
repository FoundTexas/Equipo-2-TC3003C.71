using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using Photon.Pun;

namespace WeaponSystem
{
    namespace RayCastWeapons
    {
        /// <summary>
        /// Class that has the basic methods for a RayCast hit scan based Weapon.
        /// </summary>
        public class RayCastWeapon : Weapon
        {
            [Header("RayCaast Weapon Stats")]

            [Tooltip("Spread that has the shoot relative to a Vector3")]
            [SerializeField]
            Vector3 BulletSpreadVariance =
                new Vector3(0.1f, 0.1f, 0.1f);


            [Header("RayCaast Weapon Look & Feel")]

            [Tooltip("Particle system instantiated on when hitting a wall")]
            [SerializeField] ParticleSystem ImpactParticleSystem;
            [Tooltip("Trail left by the the bullet while it moves in the shooted direction")]
            [SerializeField] GameObject BulletTrail;


            [Header("RayCaast Bullet Stats")]

            [Tooltip("Layer of wht can be hitted by the Weapon if Raycast is used")]
            public LayerMask rayMasks;

            // ----------------------------------------------------------------------------------------------- Private Methods

            // Coroutine that handels the movement of the fired trail over time and distance.
            IEnumerator SpawnTrail(TrailRenderer trail, Vector3 vec, bool obama)
            {
                float time = 0;
                Vector3 startPosition = trail.transform.position;
                while (time < 1f)
                {
                    trail.transform.position = Vector3.Lerp(startPosition, vec, time);
                    time += Time.deltaTime / trail.time;

                    yield return null;
                }
                trail.transform.position = vec;
                if (obama)
                {
                    Destroy(Instantiate(ImpactParticleSystem, vec, Quaternion.identity).gameObject, 1);

                }
                Destroy(trail.gameObject, 0.5f);
            }

            // ----------------------------------------------------------------------------------------------- Public Methods

            /// <summary>
            /// This Function returns a RaycastHit (can be Null) from the RayOut position given a Vector3 direction.
            /// </summary>
            /// <param name="direction"> Vector3 of the direction the Ray will take</param>
            /// <returns> RaycasyHit of hitted object or null. </returns>
            public RaycastHit GetRay(Vector3 direction)
            {
                RaycastHit tmp = new RaycastHit();
                if (Physics.Raycast(PlayerRef.transform.position, direction,
                    out RaycastHit hitinfo, distance, rayMasks))
                {
                    tmp = hitinfo;
                }
                return tmp;
            }
            /// <summary>
            /// This Function gets a direction relative to the player center and off sets it given a Vector3.
            /// </summary>
            /// <returns> This returns a normilized direction vector in 3D.</returns>
            public Vector3 Direction()
            {
                Vector3 direction = PlayerRef.transform.forward;

                direction += new Vector3(
                    Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                    Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                    Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
                );
                direction.Normalize();
                return direction;
            }

            public void ShootRay()
            {
                if (BulletTrail)
                {
                    Vector3 dir = Direction();
                    RaycastHit HitGun = GetRay(dir);

                    TrailRenderer trail = //GameManager.isOnline ? 
                    //PhotonNetwork.Instantiate(BulletTrail.name, RayOut.position, Quaternion.identity).GetComponent<TrailRenderer>() : 
                    //Instantiate(BulletTrail, RayOut.position, Quaternion.identity).GetComponent<TrailRenderer>();
                    Instantiate(BulletTrail, RayOut.position, Quaternion.identity).GetComponent<TrailRenderer>();


                    if (HitGun.transform)
                    {
                        StartCoroutine(SpawnTrail(trail, HitGun.point, true));
                        IDamage Dmginterface = null;
                        if (HitGun.transform.gameObject.TryGetComponent<IDamage>(out Dmginterface))
                        {
                            Dmginterface.TakeDamage(dmg);
                        }
                    }
                    else
                    {
                        Debug.Log(dir * distance);
                        StartCoroutine(SpawnTrail(trail, firePoint.position + dir * distance, true));
                    }
                }
            }

            // ----------------------------------------------------------------------------------------------- Overrided Methods

            /// <summary>
            /// This Virtual Method handels the shooting for all weapons and can be overrided.
            /// </summary>
            [PunRPC]
            public override void PunRPCShoot()
            {
                if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
                {
                    if (curMagazine > 0 || curMagazine == -100)
                    {
                        if (curShootS <= 0)
                        {
                            curShootS = shootSpeed;
                            PlayShootAnimation();
                            ShootRay();
                        }
                    }
                    else
                    {
                        PunRPCReload();
                    }
                }
            }
        }
    }
}
