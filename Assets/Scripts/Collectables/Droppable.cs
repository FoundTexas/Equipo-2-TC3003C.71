using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using WeaponSystem;

namespace Collectables
{
    /// <summary>
    /// Class of the dropped items by the Dropper
    /// </summary>
    public class Droppable : MonoBehaviour
    {
        [Tooltip("Energy value given by the item")]
        public float EnergyVal;
        [Tooltip("Ammo value given by the item")]
        public float AmmoVal;
        [Tooltip("Pick up effect")]
        [SerializeField] private GameObject effect;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject effectGameObject = Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(effectGameObject, 1.5f);
                this.gameObject.SetActive(false);
                PlayerHealth player = other.GetComponent<PlayerHealth>();
                if (EnergyVal > 0)
                {
                    other.GetComponent<PlayerHealth>().AddHealth(EnergyVal);
                }
                if (AmmoVal > 0)
                {
                    other.GetComponent<PlayerAttack>().GetWeaponManager().CurrentSelect().AddAmmo();
                }

                Destroy(this.gameObject, 2);
            }
        }
    }
}
