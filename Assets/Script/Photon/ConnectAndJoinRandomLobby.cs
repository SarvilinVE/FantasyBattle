using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectAndJoinRandomLobby : IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    private LoadBalancingClient _lbc;

    public bool isJoinRoom = false;

    private CurrentRoomInfo _currentRoomInfo;

    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);

    public LoadBalancingClient lbc => _lbc;


    public ConnectAndJoinRandomLobby(ServerSettings serverSettings, CurrentRoomInfo currentRoomInfo)
    {
        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);

        _lbc.ConnectUsingSettings(serverSettings.AppSettings);
    }
    public void Execute(TMP_Text stateText)
    {
        if (_lbc == null)
            return;

        _lbc.Service();

        stateText.text = _lbc.State.ToString();
    }
    public void RemoveCallback()
    {
        _lbc.RemoveCallbackTarget(this);
    }
    public void OnConnected()
    {

    }

    public void OnConnectedToMaster()
    {
        Debug.Log("On connected master");

        _lbc.OpJoinLobby(_customLobby);
    }
    public void CreateRoom(CurrentRoomInfo currentRoomInfo, EnterRoomParams enterRoomParams)
    {
        _currentRoomInfo = currentRoomInfo;
        _lbc.OpJoinOrCreateRoom(enterRoomParams);
    }

    public void OnCreatedRoom()
    {
        
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {

    }

    public void OnDisconnected(DisconnectCause cause)
    {

    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {

    }

    public void OnJoinedLobby()
    {
        Debug.Log($"{_lbc.CurrentLobby.GetType()}");
    }

    public void OnJoinedRoom()
    {
        Debug.Log($"On join room");
        _currentRoomInfo.OpenRoom(this);
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("On join room failsed");
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("On join room failsed");
    }

    public void OnLeftLobby()
    {

    }

    public void OnLeftRoom()
    {
        Debug.Log("On left room");
        _currentRoomInfo.LeaveRoom();
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log($"LobbyInfo");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {

    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log($"{roomList.Count}");
    }
}
