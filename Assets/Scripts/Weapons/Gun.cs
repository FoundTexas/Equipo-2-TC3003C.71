using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{

    public override void Shoot(){
        if (curMagazine > 0)
        {
            if (curShootS <= 0)
            {
                curShootS = shootSpeed;
                curMagazine--;
                PlayShootAnimation();

                Vector3 dir = Direction();
                RaycastHit HitGun = GetRay(dir);

                Debug.Log(HitGun.transform.name);
                TrailRenderer trail = Instantiate(BulletTrail, firePoint.position,firePoint.rotation);
                if(HitGun.transform){
                    StartCoroutine(SpawnTrail(trail,HitGun.point,true));
                    IDamage Dmginterface = null;
                    if (HitGun.transform.gameObject.TryGetComponent<IDamage>(out Dmginterface))
                    {
                        Dmginterface.TakeDamage(dmg);
                    }
                }
                
                else{
                    StartCoroutine(SpawnTrail(trail, firePoint.position + dir * distance,false));
                }
            }
        }
    }
}

