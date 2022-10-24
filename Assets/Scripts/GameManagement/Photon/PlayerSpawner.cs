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
        GameObject g;
        if (GameManager.isOnline)
        {
            g = PhotonNetwork.Instantiate(
                player.name, transform.position, Quaternion.identity);
        }
        else
        {
            g = Instantiate(player, transform.position, Quaternion.identity);
        }
        g.GetComponent<Move>().setCam(sm);

    }
}
