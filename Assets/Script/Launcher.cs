using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start()
    {
        Connect();
    }

    private void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"{cause}");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"On JoinedLobby {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinOrCreateRoom("roomName", new RoomOptions { MaxPlayers = 2, IsVisible = true}, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"OnJoinedRoom {PhotonNetwork.InRoom}");

        PhotonNetwork.LoadLevel(0);
        OnDisconnected(DisconnectCause.None);
    }
}
