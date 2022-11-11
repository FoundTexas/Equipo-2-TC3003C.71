using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace Player
{
    /// <summary>
    /// Class that manages the player movement behaviour.
    /// </summary>
    public class PlayerHealth : MonoBehaviourPunCallbacks, IDamage
    {
        public PhotonView view;
        public Gradient color;
        public HealthBar healthBar;
        public GameObject explosionFx;
        public GameObject forceField;
        [Min(0)] public float maxHP = 6;
        private float playerHP;
        private float invFrames = 0f; // Invincivility frames after getting hit
        private HitStop hitStop;
        private Move playerMove;
        private SceneLoader sceneLoader;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        void Start()
        {
            view = GetComponent<PhotonView>();
            if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
            {
                // Initialize private components
                GameObject manager = GameObject.FindWithTag("Manager");
                if (manager != null)
                    hitStop = manager.GetComponent<HitStop>();
                sceneLoader = GameObject.FindWithTag("SceneLoader").GetComponent<SceneLoader>();

                // Establish original values
                playerHP = maxHP;
                if (!healthBar)
                    healthBar = FindObjectOfType<HealthBar>();
                healthBar.SetMaxHealth(maxHP);
                playerMove = GetComponent<Move>();
            }
        }

        void Update()
        {
            if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
            {
                if (!GameManager.isOnline)
                {
                    PunRPCResetShield();
                }
                else if (GameManager.isOnline)
                {

                    view.RPC("PunRPCResetShield", RpcTarget.All);
                }
            }
        }

        [PunRPC]
        void PunRPCResetShield()
        {
            invFrames -= Time.deltaTime;
            if (invFrames <= 0)
                forceField.SetActive(false);
        }

        void OnTriggerEnter(Collider collision)
        {
            if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
            {
                if (collision.gameObject.tag == "Enemy" && invFrames <= 0)
                {
                    Vector3 dir = transform.position - collision.transform.position;
                    dir = dir.normalized + Vector3.up;
                    playerMove.AddForce(15f, dir, 0.5f);
                    TakeDamage(1);
                }
                    
            }
        }

        // ----------------------------------------------------------------------------------------------- Public Methods
        /// <summary>
        /// Method that adds health with a specific value to the player.
        /// </summary>
        /// <param name="amount"> float value to be added to the player's health. </param>(
        public void AddHealth(float amount)
        {
            if (!GameManager.isOnline)
            {
                AddHealthRPC(amount);
            }
            else if (GameManager.isOnline && view.IsMine)
            {
                view.RPC("AddHealthRPC", RpcTarget.All, amount);
            }
        }

        [PunRPC]
        public void AddHealthRPC(float amount)
        {
            playerHP += amount;
            healthBar.SetHealth(playerHP);
        }

        // ----------------------------------------------------------------------------------------------- Interface IDamage
        /// <summary>
        /// Interface Abstract method in charge of the death routine of the assigned Object.
        /// </summary>
        public void PunRPCDie()
        {
            if (!GameManager.isOnline)
            {
                DieRPC();
            }
            else if (GameManager.isOnline && view.IsMine)
            {
                view.RPC("DieRPC", RpcTarget.All);
            }
        }

        [PunRPC]
        public void DieRPC()
        {
            // Create death effects
            Vector3 vfxPos = transform.position;
            vfxPos.y = transform.position.y + 1;
            GameObject deathvfx = Instantiate(explosionFx, vfxPos, Quaternion.identity);

            hitStop.HitStopFreeze(10f, 1f);
            gameObject.SetActive(false);
            sceneLoader.LoadByIndex(GameManager.getSceneIndex(), GameManager.getCheckpoint());
            
            var vfxDuration = 1f;
            Destroy(deathvfx, vfxDuration);

        }

        /// <summary>
        /// Interface Abstract method that handels when an object takes damage.
        /// </summary>
        /// <param name="dmg"> Amount of damage taken. </param>
        public virtual void TakeDamage(float dmg)
        {
            if (!GameManager.isOnline)
            {
                TakeDamageRPC(dmg);
            }
            else if (GameManager.isOnline && view.IsMine)
            {
                view.RPC("TakeDamageRPC", RpcTarget.All, dmg);
            }
        }

        [PunRPC]
        void TakeDamageRPC(float dmg)
        {
            invFrames = 2f;
            playerHP -= dmg;
            healthBar.SetHealth(playerHP);
            if (playerHP <= -1)
            {
                if (GameManager.isOnline)
                {
                    view.RPC("PunRPCDie", RpcTarget.All);
                }
                else if (!GameManager.isOnline)
                {
                    PunRPCDie();
                }
            }
            else
            {
                forceField.SetActive(true);
                hitStop.HitStopFreeze(2f, 0.2f);
            }
        }

        /// <summary>
        /// Interface Abstract method that starts the freezing of an object.
        /// </summary>
        public void Freeze()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Interface Abstract method that starts the burnning of an object.
        /// </summary>
        public void Burn()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Interface Abstract method that handels the burnning of an object.
        /// </summary>
        public IEnumerator Burnning()
        {
            throw new System.NotImplementedException();
        }
    }
}
