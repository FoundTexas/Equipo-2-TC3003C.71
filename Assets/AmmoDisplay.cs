using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    public TMP_Text currentAmmo;
    public TMP_Text remainingAmmo;

    public void SetCurrentAmmo(string amount)
    {
        currentAmmo.text = amount;
    }

    public void SetRemainingAmmo(string amount)
    {
        remainingAmmo.text = amount;
    }

}
