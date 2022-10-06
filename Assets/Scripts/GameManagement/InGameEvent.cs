using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    [System.Serializable]
    public class InGameEventVals
    {
        public bool Ended, Persistent, Replay, Active, Triggered;
    }
    public class InGameEvent : MonoBehaviour
    {
        public InGameEventVals values;
        public bool multiscene = false;
        public int index;
        bool Hitted = false, Setted = false;

        [Tooltip("Set of instructions that affect other game objects")]
        public UnityEvent TriggerEvent;

        public void SetEnded(bool b) { values.Ended = b; }
        public void SetActive(bool b) { values.Active = b; }

        public void StartVals()
        {
            if (values.Ended && values.Triggered)
            {
                if (values.Persistent)
                {
                    values.Active = false;
                    this.gameObject.SetActive(false);
                    TriggerEvent.Invoke();
                }
                else if (values.Replay)
                {
                    values.Active = true;
                    this.gameObject.SetActive(values.Active);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
            else if (values.Ended == false)
            {
                this.gameObject.SetActive(values.Active);
            }

            Setted = true;

            if (Hitted)
            {
                SetTrigger();
            }
        }

        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// Void in charge of instantiate the UnityEvent.
        /// </summary>
        public void SetTrigger()
        {
            if (Setted)
            {
                values.Triggered = true;
                if (multiscene)
                {
                    string sname = SceneManager.GetActiveScene().name;
                    GameManager.SetEventReference(sname + "e" + index + "1");
                }
                TriggerEvent.Invoke();
            }
            else if (!Setted)
            {
                Hitted = true;
            }
        }

    }
}
