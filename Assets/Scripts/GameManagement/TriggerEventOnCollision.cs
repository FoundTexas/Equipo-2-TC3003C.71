using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

namespace GameManagement
{
    /// <summary>
    /// Class that handels the enabling of a UnityEvent on Trigger Collision in order affect the game in some sort.
    /// </summary>
    public class TriggerEventOnCollision : InGameEvent
    {
        // ----------------------------------------------------------------------------------------------- Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (GameManager.isOnline || PhotonNetwork.IsMasterClient)
                {

                    pv.RPC("PunRPCSetTrigger", RpcTarget.All);
                }
                else if (!GameManager.isOnline)
                {
                    PunRPCSetTrigger();
                }
            }
        }
    }
}
