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

    public void OnClickConnect()
    {
        infoText.text = "Connecting...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        mainPanel.SetActive(false);
        onlinePanel.SetActive(true);
        //PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate()
    {
        if(roomName.text.Length >= 1)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName.text, new RoomOptions(){MaxPlayers = 4}, TypedLobby.Default);
        }
    }

    public override void OnJoinedRoom()
    {
        GameManager.isOnline = true;
        FindObjectOfType<SceneLoader>().LoadByIndex(1);
    }
}
