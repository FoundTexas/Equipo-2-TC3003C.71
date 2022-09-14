using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    
    [Header("Stats")]
    [Range(0, 1)] public float dropEnergy = 0.4f; 
    [Range(0, 1)] public float dropCurrency = 0.6f; 
    
    [Header("Prefabs")]
    public GameObject EnergyPrefab;
    public GameObject CurrencyPrefab;
    bool isApplicationQuitting = false;

    public void Spawn()
    {
        if (isApplicationQuitting)
        {
            return;
        }
        else if (!isApplicationQuitting)
        {
            if (Random.Range(0f, 1f) >= dropEnergy)
            {
                Vector3 randomPosition = transform.position + Random.insideUnitSphere;
                Instantiate(EnergyPrefab, randomPosition, Quaternion.identity);
            }
            if (Random.Range(0f, 1f) >= dropCurrency)
            {
                //Player obtained currency
                Vector3 randomPosition = transform.position + Random.insideUnitSphere;
                Instantiate(CurrencyPrefab, randomPosition, Quaternion.identity);
            }
        }
    }

    void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }
}
