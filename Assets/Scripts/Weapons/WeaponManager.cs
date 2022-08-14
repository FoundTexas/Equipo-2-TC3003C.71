using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] List<Weapon> weapons;
    Dictionary<string, int> weaponDictionary;
    [SerializeField] Weapon selected;
    List<string> unlocked = new List<string>(new string[1] {"Grappling"});

    void Start()
    {
        int i = 0;
        foreach(var weapon in weapons)
        {
            weaponDictionary.Add(weapon.ID, 1);
        }
    }

    void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        if (Input.inputString != "")
        {
            int number = 1;
            bool is_a_number = Int32.TryParse(Input.inputString, out number);
            if (is_a_number && number > 0 && number <= 7)
            {
                ChangeWeapon(number - 1);
            }
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            int selectedIndex = GetSelectedIndex();
            selectedIndex -= Mathf.RoundToInt(Input.mouseScrollDelta.y);

            selectedIndex = Mathf.Clamp(selectedIndex, 0, weapons.Count-1);
            ChangeWeapon(selectedIndex);
        }

    }

    public void ChangeWeapon(int i)
    {
        if (unlocked.Count >= i)
        {
            selected = weapons[weaponDictionary[unlocked[i]]];
        }
    }

    public void UnlockWeapon(string weapon)
    {
        unlocked.Add(weapon);
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
