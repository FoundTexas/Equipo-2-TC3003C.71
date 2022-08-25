using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundShotgun : Weapon
{
    public float explosionRadius = 10;
    public float explosionForce = 100;
    public GameObject player;
    public override void Shoot()
    {
        if (curMagazine > 0)
        {
            if (curShootS <= 0)
            {
                curShootS = shootSpeed;
                PlayShootAnimation();
                Vector3 pos = firePoint.position + firePoint.forward * distance;
                if (GetRay().transform)
                {
                    pos = GetRay().point;
                }
                Collider[] objs = Physics.OverlapSphere(pos, explosionRadius, rayMasks);
                foreach (var obj in objs)
                {
                    Rigidbody tmprb = obj.gameObject.GetComponent<Rigidbody>();
                    if (tmprb != null)
                    {
                        tmprb.AddExplosionForce(explosionForce, pos, explosionRadius, 3.0F);
                        if (tmprb.gameObject != player)
                        {
                            IDamage Dmginterface = null;
                            if (tmprb.gameObject.TryGetComponent<IDamage>(out Dmginterface))
                            {
                                Dmginterface.TakeDamage(dmg);
                            }
                        }
                    }
                }
                curMagazine--;
            }
        }
    }
}
