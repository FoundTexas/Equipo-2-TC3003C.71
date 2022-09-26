using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectables
{
    /// <summary>
    /// Class responable of dropping the collectables using random probability using max values.
    /// </summary>
    public class Dropper : MonoBehaviour
    {
        [Header("Stats")]

        [Tooltip("Chance off dropping Energy collectable")]
        [Range(0, 1)] public float dropEnergy = 0.4f;
        [Tooltip("Chance off dropping Ammo collectable")]
        [Range(0, 1)] public float dropCurrency = 0.6f;

        [Header("Prefabs")]

        [Tooltip("Prefab of droppable Energy collectable")]
        public GameObject EnergyPrefab;
        [Tooltip("Prefab of droppable Ammo collectable")]
        public GameObject CurrencyPrefab;
        bool isApplicationQuitting = false;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        void OnApplicationQuit()
        {
            isApplicationQuitting = true;
        }

        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// Method responsable of spawnning an Energy o Ammo collectable base on probability.
        /// </summary>
        public void Spawn()
        {
            if (isApplicationQuitting)
            {
                return;
            }
            else if (!isApplicationQuitting)
            {
                if (Random.Range(0f, 1f) >= dropCurrency)
                {
                    //Player obtained currency
                    Vector3 randomPosition = transform.position + Random.insideUnitSphere;
                    Instantiate(CurrencyPrefab, randomPosition, Quaternion.identity);
                    return;
                }
                if (Random.Range(0f, 1f) >= dropEnergy)
                {
                    Vector3 randomPosition = transform.position + Random.insideUnitSphere;
                    Instantiate(EnergyPrefab, randomPosition, Quaternion.identity);
                    return;
                }
            }
        }
    }
}
