using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    [System.Serializable]
    public class InGameEvent : MonoBehaviour
    {
        public bool multiscene = false;
        public int index;
        public bool Ended, Persistent, Replay, Active, Triggered;

        [Tooltip("Set of instructions that affect other game objects")]
        public UnityEvent TriggerEvent;

        public void SetEnded(bool b) { Ended = b; }
        public void SetActive(bool b) { Active = b; }

        public void StartVals()
        {
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
