using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Class that manages the audio and animations involving the player.
    /// </summary>

    [RequireComponent(typeof(AudioSource))]
    public class AudioAndVideoManager : MonoBehaviour
    {
        [Tooltip("Time in idle needed to reproduce a secondary idle animation")]
        [SerializeField] private float IdleTime;
        [SerializeField] private AudioClip land, jump, unlock;
        [SerializeField] private AudioClip[] step, step2;
        [SerializeField] private string[] grounds;
        [SerializeField] private Animator anim;
        private float gunset = -1;
        private float curgunval = 0;
        private Dictionary<string, int> ground = new Dictionary<string, int>();
        private AudioSource audios;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void Start()
        {
            // Initialize private components
            audios = GetComponent<AudioSource>();
            
            // Fills the ground Dictionary with the grounds list
            for(int i = 0; i < grounds.Length; i++)
                ground.Add(grounds[i], i);
        }

        // private void Update()
        // {
        //     ChangeGun();
        // }
        
        // ----------------------------------------------------------------------------------------------- Public Methods
        /// <summary>
        /// Plays sound of a step when called from the player Animator, selects a random sound from 2 options.
        /// </summary>
        /// <param name="floor"> string containing the name of the type of floor (material) </param>
        public void StepSound(string floor)
        {
            int sound_index = ground[floor];
            audios.PlayOneShot(Random.Range(0,2) == 0? step[sound_index] : step2[sound_index]);
        }

        /// <summary>
        /// Plays unlock sound when inputting the secret code when called from the player Animator.
        /// </summary>
        public void UnlockSound()
        {
            audios.PlayOneShot(unlock);
        }

        /// <summary>
        /// Plays Robot's jump sound when called.
        /// </summary>
        public void jumpSound()
        {
            audios.PlayOneShot(jump);
            anim.SetTrigger("Jump");
        }

        /// <summary>
        /// Plays Robot's dive sound when called.
        /// </summary>
        public void DiveSound()
        {
            audios.PlayOneShot(jump);
            anim.SetTrigger("Jump");
            anim.SetTrigger("Jump2");
        }

        /// <summary>
        /// Function that sets the Animator "isGround" boolean and sets landing sound if is the case.
        /// </summary>
        /// <param name="grounded">State sent and compared with Animator state. </param>
        public void IsOnGround(bool grounded)
        {
            // if (!anim.GetBool("isGrounded") && grounded)
            //     audios.PlayOneShot(land);
            anim.SetBool("isGrounded", grounded);
        }

        /// <summary>
        /// Function that sets the Animator "onWall" boolean and sets landing sound if is the case.
        /// </summary>
        /// <param name="wall"> State sent and compared with Animator state. </param>
        public void IsOnWall(bool wall)
        {
            if (!anim.GetBool("onWall") && wall)
                audios.PlayOneShot(land);
            anim.SetBool("onWall", wall);
        }

        /// <summary>
        /// Function that sets the Animator "hasGun" boolean and sets The gun�s sound if is the case.
        /// </summary>
        /// <param name="gun"> State sent and compared with Animator state. </param>
        /// <param name="sound"> AudioClip from the Weapon. </param>
        public void GunValue(bool gun, AudioClip sound)
        {
            if (anim.GetFloat("hasGun") == 0 && gun)
                audios.PlayOneShot(sound);
            anim.SetFloat("hasGun", gun? 1 : 0);
        }

        // void ChangeGun()
        // {
        //     if (gunset > 0)
        //     {
        //         if (curgunval > 0)
        //         {

        //         }
        //     }
        //     else if (gunset < 0)
        //     {
        //         if (curgunval > 0)
        //         {

        //         }
        //     }
        // }

        /// <summary>
        /// Function that sets Animator "speed" Float.
        /// </summary>
        /// <param name="speed"> State sent and compared with Animator state. </param>
        public void SetIfMovement(float speed)
        {
            anim.SetFloat("speed", speed);
            anim.SetFloat("IdleTime", IdleTime);

            if (speed <= 0.3f)
            {
                if (IdleTime > 40)
                    IdleTime = 0;
                else
                    IdleTime += Time.deltaTime;
            }
            else
                IdleTime = 0;
        }
    }
}
