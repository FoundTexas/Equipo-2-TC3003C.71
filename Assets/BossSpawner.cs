using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossSpawner : MonoBehaviour
{
    public GameObject boss;
    bool spawned = false;
    public void Spawn()
    {
        if (!spawned)
            CallSetting();
        if (GameManager.isOnline)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(
                    boss.name, transform.position, transform.rotation);
            }
        }
        else if (!GameManager.isOnline)
        {
            Instantiate(boss, transform.position, transform.rotation);
        }
    }

    [PunRPC]
    public void PunRPCSetting(bool someValue)
    {
        spawned = someValue;
    }

    void CallSetting()
    {
        if (GameManager.isOnline)
        {
            PhotonView PV = GetComponent<PhotonView>();
            PV.RPC("Setting", RpcTarget.All, true);
        }
        else if (!GameManager.isOnline)
        {
            spawned = true;
        }
    }
}
