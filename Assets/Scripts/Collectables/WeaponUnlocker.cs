using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WeaponSystem;

namespace Collectables
{
    /// <summary>
    /// This Class unlocks weapons by adding the Weapon ID to the Player's WeaponManager when colllided and adding the max Ammo to that weapon by calling AddAmmo()
    /// </summary>
    public class WeaponUnlocker : MonoBehaviour
    {
        [Tooltip("Referance to the weapon script ti be unlocked")]
        [SerializeField] Weapon weapon;
        [Tooltip("Text Mesh que muestra el nombre del arma a desbloquear")]
        [SerializeField] TextMeshPro gunText;
        [Tooltip("Effecto de recoger el arma")]
        [SerializeField] GameObject effect;
        Transform gun;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        private void Start()
        {
            gunText.text = "<<" + weapon.GetID() + ">>";
            InstantiateObject();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (gun)
            {
                if (other.CompareTag("Player"))
                {
                    GameObject g = Instantiate(effect, transform.position, Quaternion.identity);
                    WeaponManager player = other.GetComponent<PlayerHealth>().GetWeaponManager();
                    Destroy(gun.gameObject);
                    Destroy(g, 1f);
                    player.UnlockWeapon(weapon.GetID());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                InstantiateObject();
            }
        }

        // ----------------------------------------------------------------------------------------------- Private Methods

        void InstantiateObject()
        {
            Transform parent = transform.GetChild(0).GetChild(0);
            gun = Instantiate(weapon.GunModel, parent.position, Quaternion.identity, parent).transform;
            gun.localPosition = Vector3.up;
        }
    }
}
