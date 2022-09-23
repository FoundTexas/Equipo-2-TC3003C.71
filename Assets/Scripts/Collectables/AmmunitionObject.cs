using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;

[RequireComponent(typeof(BoxCollider))]

[ RequireComponent(typeof(ParticleSystem))]
public class AmmunitionObject : MonoBehaviour
{
    ParticleSystem particle;
    bool collided = false;
    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
        GetComponent<BoxCollider>().isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collided)
        {
            collided = true;
            other.GetComponent<WeaponManager>().currentSelect().AddAmmo();
            particle.Play();
            Destroy(this, 0.1f);
        }
    }
}
