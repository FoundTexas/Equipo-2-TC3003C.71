using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using GameManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [Header("Connect")]
    public Text infoText;
    public GameObject mainPanel;
    public GameObject onlinePanel;
    [Header("Lobby")]
    public Text roomName;
    bool load = false;

    public void OnClickConnect()
    {
        if (!load)
        {
            load = true;
            infoText.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        infoText.text = "Master...";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        infoText.text = "Lobby!";
        mainPanel.SetActive(false);
        onlinePanel.SetActive(true);
    }

    public void OnClickCreate()
    {
        if (load)
        {
            if (roomName.text.Length >= 1)
            {
                load = false;
                PhotonNetwork.JoinOrCreateRoom(roomName.text, new RoomOptions() { MaxPlayers = 4, }, TypedLobby.Default);
                infoText.text = "JoinOrCreateRoom...";
            }
        }
    }

    public override void OnJoinedRoom()
    {
        GameManager.isOnline = true;
        FindObjectOfType<SceneLoader>().LoadOnline();
    }
}
