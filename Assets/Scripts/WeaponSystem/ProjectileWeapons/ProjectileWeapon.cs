using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    namespace ProjectileWeapon
    {
        public class ProjectileWeapon : Weapon
        {
            [Header("Projectile Weapon Look & Feel")]

            [Tooltip("Projectile Prefab instantiated if weapon is pojectile base")]
            public GameObject projectile;

            // ----------------------------------------------------------------------------------------------- Public Methods

            /// <summary>
            /// Function that instantiate the Weapon Projectile.
            /// </summary>
            public void SpawnProjectile()
            {
                Instantiate(projectile, firePoint.position, firePoint.transform.rotation, null);
            }

            // ----------------------------------------------------------------------------------------------- Virtual Methods

            /// <summary>
            /// This Virtual Method handels the shooting for all weapons and can be overrided.
            /// </summary>
            public override void Shoot()
            {
                if (curMagazine > 0)
                {
                    if (curShootS <= 0)
                    {
                        curShootS = shootSpeed;
                        PlayShootAnimation();

                        if (projectile)
                        {
                            SpawnProjectile();
                        }

                        curMagazine--;


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
