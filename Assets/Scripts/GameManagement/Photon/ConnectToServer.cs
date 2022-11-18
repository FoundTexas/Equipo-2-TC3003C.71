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
                PhotonNetwork.AutomaticallySyncScene = true;
                infoText.text = "JoinOrCreateRoom...";
            }
        }
    }

    public override void OnJoinedRoom()
    {
        GameManager.isOnline = true;
        FindObjectOfType<SceneLoader>().LoadOnline();
    }

    public void OnClickDisconnect()
    {
        PhotonNetwork.Disconnect();
        infoText.text = "";
        GameManager.isOnline = false;
        mainPanel.SetActive(true);
        onlinePanel.SetActive(false);
        load = false;
    }

    public static void DisconectFromEvereywhere()
    {
        // if(!PhotonNetwork.IsMasterClient)
        //     return;
        // PhotonNetwork.CurrentRoom.IsOpen = false;
        // PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;
        // PhotonNetwork.CurrentRoom.PlayerTtl = 0;
        
        // foreach(var player in PhotonNetwork.PlayerListOthers)
        // {
        //     PhotonNetwork.CloseConnection(player);
        // }
    
        PhotonNetwork.Disconnect();
        GameManager.isOnline = false;
    }
}
