using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DisconectHandler : MonoBehaviourPunCallbacks
{
    private void OnApplicationQuit() {
        if(!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;
        PhotonNetwork.CurrentRoom.PlayerTtl = 0;
        
        foreach(var player in PhotonNetwork.PlayerListOthers)
        {
            PhotonNetwork.CloseConnection(player);
        }

        PhotonNetwork.Disconnect();
    }
}
