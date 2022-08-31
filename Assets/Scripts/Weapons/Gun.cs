using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
                TrailRenderer trail = Instantiate(BulletTrail, firePoint.position, Quaternion.identity);
                if(HitGun.transform){
                    StartCoroutine(SpawnTrail(trail,HitGun.point,true));
                    IDamage Dmginterface = null;
                    if (HitGun.transform.gameObject.TryGetComponent<IDamage>(out Dmginterface))
                    {
                        Dmginterface.TakeDamage(dmg);
                    }
                }
                
                else{
                    StartCoroutine(SpawnTrail(trail,dir,false));
                }
            }
        }
    }
}

