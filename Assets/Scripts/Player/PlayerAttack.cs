using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;

namespace Player
{
    /// <summary>
    /// Class that contains the player attack behaviour, including weapons and melee attack info.
    /// </summary>
    public class PlayerAttack : MonoBehaviour
    {
        [Tooltip("Minimum time to use melee attack again")]
        [Min(0)] public float meleeCoolDownTime = 0.8f; 
        public WeaponManager playerWeapons;
        public AmmoDisplay ammoDisplay;
        private int comboClicks = 0;
        private Weapon currentWeapon;
        private Animator animator;
        
        // ----------------------------------------------------------------------------------------------- Unity Methods
        void Start()
        {
            // Initialize private components
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            CheckCurrentWeapon();
            if (Input.GetMouseButtonDown(1)) 
                MeleeAttack();
        }

        // ----------------------------------------------------------------------------------------------- Private Methods
        /// <summary>
        /// Method that manages the player's current weapon.
        /// </summary>
        private void CheckCurrentWeapon()
        {
            currentWeapon = playerWeapons.CurrentSelect();
            if (currentWeapon != null)
            {
                if (ammoDisplay)
                {
                    ammoDisplay.SetGunName(currentWeapon.GetID());

                    if (currentWeapon.curMagazine == -100|| currentWeapon.curAmmo == -100)
                    {
                        ammoDisplay.SetCurrentAmmo("∞");
                        ammoDisplay.SetRemainingAmmo("∞");
                    }
                    else
                    {
                        ammoDisplay.SetCurrentAmmo(currentWeapon.curMagazine.ToString());
                        ammoDisplay.SetRemainingAmmo(currentWeapon.curAmmo.ToString());
                    }
                }

            }
        }

        /// <summary>
        /// Method that controls melee attack routine.
        /// </summary>
        private void MeleeAttack()
        {
            StopAllCoroutines();
            StartCoroutine(ResetCombo());
            comboClicks++;
            comboClicks = Mathf.Clamp(comboClicks, 0, 3);

            if(comboClicks == 1)
                animator.SetTrigger("Attack1");
            if(comboClicks == 2)
                animator.SetTrigger("Attack2");
            if(comboClicks == 3)
                animator.SetTrigger("Attack3");
        }

        // ----------------------------------------------------------------------------------------------- Public Methods
        /// <summary>
        /// Method that returns the PlayerAttack reference of its WeaponManager component.
        /// </summary>
        /// <returns>Returns the WeaponManager component reference. </returns>
        public WeaponManager GetWeaponManager()
        {
            return playerWeapons;
        }

        // ----------------------------------------------------------------------------------------------- Private Coroutines
        /// <summary>
        /// Method that resets the player's combo after a cool down time.
        /// </summary>
        private IEnumerator ResetCombo()
        {
            yield return new WaitForSeconds(meleeCoolDownTime);
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");
            comboClicks = 0;
        }
    }
}