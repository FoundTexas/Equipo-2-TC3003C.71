using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

namespace Player
{
    /// <summary>
    /// Class that manages the player movement behaviour.
    /// </summary>
    public class PlayerHealth : MonoBehaviour, IDamage
    {
        public HealthBar healthBar;
        public GameObject explosionFx;
        public GameObject forceField;
        [Min(0)] public float maxHP = 6;
        private float playerHP;
        private float invFrames = 0f; // Invincivility frames after getting hit
        private HitStop hitStop;
        private SceneLoader sceneLoader;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        void Start()
        {
            // Initialize private components
            GameObject manager = GameObject.FindWithTag("Manager");
            if(manager != null)
                hitStop = manager.GetComponent<HitStop>();
            sceneLoader = GameObject.FindWithTag("SceneLoader").GetComponent<SceneLoader>();
            
            // Establish original values
            playerHP = maxHP;
            healthBar.SetMaxHealth(maxHP);
        }

        void Update()
        {
            invFrames -= Time.deltaTime;
            if(invFrames <= 0)
                forceField.SetActive(false);
        }

        void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Enemy" && invFrames <= 0)
                TakeDamage(1);
        }   

        void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.tag == "Enemy" && invFrames <= 0)
                TakeDamage(1);
        } 

        // ----------------------------------------------------------------------------------------------- Public Methods
        /// <summary>
        /// Method that adds health with a specific value to the player.
        /// </summary>
        /// <param name="amount"> float value to be added to the player's health. </param>
        public void AddHealth(float amount)
        {
            playerHP += amount;
            healthBar.SetHealth(playerHP);
        }

        // ----------------------------------------------------------------------------------------------- Interface IDamage
        /// <summary>
        /// Interface Abstract method in charge of the death routine of the assigned Object.
        /// </summary>
        public void Die()
        {
            // Create death effects
            Vector3 vfxPos = transform.position;
            vfxPos.y = transform.position.y + 1;
            GameObject deathvfx = Instantiate(explosionFx, vfxPos, Quaternion.identity);
            
            hitStop.HitStopFreeze(0.2f, 0.1f);
            gameObject.SetActive(false);
            sceneLoader.LoadByIndex(1);
            
            var vfxDuration = 1f;
            Destroy(deathvfx, vfxDuration);
        }

        /// <summary>
        /// Interface Abstract method that handels when an object takes damage.
        /// </summary>
        /// <param name="dmg"> Amount of damage taken. </param>
        public virtual void TakeDamage(float dmg)
        {
            invFrames = 2f;
            playerHP -= dmg;
            healthBar.SetHealth(playerHP);
            if (playerHP <= -1)
                Die();
            else
            {
                forceField.SetActive(true);
                hitStop.HitStopFreeze(0.02f, 0.2f);
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
