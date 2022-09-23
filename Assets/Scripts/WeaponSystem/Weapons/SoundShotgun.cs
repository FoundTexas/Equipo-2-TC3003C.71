using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    namespace Weapons
    {
        public class SoundShotgun : Weapon
        {
            public float explosionRadius = 10;
            public float explosionForce = 100;
            public override void Shoot()
            {
                if (curMagazine > 0)
                {
                    if (curShootS <= 0)
                    {
                        curShootS = shootSpeed;
                        PlayShootAnimation();
                        Vector3 pos = firePoint.position + firePoint.forward * distance;
                        Vector3 dir = Direction();
                        if (GetRay(dir).transform)
                        {
                            pos = GetRay(dir).point;
                        }
                        Collider[] objs = Physics.OverlapSphere(pos, explosionRadius, rayMasks);
                        foreach (var obj in objs)
                        {
                            Rigidbody tmprb = obj.gameObject.GetComponent<Rigidbody>();
                            if (tmprb != null)
                            {
                                tmprb.AddExplosionForce(explosionForce, pos, explosionRadius, 3.0F);
                            }
                            if (obj.gameObject != PlayerRef)
                            {
                                IDamage Dmginterface = null;
                                if (obj.gameObject.TryGetComponent<IDamage>(out Dmginterface))
                                {
                                    Dmginterface.TakeDamage(dmg);
                                }
                            }
                        }
                        curMagazine--;
                    }
                }
            }
        }
    }
}
