using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour
    {
        [Header("Weapon Info")]

        [Tooltip("Weapon identification name")]
        [SerializeField] string ID = "";
        [Tooltip("Mesh reference to gun's 3D object")]
        public GameObject GunModel;


        [Header("Weapon Stats")]

        [Tooltip("Speed between shoots")]
        public float shootSpeed;
        [Tooltip("Current elapsed time between shoots")]
        public float curShootS;
        [Tooltip("Spread that has the shoot relative to a Vector3")]
        [SerializeField]
        Vector3 BulletSpreadVariance =
            new Vector3(0.1f, 0.1f, 0.1f);
        [Tooltip("Amount of proyectiles the weapon can contain")]
        [SerializeField] int ammo;
        [Tooltip("Amount of proyectiles the weapon holds and can fire before reload")]
        [SerializeField] int magazine;
        public int curAmmo, curMagazine;


        [Header("Weapon Look & Feel")]

        [Tooltip("Particle system instantiated on when hitting a wall")]
        [SerializeField] ParticleSystem ImpactParticleSystem;
        [Tooltip("Trail left by the the bullet while it moves in the shooted direction")]
        public TrailRenderer BulletTrail;
        [Tooltip("Position where the projectiles and Raycasts are Fired")]
        public Transform firePoint;
        [Tooltip("Projectile Prefab instantiated if weapon is pojectile base")]
        public GameObject projectile;
        [Tooltip("Sound played when gun fired")]
        [SerializeField] AudioClip sound;
        [Tooltip("Sound played when gun selected")]
        public AudioClip select;
        [Tooltip("Particles burst played when gun fired")]
        [SerializeField] ParticleSystem particles;
        Animator anim;
        AudioSource source;


        [Header("Bullet Stats")]

        [Tooltip("Layer of wht can be hitted by the Weapon if Raycast is used")]
        public LayerMask rayMasks;
        [Tooltip("Max distance the raycast will reach")]
        public float distance;
        [Tooltip("Damage done by waepon's hit")]
        public float dmg;
        [Tooltip("Reference to this WeaponManager owner Player")]
        public GameObject PlayerRef;
        [Tooltip("centered position from where the ray is cast")]
        public Transform RayOut;
        bool canReload = true;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void Start()
        {
            source = GetComponent<AudioSource>();
            anim = GetComponent<Animator>();
            canReload = true;
            curMagazine = magazine;
            curAmmo = ammo;

            if (PlayerRef)
            {
                RayOut = PlayerRef.transform.GetChild(0);
            }
            if (firePoint.TryGetComponent<ParticleSystem>(out particles) == false)
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

            Debug.DrawRay(RayOut.position, PlayerRef.transform.forward * distance, Color.red);
        }
        private void FixedUpdate()
        {
            if (curShootS > 0) { curShootS -= Time.deltaTime; }
        }
        // ----------------------------------------------------------------------------------------------- Public Methods
        /// <summary>
        /// Ths Function returns an string with the weapon id value.
        /// </summary>
        /// <returns> Weapon ID </returns>
        public string GetID() { return ID; }
        /// <summary>
        /// This void restores ammo to the max amount.
        /// </summary>
        public void AddAmmo() { curAmmo = ammo; }
        public void Reolad()
        {
            if (canReload)
                anim.SetTrigger("reload");
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
        // ----------------------------------------------------------------------------------------------- Virtual Methods
        public virtual void PlayShootAnimation()
        {
            particles.Play();
            source.PlayOneShot(sound);
        }


        public virtual RaycastHit GetRay(Vector3 direction)
        {
            RaycastHit tmp = new RaycastHit();
            if (Physics.Raycast(RayOut.position, direction,
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

        public IEnumerator SpawnTrail(TrailRenderer trail, Vector3 vec, bool obama)
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

        public virtual void Shoot()
        {
            if (curMagazine > 0)
            {
                if (curShootS <= 0)
                {
                    curShootS = shootSpeed;
                    PlayShootAnimation();

                    GetRay(Direction()).transform.GetComponent<IDamage>().TakeDamage(dmg);
                    curMagazine--;
                }
            }
        }
    }
}
