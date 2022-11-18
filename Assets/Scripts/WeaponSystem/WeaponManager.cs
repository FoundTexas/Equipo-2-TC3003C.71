using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Player;
using Interfaces;
using Photon.Pun;

namespace WeaponSystem
{
    [Serializable]
    public class WeaponsUnlocked
    {
        public List<string> unlock = new List<string>(new string[2] { "Grappling", "SoundShoot" });
    }
    /// <summary>
    /// Class that manages the weapons available to the player.
    /// </summary>
    public class WeaponManager : MonoBehaviour, ISave
    {

        public static bool hasWeapon = false;

        //ref photon
        public PhotonView view;
        public PhotonView fatherview;


        PlayerInputs PlayerInput;
        InputAction SwapInput, ToggleInput;

        [Tooltip("List of all weapons on that the player can access and are childs of the WeaponManager Object on scene")]
        public List<Weapon> weapons = new List<Weapon>();
        [Tooltip("Transform reference of the torso object in the armature skeleton")]
        public Transform torso;
        [Tooltip("Transform reference of the hand object in the armature skeleton")]
        public Transform hand;
        [Tooltip("String List the represent the current unlocked weapons of the player")]
        [SerializeField] WeaponsUnlocked unlocked;
        [Tooltip("Reference to the animation and audio manager")]
        [NonSerialized] public AudioAndVideoManager audios;
        [Tooltip("The current selected Weapon script")]
        public Weapon selected;

        Vector3 pos;
        Dictionary<string, int> weaponDictionary = new Dictionary<string, int>();

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void Awake()
        {
            PlayerInput = new PlayerInputs();
        }
        private void OnEnable()
        {
            if (!GameManager.isOnline || GameManager.isOnline && fatherview.IsMine)
            {
                SwapInput = PlayerInput.Game.ChangeArm;
                SwapInput.Enable();
                ToggleInput = PlayerInput.Game.ToggleArm;
                ToggleInput.Enable();

                ToggleInput.performed += ToggleWeaponInput;
                SwapInput.performed += Scroll;
            }
        }
        private void OnDisable()
        {
            if (!GameManager.isOnline || GameManager.isOnline && fatherview.IsMine)
            {
                SwapInput.Disable();
                ToggleInput.Disable();
            }
        }
        void Start()
        {

            FromJson();
            audios = GetComponentInParent<AudioAndVideoManager>();
            //Cursor.lockState = CursorLockMode.Confined;
            for (int i = 0; i < weapons.Count; i++)
            {
                weaponDictionary.Add(weapons[i].GetID(), i);
                weapons[i].gameObject.SetActive(false);
            }
            if (!GameManager.isOnline || GameManager.isOnline && fatherview.IsMine)
            {
                hasWeapon = false;
            }
            else
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
            if (!GameManager.isOnline || GameManager.isOnline && fatherview.IsMine)
            {
                if (unlocked.unlock.Count > 0)
                {

                    int selectedIndex = GetSelectedIndex();
                    selectedIndex++;

                    if (selectedIndex >= unlocked.unlock.Count)
                    {
                        selectedIndex = 0;
                    }

                    if (!GameManager.isOnline)
                    {
                        PunRPCChangeWeapon(selectedIndex);
                    }
                    else if (GameManager.isOnline)
                    {
                        view.RPC("PunRPCChangeWeapon", RpcTarget.All, selectedIndex);
                    }
                }
            }
        }
        /// <summary>
        /// This Void gets the selected index using a comparison by the selected ID.
        /// </summary>
        /// <returns>Returns the selected index relative to the unlocked index. </returns>
        int GetSelectedIndex()
        {
            return unlocked.unlock.IndexOf(selected.GetID());
        }
        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// This function is in charge of changing between an ON and OFF state 
        /// toggleing and diableing the Weapon on the Player.
        /// </summary>
        public void ToggleWeaponInput(InputAction.CallbackContext context)
        {
            if (!GameManager.isOnline || GameManager.isOnline && fatherview.IsMine)
            {
                if (unlocked.unlock.Count != 0)
                {
                    hasWeapon = !hasWeapon;

                    if (!GameManager.isOnline)
                    {
                        PunRPCToggleWeapon(hasWeapon);
                    }
                    else if (GameManager.isOnline)
                    {
                        view.RPC("PunRPCToggleWeapon", RpcTarget.All, hasWeapon);
                    }
                }
            }
        }

