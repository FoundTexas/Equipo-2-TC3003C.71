using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Player;

namespace WeaponSystem
{
    /// <summary>
    /// Class that manages the weapons available to the player.
    /// </summary>
    public class WeaponManager : MonoBehaviour
    {
        public static bool hasWeapon = true;

        PlayerInputs PlayerInput;
        InputAction SwapInput, ToggleInput;

        [Tooltip("List of all weapons on that the player can access and are childs of the WeaponManager Object on scene")]
        [SerializeField] List<Weapon> weapons = new List<Weapon>();
        [Tooltip("Transform reference of the torso object in the armature skeleton")]
        [SerializeField] Transform torso;
        [Tooltip("Transform reference of the hand object in the armature skeleton")]
        [SerializeField] Transform hand;
        [Tooltip("String List the represent the current unlocked weapons of the player")]
        [SerializeField] List<string> unlocked = new List<string>(new string[2] { "Grappling", "SoundShoot" });
        [Tooltip("Reference to the animation and audio manager")]
        [SerializeField] AudioAndVideoManager audios;
        [Tooltip("The current selected Weapon script")]
        [SerializeField] Weapon selected;

        Vector3 pos;

        Dictionary<string, int> weaponDictionary = new Dictionary<string, int>();

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void Awake()
        {
            PlayerInput = new PlayerInputs();
        }
        private void OnEnable()
        {
            SwapInput = PlayerInput.Game.ChangeArm;
            SwapInput.Enable();
            ToggleInput = PlayerInput.Game.ToggleArm;
            ToggleInput.Enable();

            ToggleInput.performed += ToggleWeaponInput;
            SwapInput.performed += Scroll;
        }
        private void OnDisable()
        {
            SwapInput.Disable();
            ToggleInput.Disable();
        }
        void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
            for (int i = 0; i < weapons.Count; i++)
            {
                weaponDictionary.Add(weapons[i].GetID(), i);
                weapons[i].gameObject.SetActive(false);
            }

            ToggleWeapon();

            if (unlocked.Count > 0)
            {
                selected.gameObject.SetActive(true);
            }
        }
        private void LateUpdate()
        {
            transform.localPosition = pos;
        }
        // ----------------------------------------------------------------------------------------------- Private Methods
        /// <summary>
        /// This void is in charge of handelling The Weapon change.
        /// </summary>
        void Scroll(InputAction.CallbackContext callbackContext)
        {
            if (unlocked.Count > 0)
            {

                int selectedIndex = GetSelectedIndex();
                selectedIndex++;

                if (selectedIndex >= unlocked.Count)
                {
                    selectedIndex = 0;
                }

                ChangeWeapon(selectedIndex);
            }
        }
        /// <summary>
        /// This Void gets the selected index using a comparison by the selected ID.
        /// </summary>
        /// <returns>Returns the selected index relative to the unlocked index. </returns>
        int GetSelectedIndex()
        {
            return unlocked.IndexOf(selected.GetID());
        }
        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// This function is in charge of changing between an ON and OFF state 
        /// toggleing and diableing the Weapon on the Player.
        /// </summary>
        public void ToggleWeaponInput(InputAction.CallbackContext context)
        {
            if (unlocked.Count != 0)
            {
                ToggleWeapon();
            }
        }
        public void ToggleWeapon()
        {
            hasWeapon = !hasWeapon;
            if (hasWeapon)
            {
                this.transform.parent = hand;
                pos = Vector3.zero;
                transform.localRotation = Quaternion.Euler(-65, 48, 54);
            }
            else if (!hasWeapon)
            {
                this.transform.parent = torso;
                pos = Vector3.zero;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            audios.GunValue(hasWeapon, selected.select);
        }
        /// <summary>
        /// This Function is in charge og changing between the unlocked weapons
        /// </summary>
        /// <param name="i"> Weapon index relative to the unlocked weapons. </param>
        public void ChangeWeapon(int i)
        {
            if (unlocked.Count >= i)
            {
                selected.gameObject.SetActive(false);
                selected = weapons[weaponDictionary[unlocked[i]]];
                selected.gameObject.SetActive(true);
            }
        }
        /// <summary>
        /// This Method given a string tries to unlock a weapon adding it to thour unlocked options.
        /// </summary>
        /// <param name="weapon"> Weapon Id given to the function. </param>
        public void UnlockWeapon(string weapon)
        {
            if (unlocked.Contains(weapon) == false)
            {
                unlocked.Add(weapon);
            }
            else if (unlocked.Contains(weapon))
            {
                weapons[weaponDictionary[weapon]].AddAmmo();
            }

            if (!hasWeapon)
            {
                selected = weapons[weaponDictionary[weapon]];
                ChangeWeapon(GetSelectedIndex());
                ToggleWeapon();
            }
        }
        /// <summary>
        /// This method gets the current selected weapon (can be Null) and returns the reference.
        /// </summary>
        /// <returns> The current weapon on the selected slot. </returns>
        public Weapon CurrentSelect()
        {
            return unlocked.Count != 0 ? selected : null;
        }
    }
}