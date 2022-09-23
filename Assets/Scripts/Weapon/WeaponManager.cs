using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    /// <summary>
    /// List Of the diferent weapon inheritanced clases
    /// </summary>
    [SerializeField] List<Weapon> weapons = new List<Weapon>();
    /// <summary>
    /// Current Selected Weapon
    /// </summary>
    Weapon selected;
    /// <summary>
    /// Current Unlocked Weapon IDs
    /// </summary>
    List<string> unlocked = new List<string>(new string[2] {"Grappling", "SoundShoot" });

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] Transform torso, hand;
    [SerializeField] AudioAndVideoManager audios;
    public static bool hasWeapon = true;
    Vector3 pos;

    Dictionary<string, int> weaponDictionary = new Dictionary<string, int>();

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        for (int i = 0; i < weapons.Count; i++)
        {
            weaponDictionary.Add(weapons[i].ID, i);
            weapons[i].gameObject.SetActive(false);
        }
        toggleWeapon();

        if (unlocked.Count > 0)
        {
            selected.gameObject.SetActive(true);
        }
    }
    void Update()
    {
        Inputs();
    }
    private void LateUpdate()
    {
        transform.localPosition = pos;
    }
    void Inputs()
    {
        if (unlocked.Count > 0)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                int selectedIndex = GetSelectedIndex();
                selectedIndex += Mathf.RoundToInt(Input.mouseScrollDelta.y);

                selectedIndex = Mathf.Clamp(selectedIndex, 0, unlocked.Count - 1);
                ChangeWeapon(selectedIndex);
            }
            if (Input.GetKeyDown("q"))
            {
                toggleWeapon();
            }
        }
    }

    public void toggleWeapon()
    {
        hasWeapon = !hasWeapon;
        if (hasWeapon)
        {
            this.transform.parent = hand;
            pos = Vector3.zero;
            transform.localRotation = Quaternion.Euler(-65, 48, 54);
        }
        else if (!hasWeapon)
        {
            this.transform.parent = torso;
            pos = Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        audios.GunValue(hasWeapon, selected.sound);
    }

    public void ChangeWeapon(int i)
    {
        if (unlocked.Count >= i)
        {
            selected.gameObject.SetActive(false);
            selected = weapons[weaponDictionary[unlocked[i]]];
            selected.gameObject.SetActive(true);
        }
    }

    public void UnlockWeapon(string weapon)
    {
        if (unlocked.Contains(weapon) == false){
            unlocked.Add(weapon);
        }
        else if (unlocked.Contains(weapon))
        {
            weapons[weaponDictionary[weapon]].AddAmmo();
        }

        if (!hasWeapon)
        {
            selected = weapons[weaponDictionary[weapon]];
            ChangeWeapon(GetSelectedIndex());
            toggleWeapon();
        }
    }

    public Weapon currentSelect()
    {
        return selected;
    }

    int GetSelectedIndex()
    {
        return unlocked.IndexOf(selected.ID);
    }
}
