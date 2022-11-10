using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GameManagement;

public class BossSpawner : MonoBehaviour
{
    public GameObject boss;
    public InGameEvent KillBossEvent;
    // bool spawned = false;

    void Start()
    {
        boss.SetActive(false);
    }
    public void Spawn()
    {
        if (boss.activeSelf)
            return;

        CallSetting();
    }

    [PunRPC]
    public void PunRPCSetting()
    {
        if (boss.activeSelf)
            return;
        
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
