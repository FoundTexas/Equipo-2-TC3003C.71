using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class WeaponManager : MonoBehaviour
    {
        public static bool hasWeapon = true;

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
        void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
            for (int i = 0; i < weapons.Count; i++)
            {
                weaponDictionary.Add(weapons[i].GetID(), i);
                weapons[i].gameObject.SetActive(false);
            }
            toggleWeapon();

            if (unlocked.Count > 0)
            {
                selected.gameObject.SetActive(true);
            }
        }
        void Update()
        {
            Inputs();
        }
        private void LateUpdate()
        {
            transform.localPosition = pos;
        }
        // ----------------------------------------------------------------------------------------------- Private Methods
        /// <summary>
        /// This void is in charge of handelling The WeaponManager Inputs.
        /// </summary>
        void Inputs()
        {
            if (unlocked.Count > 0)
            {
                if (Input.mouseScrollDelta.y != 0)
                {
                    int selectedIndex = GetSelectedIndex();
                    selectedIndex += Mathf.RoundToInt(Input.mouseScrollDelta.y);

                    selectedIndex = Mathf.Clamp(selectedIndex, 0, unlocked.Count - 1);
                    ChangeWeapon(selectedIndex);
                }
                if (Input.GetKeyDown("q"))
                {
                    toggleWeapon();
                }
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
        public void toggleWeapon()
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
                toggleWeapon();
            }
        }
        /// <summary>
        /// This method gets the current selected weapon (can be Null) and returns the reference.
        /// </summary>
        /// <returns> The current weapon on the selected slot. </returns>
        public Weapon currentSelect()
        {
            return selected;
        }
    }
}
