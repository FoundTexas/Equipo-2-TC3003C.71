using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GameManagement;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;
    public SceneManagement sm;

    void Start()
    {
        GameObject g = null;
        if (GameManager.isOnline)
        {
            g = PhotonNetwork.Instantiate(
                player.name, transform.position, Quaternion.identity);
        }
        else if (!GameManager.isOnline)
        {
            g = Instantiate(player, transform.position, Quaternion.identity);
        }

        if (g)
        {
            g.GetComponent<Move>().setCam(sm);
        }

    }
}
