using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GameManagement;

public class BossSpawner : MonoBehaviour
{
    public GameObject boss;
    public bool spawned;
    public InGameEvent KillBossEvent;
    // bool spawned = false;

    void Start()
    {
        boss.SetActive(false);
        spawned = false;
    }
    public void Spawn()
    {
        if (spawned)
            return;

        CallSetting();
    }

    [PunRPC]
    public void PunRPCSetting()
    {

        if (spawned)
            return;
        spawned = true;
        
        boss.SetActive(true);

        TriggerEventOnDead g = null;
        g = boss.GetComponent<TriggerEventOnDead>();
        
        if(g)
        {
            g.GameEvent = KillBossEvent;
        }
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
            PunRPCSetting();
        }
    }
}
