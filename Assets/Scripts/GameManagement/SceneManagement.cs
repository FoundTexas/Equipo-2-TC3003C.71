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
        public Collectable[] collect;
        InGameEvent[] events;
        public TextMeshProUGUI Amount;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void Start()
        {
            collect = FindObjectsOfType<Collectable>();
            events = FindObjectsOfType<InGameEvent>();

            Debug.Log(JsonUtility.ToJson(this));

            FromJson();
            ChangeUI();
        }

        private void OnDestroy()
        {
            SaveValue();
        }
        // ----------------------------------------------------------------------------------------------- Pivate Methods
        void ChangeUI()
        {
            Amount.text = collect.ToList().FindAll(x => x.GetCollected() == true).Count
                + " / " + collect.ToList().Count;
        }
        // ----------------------------------------------------------------------------------------------- Public Methods
        public void SaveValue()
        {
            GameManager.SaveGame();
            ChangeUI();
        }

        void UpdateValues()
        {
            for (int i = 0; i < collect.Length; i++)
            {
                //collect[i].SetCollected(values.Values[i]);
            }
        }

        // ----------------------------------------------------------------------------------------------- ISave

        public void FromJson()
        {
            string sname = SceneManager.GetActiveScene().name;
            string s = JsonUtility.ToJson(this);

            for (int i = 0; i < collect.ToList().Count; i++)
            {
                Collectable c = collect[i];
                c.index = i;
                string tmps = JsonUtility.ToJson(c);

                Debug.Log(tmps);

                if (PlayerPrefs.HasKey(sname + ".c" + c.index + ".1"))
                {
                    tmps = PlayerPrefs.GetString(sname + ".c" + c.index + ".1");
                }
                JsonUtility.FromJsonOverwrite(tmps, c);
            }

            //foreach(var e in events)
            for (int i = 0; i < events.ToList().Count; i++)
            {
                InGameEvent e = events [i];
                e.index = i;
                string tmps = JsonUtility.ToJson(e);

                Debug.Log(tmps);

                if (PlayerPrefs.HasKey(sname + ".e" + e.index + ".1"))
                {
                    tmps = PlayerPrefs.GetString(sname + ".e" + e.index + ".1");
                }
                JsonUtility.FromJsonOverwrite(tmps, e);
                e.StartVals();
            }

        }

        public bool Save()
        {
            string sname = SceneManager.GetActiveScene().name;


            for (int i = 0; i < collect.Length; i++)
            {
                Collectable e = collect[i];
                string tmps = JsonUtility.ToJson(e);

                PlayerPrefs.SetString(sname + ".c." + i + ".1", tmps);

            }

            for (int i = 0; i < events.Length; i++)
            {
                InGameEvent e = events[i];
                string tmps = JsonUtility.ToJson(e);
                
                PlayerPrefs.SetString(sname + ".e." + i + ".1", tmps);

            }

            return true;
        }


    }
}
