using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Droppable : MonoBehaviour
{
    public float EnergyVal, AmmoVal;
    AudioSource audios;

    private void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audios.Play();
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if(EnergyVal > 0)
            {
                player.AddHealth(EnergyVal);
            }
            if (AmmoVal > 0)
            {
                player.GetWeaponManager().currentSelect().AddAmmo();
            }

            Destroy(this.gameObject,0.3f);
        }
    }
}
