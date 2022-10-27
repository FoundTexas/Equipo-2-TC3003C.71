using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Photon.Pun;

namespace WeaponSystem
{
    /// <summary>
    /// Main class for all weapons containning the basic stats and methods that all share.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour
    {
        public PhotonView view;
        public PlayerInputs PlayerInput;
        public InputAction FireInput, ReloadInput;

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

        private void Awake()
        {
            PlayerInput = new PlayerInputs();
        }
        private void OnEnable()
        {
            if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
            {
                FireInput = PlayerInput.Game.Fire;
                FireInput.Enable();
                ReloadInput = PlayerInput.Game.Reload;
                ReloadInput.Enable();
            }
        }
        private void OnDisable()
        {
            if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
            {
                FireInput.Disable();
                ReloadInput.Disable();
            }
        }
        private void Start()
        {
            source = GetComponent<AudioSource>();
            anim = GetComponent<Animator>();
            if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
            {
                Debug.Log("start1");
                
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
        }
        private void Update()
        {
            if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
            {
                if (WeaponManager.hasWeapon)
                {
                    if (FireInput.IsPressed())
                    {

                        PunRPCShoot();

                    }
                    if (ReloadInput.IsPressed())
                    {
                        if (!GameManager.isOnline)
                        {
                            PunRPCReload();
                        }
                        else if (GameManager.isOnline)
                        {
                            view.RPC("PunRPCReload", RpcTarget.All);
                        }
                    }
                }
            }
        }
        private void FixedUpdate()
        {
            if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
            {
                if (curShootS > 0) { curShootS -= Time.deltaTime; }
            }
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
        /// <summary>
        /// Void that triggers the reload animation on the weapon.
        /// </summary>
        [PunRPC]
        public void PunRPCReload()
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
            }
        }

        /// <summary>
        /// Method that visualize the shoot given the present parameters rendering a trail 
        /// and or spawning a projectile if given the prefabs.
        /// </summary>
        [PunRPC]
        public void PunRPCPlayShootAnimation()
        {
            particles.Play();
            source.PlayOneShot(sound);
        }

        // ----------------------------------------------------------------------------------------------- Virtual Methods

        /// <summary>
        /// This Virtual Method handels the shooting for all weapons and can be overrided.
        /// </summary>
        public virtual void PunRPCShoot()
        {
            if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
            {
                if (curMagazine > 0 || curMagazine == -100)
                {
                    if (curShootS <= 0)
                    {
                        curShootS = shootSpeed;
                        PunRPCPlayShootAnimation();

                        if (curMagazine != -100)
                        {
                            curMagazine--;
                        }
                    }
                }
                else
                {
                    PunRPCReload();
                }
            }
        }
    }
}
