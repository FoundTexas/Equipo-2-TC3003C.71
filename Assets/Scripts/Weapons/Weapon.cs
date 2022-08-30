using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    [Header("ID")]
    public string ID = "";
    [Header("Speed/Timers Stats")]
    [Tooltip("Speed you reload your magazine")]
    [SerializeField] float reloadSpeed;
    [Tooltip("Speed between shoots")]
    public float shootSpeed;
    public float curShootS;

    [Header("Bullet Stats")]
    [SerializeField] Vector3 BulletSpreadVariance = new Vector3(0.1f,0.1f,0.1f);
    [SerializeField] ParticleSystem ImpactParticleSystem;
    public TrailRenderer BulletTrail;
    [Tooltip("Amount of proyectiles the weapon can contain")]
    [SerializeField] int ammo;
    [Tooltip("Amount of proyectiles the weapon holds and can fire before reload")]
    [SerializeField] int magazine;
    public int curMagazine, curAmmo;
    [Tooltip("Position where the projectiles and Raycasts are Fired")]
    public Transform firePoint;
    Transform camara;
    [SerializeField] bool isRaycast;
    public GameObject projectile;
    public LayerMask rayMasks;
    public float distance, dmg;
    public AudioClip sound;

    public ParticleSystem particles;
    Animator anim;
    public bool canReload = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        camara = Camera.main.transform;
        canReload = true;
        curMagazine = magazine;
        curAmmo = ammo;
        if(firePoint.TryGetComponent<ParticleSystem>(out particles) == false)
        {
            particles = firePoint.gameObject.AddComponent<ParticleSystem>();
            particles.Stop();
        }
    }
    private void Update()
    {
        if (WeaponManager.hasWeapon)
        {
            if (Input.GetKeyDown("r")) { Reolad(); }
            if (Input.GetMouseButtonDown(0)) { Shoot(); }
        }
    }
    private void FixedUpdate()
    {
        if (curShootS > 0) { curShootS -= Time.deltaTime; }
    }


    public void Reolad()
    {
        if (canReload)
        {
            Debug.Log("reload");
            anim.SetTrigger("reload");
        }
    }
    public void ReoladEvent()
    {
        if (curAmmo > 0)
        {
            Debug.Log("reloading");
            canReload = false;
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
            Debug.Log("reloaded");
        }
    }

    public void AddAmmo()
    {
        curAmmo = ammo;
    }

    public virtual void PlayShootAnimation()
    {
        particles.Play();
    }

    public virtual RaycastHit GetRay(Vector3 direction)
    {

        RaycastHit tmp = new RaycastHit();
        if (Physics.Raycast(firePoint.position, direction,
            out RaycastHit hitinfo, distance, rayMasks))
        {
            tmp = hitinfo;
        }
        return tmp;
    }

    public virtual void SpawnProjectile()
    {
        Instantiate(projectile, firePoint.position, firePoint.transform.rotation, null);
    }

    public Vector3 Direction(){
        Vector3 direction = firePoint.forward;

        direction += new Vector3(
            Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
            Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
            Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
        );
        direction.Normalize();
        return direction;
    }

    public IEnumerator SpawnTrail(TrailRenderer trail, Vector3 vec, bool obama){
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while(time<0.5f){
            trail.transform.position = Vector3.Lerp(startPosition,vec,time);
            time += Time.deltaTime/trail.time;

            yield return null;
        }
        trail.transform.position = vec;
        if(obama){
        Destroy(Instantiate(ImpactParticleSystem, vec, Quaternion.identity),2);

        }
        Destroy(trail.gameObject,4);
    }

    public virtual void Shoot() 
    {
        if (curMagazine > 0)
        {
            if (curShootS <= 0)
            {
                curShootS = shootSpeed;
                PlayShootAnimation();
                if (isRaycast)
                {
                    GetRay(Direction()).transform.GetComponent<IDamage>().TakeDamage(dmg);
                    curMagazine--;
                }
                else if (!isRaycast) 
                { 
                    SpawnProjectile();
                    curMagazine--;
                }
            }
        }
    }
}
