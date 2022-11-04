using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GameManagement;

public class BossSpawner : MonoBehaviour
{
    public GameObject boss;
    public InGameEvent KillBossEvent;
    bool spawned = false;
    public void Spawn()
    {
        if (spawned)
            return;

        CallSetting();

        TriggerEventOnDead g = null;
        if (GameManager.isOnline)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                g = PhotonNetwork.Instantiate(
                    boss.name, transform.position, transform.rotation)
                    .GetComponent<TriggerEventOnDead>();
            }
        }
        else if (!GameManager.isOnline)
        {
            g = Instantiate(boss, transform.position, transform.rotation)
            .GetComponent<TriggerEventOnDead>();
        }
        
        if(g)
        {
            g.GameEvent = KillBossEvent;
        }
    }

    [PunRPC]
    public void PunRPCSetting()
    {
        spawned = true;
    }

    void CallSetting()
    {
        if (GameManager.isOnline)
        {
            PhotonView PV = GetComponent<PhotonView>();
            PV.RPC("PunRPCSetting", RpcTarget.All);
        }
        else if (!GameManager.isOnline)
        {
            spawned = true;
        }
    }
}
