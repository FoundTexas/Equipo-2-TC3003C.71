using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
        public PhotonView pv;
        public InGameEventVals values;
        public bool multiscene = false;
        public int index;
        bool Hitted = false, Setted = false;

        [Tooltip("Set of instructions that affect other game objects")]
        public UnityEvent TriggerEvent;

        void Start()
        {
            pv = GetComponent<PhotonView>();
        }

        public void SetEnded(bool b) { values.Ended = b; }
        public void SetActive(bool b) { values.Active = b; }


        public void StartVals()
        {
            if (GameManager.isOnline)
            {
                PhotonView pv = GetComponent<PhotonView>();
                pv.RPC("PunRPCstartVals", RpcTarget.All);
            }
            else if (!GameManager.isOnline)
            {
                PunRPCstartVals();
            }
        }

        [PunRPC]
        public void PunRPCstartVals()
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
                if (GameManager.isOnline)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        pv.RPC("PunRPCSetTrigger", RpcTarget.All);
                    }
                }
                else if (!GameManager.isOnline)
                {
                    PunRPCSetTrigger();
                }
            }
            else if (!Setted)
            {
                Hitted = true;
            }
        }
        [PunRPC]
        public void PunRPCSetTrigger()
        {
            values.Triggered = true;
            if (multiscene)
            {
                string sname = SceneManager.GetActiveScene().name;
                GameManager.SetEventReference(sname + "e" + index + "1");
            }
            TriggerEvent.Invoke();
        }
    }
}
