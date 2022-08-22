using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] List<Weapon> weapons = new List<Weapon>();
    Dictionary<string, int> weaponDictionary = new Dictionary<string, int>();
    [SerializeField] Weapon selected;
    [SerializeField] List<string> unlocked = new List<string>(new string[2] {"Grappling", "SoundShoot" });

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        for (int i = 0; i < weapons.Count; i++)
        {
            weaponDictionary.Add(weapons[i].ID, i);
            weapons[i].gameObject.SetActive(false);
        }
        selected.gameObject.SetActive(true);
    }

    void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        /*
        if (Input.inputString != "")
        {
            int number = 1;
            bool is_a_number = Int32.TryParse(Input.inputString, out number);
            if (is_a_number && number > 0 && number <= 9)
            {
                ChangeWeapon(number - 1);
            }
        }*/
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
            selected.gameObject.SetActive(false);
            selected = weapons[weaponDictionary[unlocked[i]]];
            selected.gameObject.SetActive(true);
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
