using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

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
        [Tooltip("Effecto de recoger")]
        [SerializeField] GameObject effect;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject g = Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(g, 1.5f);
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
