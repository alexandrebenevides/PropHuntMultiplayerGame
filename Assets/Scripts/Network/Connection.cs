using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Connection : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField loginField;
    private const string ROOM_NAME = "primary_room";

    public void DoLogin()
    {
        PhotonNetwork.NickName = loginField.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, new RoomOptions(), TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected.");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room not found.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected in room.");
        PhotonNetwork.LoadLevel("GameMap");
    }
}
