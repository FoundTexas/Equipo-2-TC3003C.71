using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    namespace Weapons
    {
        public class Gun : Weapon
        {
            public override void Shoot()
            {
                if (curMagazine > 0)
                {
                    if (curShootS <= 0)
                    {
                        curShootS = shootSpeed;
                        PlayShootAnimation();

                        Vector3 dir = Direction();
                        RaycastHit HitGun = GetRay(dir);

                        TrailRenderer trail = Instantiate(BulletTrail, RayOut.position, Quaternion.identity);

                        curMagazine--;
                        if (HitGun.transform)
                        {
                            Debug.Log(HitGun.transform.name);
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
                else
                {
                    Reolad();
                }
            }
        }
    }
}

