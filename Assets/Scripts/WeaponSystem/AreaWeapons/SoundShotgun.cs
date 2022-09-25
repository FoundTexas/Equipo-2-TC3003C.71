using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    namespace Weapons
    {
        public class SoundShotgun : AreaWeapon
        {
            public override void Shoot()
            {
                if (curMagazine > 0)
                {
                    if (curShootS <= 0)
                    {
                        curShootS = shootSpeed;
                        PlayShootAnimation();
                        Vector3 pos = firePoint.position + firePoint.forward * distance;
                        curMagazine--;
                    }
                }
            }
        }
    }
}
