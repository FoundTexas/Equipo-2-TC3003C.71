using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
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
    private static SceneLoader sceneLoader;

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
                PhotonNetwork.JoinOrCreateRoom(roomName.text, new RoomOptions() { MaxPlayers = 4}, TypedLobby.Default);
                // PhotonNetwork.AutomaticallySyncScene = true;
                infoText.text = "JoinOrCreateRoom...";
            }
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print(returnCode);
        print(message);
    }

    public override void OnJoinedRoom()
    {
        GameManager.isOnline = true;
        FindObjectOfType<SceneLoader>().LoadOnline();
    }

    public void OnClickDisconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public static void DisconectFromEvereywhere(SceneLoader _sceneLoader)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerListOthers)
            {
                PhotonNetwork.DestroyPlayerObjects(player);
                PhotonNetwork.CloseConnection(player);
            }
        }
        sceneLoader = _sceneLoader;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
            FindObjectOfType<SceneLoader>().LoadByIndex(0);
        

        infoText.text = "";
        GameManager.isOnline = false;
        mainPanel.SetActive(true);
        onlinePanel.SetActive(false);
        
        load = false;
    }
}
