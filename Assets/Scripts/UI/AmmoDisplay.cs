using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WeaponSystem;

public class AmmoDisplay : MonoBehaviour
{
    public TMP_Text currentAmmo;
    public TMP_Text remainingAmmo;
    public TMP_Text gunName;

    public GameObject ammoDisplayUI;

    private void Update() {
        ammoDisplayUI.SetActive(WeaponManager.hasWeapon);
    }

    public void SetCurrentAmmo(string amount)
    {
        currentAmmo.text = amount;
    }

    public void SetRemainingAmmo(string amount)
    {
        remainingAmmo.text = amount;
    }
    public void SetGunName(string id)
    {
        if(id == "BasicGun")
            gunName.text = "Laser\nGun";
        else if(id == "Grappling")
            gunName.text = "Grappling\nHook";
    }

}
