using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    [System.Serializable]
    public class InGameEvent : MonoBehaviour, ISave
    {
        public bool multiscene = false;
        public int index;
        public bool Ended, Persistent, Replay, Active, Triggered;

        [Tooltip("Set of instructions that affect other game objects")]
        public UnityEvent TriggerEvent;


        void Start()
        {
            FromJson();
        }

        public void SetEnded(bool b) { Ended = b; }
        public void SetActive(bool b) { Active = b; }

        public void FromJson()
        {
            string sname = SceneManager.GetActiveScene().name;
            string s = JsonUtility.ToJson(this);

            if (PlayerPrefs.HasKey(sname + ".e" + index + ".1"))
            {
                s = PlayerPrefs.GetString(sname + ".e" + index + ".1");
            }
            JsonUtility.FromJsonOverwrite(s, this);

            if (Ended && Triggered)
            {
                if (Persistent)
                {
                    Active = false;
                    this.gameObject.SetActive(false);
                    TriggerEvent.Invoke();
                }
                else if (Replay)
                {
                    Active = true;
                    this.gameObject.SetActive(Active);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
            else if (Ended == false)
            {
                this.gameObject.SetActive(Active);
            }
        }

        public bool Save()
        {
            string sname = SceneManager.GetActiveScene().name;
            string s = JsonUtility.ToJson(this);
            PlayerPrefs.SetString(sname + ".e" + index + ".1", s);

            return true;
        }

        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// Void in charge of instantiate the UnityEvent.
        /// </summary>
        public void SetTrigger()
        {
            Triggered = true;
            if (multiscene)
            {
                string sname = SceneManager.GetActiveScene().name;
                GameManager.SetEventReference(sname + ".e" + index + ".1", this);
            }
            TriggerEvent.Invoke();
        }

    }
}
