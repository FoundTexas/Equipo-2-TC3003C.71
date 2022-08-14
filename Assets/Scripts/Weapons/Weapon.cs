using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("ID")]
    public string ID = "";
    [Header("Speed/Timers Stats")]
    [Tooltip("Speed you reload your magazine")]
    [SerializeField] float reloadSpeed;
    [Tooltip("Speed between shoots")]
    [SerializeField] float shootSpeed;
    float curShootS;

    [Header("Bullet Stats")]
    [Tooltip("Amount of proyectiles the weapon can contain")]
    [SerializeField] int ammo;
    [Tooltip("Amount of proyectiles the weapon holds and can fire before reload")]
    [SerializeField] int magazine;
    int curMagazine, curAmmo;
    [Tooltip("Position where the projectiles and Raycasts are Fired")]
    public Transform firePoint, camara;
    [SerializeField] bool isRaycast;
    [SerializeField] GameObject projectile;
    [SerializeField] LayerMask rayMasks;
    [SerializeField] float distance, dmg;

    bool canReload = true;
    Animator anim;

    private void FixedUpdate()
    {
        if (curShootS > 0) { curShootS -= Time.deltaTime; }
    }

    public void Reolad()
    {
        if (canReload) { ReoladEvent(); }
    }
    IEnumerator ReoladEvent()
    {
        if (curAmmo > 0)
        {
            canReload = false;
            yield return new WaitForSeconds(reloadSpeed);
            curMagazine += curAmmo;
            if (curMagazine > magazine)
            {
                curAmmo = curMagazine - magazine;
                curMagazine = magazine;
            }
            else if (curMagazine <= magazine)
            {
                curAmmo = 0;
            }
            canReload = true;
        }
    }

    public void AddAmmo()
    {
        curAmmo = ammo;
    }

    public void PlayShootAnimation()
    {
        anim.SetTrigger("Shoot");
    }

    public virtual RaycastHit GetRay()
    {
        RaycastHit tmp = new RaycastHit();
        if (Physics.Raycast(camara.position, camara.forward,
            out RaycastHit hitinfo, distance, rayMasks))
        {
            tmp = hitinfo;
        }
        return tmp;
    }

    public virtual void SpawnProjectile()
    {
        Instantiate(projectile, firePoint.position, Quaternion.identity, null);
    }

    public virtual void Shoot() 
    {
        if (isRaycast) { 
            GetRay().transform.GetComponent<IDamage>().TakeDamage(dmg); 
        }
        else if (!isRaycast) { SpawnProjectile(); }
    }
}
