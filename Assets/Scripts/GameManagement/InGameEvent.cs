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
        public bool Hitted = false, Setted = false;

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
                pv.RPC("PunRPCstartVals", RpcTarget.All);
                pv.RPC("PunRPCSetSetted", RpcTarget.All);
            }
            else if (!GameManager.isOnline)
            {
                PunRPCstartVals();
                PunRPCSetSetted();
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

            if (Hitted)
            {
                print(PhotonNetwork.IsMasterClient + " InGameEvent trigger");
                PunRPCSetTrigger();
            }
        }

        [PunRPC]
        public void PunRPCSetSetted()
        {
            Setted = true;
        }


        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// Void in charge of instantiate the UnityEvent.
        /// </summary>
        [PunRPC]
        public void PunRPCSetTrigger()
        {
            // if (PhotonNetwork.IsMasterClient || !GameManager.isOnline)
            // {
                if (Setted)
                {

                    print("invoke");
                    TriggerEvent.Invoke();
                    values.Triggered = true;
                    if (multiscene)
                    {
                        string sname = SceneManager.GetActiveScene().name;
                        GameManager.SetEventReference(sname + "e" + index + "1");
                    }

                }
                else if (!Setted)
                {
                    print("activated");
                    Hitted = true;
                }
            // }
        }
    }
}
