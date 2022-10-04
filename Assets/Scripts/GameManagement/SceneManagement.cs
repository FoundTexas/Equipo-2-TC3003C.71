using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using Collectables;
using Interfaces;

[Serializable]
public class jsonbools{
    public bool[] Values;
}

namespace GameManagement
{
    [Serializable]
    public class SceneManagement : MonoBehaviour, ISave
    {
        public jsonbools values;
        public List<Collectable> collect;
        public List<InGameEvent> events;
        public TextMeshProUGUI Amount;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void Start()
        {
            FromJson();
            ChangeUI();
        }

        private void OnDestroy()
        {
            SaveValue(null);
        }
        // ----------------------------------------------------------------------------------------------- Pivate Methods
        void ChangeUI()
        {
            Amount.text = values.Values.ToList().FindAll(x => x == true).Count
                + " / " + values.Values.ToList().Count;
        }
        // ----------------------------------------------------------------------------------------------- Public Methods
        public void SaveValue(Collectable val)
        {
            if (val)
            {
                int index = collect.FindIndex(a => a == val);
                values.Values[index] = collect[index].GetCollected();
            }

            GameManager.SaveGame();
            ChangeUI();
        }

        void UpdateValues()
        {
            for (int i = 0; i < collect.Count; i++)
            {
                collect[i].SetCollected(values.Values[i]);
            }
        }

        // ----------------------------------------------------------------------------------------------- ISave

        public void FromJson()
        {
            string sname = SceneManager.GetActiveScene().name;
            string s = JsonUtility.ToJson(this);

            if (PlayerPrefs.HasKey(sname+".1"))
            {
                s = PlayerPrefs.GetString(sname + ".1");
            }
            JsonUtility.FromJsonOverwrite(s, this);

            for (int i = 0; i < events.Count; i++)
            {
                InGameEvent e = events [i];
                e.index = i;
                string tmps = JsonUtility.ToJson(e);

                if (PlayerPrefs.HasKey(sname + ".e" + e.index + ".1"))
                {
                    tmps = PlayerPrefs.GetString(sname + ".e" + e.index + ".1");
                }
                JsonUtility.FromJsonOverwrite(tmps, e);
            }

        }

        public bool Save()
        {
            string sname = SceneManager.GetActiveScene().name;
            string s = JsonUtility.ToJson(this);

            PlayerPrefs.SetString(sname + ".1", s);

            for (int i = 0; i < events.Count; i++)
            {
                InGameEvent e = events[i];
                string tmps = JsonUtility.ToJson(e);
                
                PlayerPrefs.SetString(sname + ".e." + i + ".1", tmps);

            }

            return true;
        }


    }
}
