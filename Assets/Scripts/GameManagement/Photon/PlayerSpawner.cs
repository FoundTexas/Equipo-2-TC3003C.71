using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GameManagement;
using Player;

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

    public void respawnPlayer(PlayerHealth p)
    {
        Debug.Log("Respawn");
        StartCoroutine(respawn(p));
    }

    IEnumerator respawn(PlayerHealth p)
    {
        yield return new WaitForSeconds(2);
        GameObject[] others = GameObject.FindGameObjectsWithTag("Player");
        if (others.Length > 0)
        {
            transform.position = GameManager.getCheckpoint();
            p.gameObject.SetActive(true);
        }
        else
        {
            p.sceneLoader.LoadOnline();
        }

        p.dead = false;
    }
}
