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
        [Tooltip("Amount of proyectiles the weapon can contain")]
        [SerializeField] int ammo;
        [Tooltip("Amount of proyectiles the weapon holds and can fire before reload")]
        [SerializeField] int magazine;
        public int curAmmo, curMagazine;


        [Header("Weapon Look & Feel")]

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
            Debug.Log("start1");
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
            //Debug.DrawRay(RayOut.position, PlayerRef.transform.forward * distance, Color.red);
        }
        private void FixedUpdate()
        {
            if (curShootS > 0) { curShootS -= Time.deltaTime; }
        }

        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// Function that instantiate the Weapon Projectile.
        /// </summary>
        public void SpawnProjectile()
        {
            Instantiate(projectile, firePoint.position, firePoint.transform.rotation, null);
        }
        /// <summary>
        /// Ths Function returns an string with the weapon id value.
        /// </summary>
        /// <returns> Weapon ID </returns>
        public string GetID() { return ID; }
        /// <summary>
        /// This void restores ammo to the max amount.
        /// </summary>
        public void AddAmmo() { curAmmo = ammo; }
        /// <summary>
        /// Void that triggers the reload animation on the weapon.
        /// </summary>
        public void Reolad()
        {
            if (curAmmo > 0)
            {
                if (canReload)
                    anim.SetTrigger("reload");
            }
        }
        /// <summary>
        /// Reload event triggered via animation to check the amount of ammo and fills current magazine.
        /// </summary>
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
        
        /// <summary>
        /// Method that visualize the shoot given the present parameters rendering a trail 
        /// and or spawning a projectile if given the prefabs.
        /// </summary>
        public void PlayShootAnimation()
        {
            particles.Play();
            source.PlayOneShot(sound);
        }

        // ----------------------------------------------------------------------------------------------- Virtual Methods

        /// <summary>
        /// This Virtual Method handels the shooting for all weapons and can be overrided.
        /// </summary>
        public virtual void Shoot()
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