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
using Player;

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

        public float Scenevalue = -20;

        public static float worldMinY = 0;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void Start()
        {
            collect = FindObjectsOfType<Collectable>();
            events = FindObjectsOfType<InGameEvent>();
            FromJson();

            worldMinY = Scenevalue;
        }

        // ----------------------------------------------------------------------------------------------- Pivate Methods
        void ChangeUI()
        {
            Amount.text = collect.ToList().FindAll(x => x.GetCollected() == true).Count
                + " / " + collect.ToList().Count;
        }
        // ----------------------------------------------------------------------------------------------- Public Methods

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

            for (int i = 0; i < collect.ToList().Count; i++)
            {
                Collectable c = collect[i];
                c.index = i;
                string tmps;

                if (PlayerPrefs.HasKey(sname + "c" + i + "1"))
                {
                    tmps = PlayerPrefs.GetString(sname + "c" + i + "1");
                }
                else
                {
                    tmps = JsonUtility.ToJson(c);
                    PlayerPrefs.SetString(sname + "c" + i + "1", tmps);
                }
                JsonUtility.FromJsonOverwrite(tmps, c);

                Debug.Log(JsonUtility.ToJson(c));
                Debug.Log(PlayerPrefs.HasKey(sname + "c" + i + "1"));
            }

            //foreach(var e in events)
            for (int i = 0; i < events.ToList().Count; i++)
            {
                InGameEvent e = events [i];
                e.index = i;
                string tmps = JsonUtility.ToJson(e.values);

                if (PlayerPrefs.HasKey(sname + "e" + i + "1"))
                {
                    tmps = PlayerPrefs.GetString(sname + "e" + i + "1");
                }
                else
                {
                    tmps = JsonUtility.ToJson(e.values);
                    PlayerPrefs.SetString(sname + "e" + i + "1", tmps);
                }
                JsonUtility.FromJsonOverwrite(tmps, e.values);
                Debug.Log(JsonUtility.ToJson(e.values));
                Debug.Log(PlayerPrefs.HasKey(sname + "e" + i + "1"));
                e.StartVals();
            }
            ChangeUI();
        }

        public bool Save()
        {
            string sname = SceneManager.GetActiveScene().name;

            for (int i = 0; i < collect.Length; i++)
            {
                Collectable e = collect[i];
                string tmps = JsonUtility.ToJson(e);

                PlayerPrefs.SetString(sname + "c" + i + "1", tmps);
                Debug.Log("Saving: " + e.name);
                Debug.Log(tmps);
            }

            for (int i = 0; i < events.Length; i++)
            {
                InGameEvent e = events[i];
                string tmps = JsonUtility.ToJson(e.values);
                
                PlayerPrefs.SetString(sname + "e" + i + "1", tmps);

                Debug.Log("Saving: " + e.name);
                Debug.Log(tmps);
            }
            Debug.Log("Saving: " + this.name);

            FromJson();

            return true;
        }


    }
}