        [PunRPC]
        public void PunRPCToggleWeapon(bool vaL)
        {
            if (vaL && unlocked.unlock.Contains(selected.GetID()))
            {
                this.transform.parent = hand;
                pos = Vector3.zero;
                transform.localRotation = Quaternion.Euler(-65, 48, 54);
            }
            else if (!vaL)
            {
                this.transform.parent = torso;
                pos = Vector3.zero;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (selected != null)
            {
                if (!selected.gameObject.activeInHierarchy)
                {
                    selected.gameObject.SetActive(vaL && unlocked.unlock.Contains(selected.GetID()));

                    if (audios)
                    {
                        if (unlocked.unlock.Contains(selected.GetID()))
                        { audios.GunValue(vaL, selected.select); }
                    }
                }
                int selectedIndex = GetSelectedIndex();
            }
        }
        /// <summary>
        /// This Function is in charge og changing between the unlocked weapons
        /// </summary>
        /// <param name="i"> Weapon index relative to the unlocked weapons. </param>
        [PunRPC]
        public void PunRPCChangeWeapon(int i)
        {
            if (unlocked.unlock.Count >= i)
            {
                selected.gameObject.SetActive(false);
                selected = weapons[weaponDictionary[unlocked.unlock[i]]];
                selected.gameObject.SetActive(true);
            }
        }
        /// <summary>
        /// This Method given a string tries to unlock a weapon adding it to thour unlocked options.
        /// </summary>
        /// <param name="weapon"> Weapon Id given to the function. </param>
        public void UnlockWeapon(string weapon)
        {
            if (!GameManager.isOnline || GameManager.isOnline && fatherview.IsMine)
            {
                if (unlocked.unlock.Contains(weapon) == false)
                {
                    unlocked.unlock.Add(weapon);
                }
                else if (unlocked.unlock.Contains(weapon))
                {
                    weapons[weaponDictionary[weapon]].AddAmmo();
                }

                if (!hasWeapon)
                {
                    selected = weapons[weaponDictionary[weapon]];

                    if (!GameManager.isOnline)
                    {
                        PunRPCChangeWeapon(GetSelectedIndex());
                    }
                    else if (GameManager.isOnline)
                    {
                        view.RPC("PunRPCChangeWeapon", RpcTarget.All, GetSelectedIndex());
                    }

                    if (!GameManager.isOnline)
                    {
                        PunRPCToggleWeapon(false);
                    }
                    else if (GameManager.isOnline)
                    {
                        view.RPC("PunRPCToggleWeapon", RpcTarget.All, false);
                    }
                }

                Save();
            }
        }
        /// <summary>
        /// This method gets the current selected weapon (can be Null) and returns the reference.
        /// </summary>
        /// <returns> The current weapon on the selected slot. </returns>
        public Weapon CurrentSelect()
        {
            return unlocked.unlock.Count != 0 ? selected : null;
        }

        public void FromJson()
        {
            string s = JsonUtility.ToJson(unlocked);
            //Debug.Log(s);

            if (PlayerPrefs.HasKey("WeaponManager.1"))
            {
                s = PlayerPrefs.GetString("WeaponManager.1");
                //PlayerPrefs.SetString("WeaponManager.1", s);
            }
            else
            {
                PlayerPrefs.SetString("WeaponManager.1", s);
            }

            JsonUtility.FromJsonOverwrite(s, unlocked);

            if (unlocked.unlock.Contains(selected.GetID()))
                selected.gameObject.SetActive(true);
            //Debug.Log(JsonUtility.ToJson(this));
        }

        public bool Save()
        {
            string s = JsonUtility.ToJson(unlocked);
            PlayerPrefs.SetString("WeaponManager.1", s);

            //Debug.Log("Saving: " + this.name);

            return true;
        }
    }
}
