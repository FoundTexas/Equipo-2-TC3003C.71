using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

[Serializable]
public class jsonbools{
    public bool[] Values;
}

public class SceneManagement : MonoBehaviour
{
    public jsonbools values;
    public List<Collectable> collect;
    public TextMeshProUGUI Amount;

    private void Start()
    {
        string sname = SceneManager.GetActiveScene().name;
        if (PlayerPrefs.HasKey(sname))
        {
            var tmpvalues = JsonUtility.FromJson<jsonbools>(PlayerPrefs.GetString(sname));
            UpdateValues();
            if (tmpvalues.Values.ToList().Count != collect.Count)
            {
                PlayerPrefs.DeleteKey(sname);
                SaveValue(null);
            }
            else
            {
                values = tmpvalues;
                UpdateValues();
            }
        }
        else
        {
            UpdateValues();
            SaveValue(null);
        }

        ChangeUI();
    }

    public void SaveValue(Collectable val)
    {
        if (val)
        {
            int index = collect.FindIndex(a => a == val);
            values.Values[index] = collect[index].GetCollected();
        }

        string sname = SceneManager.GetActiveScene().name;
        string json = JsonUtility.ToJson(values);
        PlayerPrefs.SetString(sname, json);
        ChangeUI();
    }

    void UpdateValues()
    {
        for (int i = 0; i < collect.Count; i++)
        {
            collect[i].SetCollected(values.Values[i]);
        }
    }

    void ChangeUI()
    {
        Amount.text = values.Values.ToList().FindAll(x => x == true).Count
            + " / " + values.Values.ToList().Count;
    }

    private void OnDestroy()
    {
        SaveValue(null);
    }
}
