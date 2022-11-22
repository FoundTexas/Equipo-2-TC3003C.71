using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WeaponSystem;
using Player;
using Photon.Pun;

namespace Collectables
{
    /// <summary>
    /// This Class unlocks weapons by adding the Weapon ID to the Player's WeaponManager when colllided and adding the max Ammo to that weapon by calling AddAmmo()
    /// </summary>
    public class WeaponUnlocker : MonoBehaviourPunCallbacks
    {
        [Tooltip("Reference to the weapon script to be unlocked")]
        [SerializeField] private Weapon weapon;
        [Tooltip("Text Mesh that shows the weapon to be unlocked")]
        [SerializeField] private TextMeshPro gunText;
        [Tooltip("Pick up effect")]
        [SerializeField] private GameObject effect;
        private Transform gun;
        private GameObject player;
        private WeaponManager weapons;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void Start()
        {
            if(weapon.GetID() == "BasicGun")
                gunText.text = "Laser Gun";
            else if(weapon.GetID() == "Grappling")
                gunText.text = "Grappling Hook";
            InstantiateObject();
                
        }

        private void OnTriggerEnter(Collider other)
        {
            if (gun)
            {
                if(GameManager.isOnline && other.CompareTag("Player"))
                {
                    if(other.GetComponent<PhotonView>().IsMine)
                    {
                        weapons = other.gameObject.GetComponent<PlayerAttack>().GetWeaponManager();
                        GameObject g = Instantiate(effect, transform.position, Quaternion.identity);
                        Destroy(gun.gameObject);
                        Destroy(g, 1f);
                        weapons.UnlockWeapon(weapon.GetID());
                        Destroy(gameObject);
                    }
                }
                else if (other.CompareTag("Player") && !GameManager.isOnline)
                {
                    weapons = other.gameObject.GetComponent<PlayerAttack>().GetWeaponManager();
                    GameObject g = Instantiate(effect, transform.position, Quaternion.identity);
                    Destroy(gun.gameObject);
                    Destroy(g, 1f);
                    weapons.UnlockWeapon(weapon.GetID());
                    Destroy(gameObject);
                }
            }
        }

        // ----------------------------------------------------------------------------------------------- Private Methods

        /// <summary>
        /// This method instantiates the gun unlocked.
        /// </summary>
        private void InstantiateObject()
        {
            Transform parent = transform.GetChild(0).GetChild(0);
            gun = Instantiate(weapon.GunModel, parent.position, Quaternion.identity, parent).transform;
            gun.localPosition = Vector3.up;
        }
    }
}
