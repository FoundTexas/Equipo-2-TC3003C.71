using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float CurrencyValue = 0f;
    void OnCollisionEnter(Collision collision)
    {
        if(CurrencyValue == 0f)
        {
            // Player obtained energy

        }
        else
        {
            // Player obtained currency
            
        }
    }
}
