using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUnlocker : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] TextMeshPro gunText;
    Transform gun;

    private void Start()
    {
        gunText.text = "<<" + weapon.ID + ">>";
        InstantiateObject();
    }

    void InstantiateObject()
    {
        Transform parent = transform.GetChild(0).GetChild(0);
        gun = Instantiate(weapon.GunModel, parent.position, Quaternion.identity, parent).transform;
        gun.localPosition = Vector3.up;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gun)
        {
            if (other.CompareTag("Player"))
            {
                WeaponManager player = other.GetComponent<PlayerHealth>().GetWeaponManager();
                Destroy(gun.gameObject);
                player.UnlockWeapon(weapon.ID);
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
}
