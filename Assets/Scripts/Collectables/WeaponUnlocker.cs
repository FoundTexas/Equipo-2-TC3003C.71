using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WeaponSystem;
using Player;

namespace Collectables
{
    /// <summary>
    /// This Class unlocks weapons by adding the Weapon ID to the Player's WeaponManager when colllided and adding the max Ammo to that weapon by calling AddAmmo()
    /// </summary>
    public class WeaponUnlocker : MonoBehaviour
    {
        [Tooltip("Reference to the weapon script to be unlocked")]
        [SerializeField] private Weapon weapon;
        [Tooltip("Text Mesh that shows the weapon to be unlocked")]
        [SerializeField] private TextMeshPro gunText;
        [Tooltip("Pick up effect")]
        [SerializeField] private GameObject effect;
        private Transform gun;

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
                if (other.CompareTag("Player"))
                {
                    GameObject g = Instantiate(effect, transform.position, Quaternion.identity);
                    WeaponManager player = other.GetComponent<PlayerAttack>().GetWeaponManager();
                    Destroy(gun.gameObject);
                    Destroy(g, 1f);
                    player.UnlockWeapon(weapon.GetID());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                InstantiateObject();
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
