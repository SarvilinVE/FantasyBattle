using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class ConnectAndJoinRandomLobby : IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    //[SerializeField]
    //private ServerSettings _serverSettings;

    //[SerializeField]
    //private TMP_Text _stateUiText;

    private LoadBalancingClient _lbc;

    private const string AI_KEY = "ai";
    private const string PLAYERS_KEY = "rp";

    private const string EXP_KEY = "C0";
    private const string MAP_KEY = "C1";

    private int _numRoom = 1;

    private CurrentRoomInfo _currentRoomInfo;
    private GameObject _parentPrefab;

    private TypedLobby _sqlLobby = new TypedLobby("sqlLobby", LobbyType.SqlLobby);

    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);

    private Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();

    public LoadBalancingClient lbc => _lbc;


    public ConnectAndJoinRandomLobby(ServerSettings serverSettings, CurrentRoomInfo currentRoomInfo, GameObject parentPrefab)
    {
        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);

        _lbc.ConnectUsingSettings(serverSettings.AppSettings);

        _currentRoomInfo = currentRoomInfo;
        _parentPrefab = parentPrefab;
    }
    public void Execute(TMP_Text stateText)
    {
        if (_lbc == null)
            return;

        _lbc.Service();

        stateText.text = _lbc.State.ToString();

        //_stateUiText.text = $"State: {state}, UserID: {_lbc.UserId}";
    }
    private void RemoveCallback()
    {
        _lbc.RemoveCallbackTarget(this);
    }
    public void OnConnected()
    {

    }

    public void OnConnectedToMaster()
    {
        Debug.Log("On connected master");

        //var roomOptions = new RoomOptions
        //{
        //    MaxPlayers = 4,
        //    IsVisible = true,
        //    CustomRoomPropertiesForLobby = new[]
        //    {
        //        EXP_KEY,
        //        MAP_KEY
        //    },
        //    CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
        //    {
        //        {EXP_KEY, 400 },
        //        {MAP_KEY, "Green Garden" }
        //    }
        //};

        //var enterRoomParams = new EnterRoomParams
        //{
        //    RoomName = "NewRoom",
        //    RoomOptions = roomOptions,
        //    ExpectedUsers = new[] {"@sd78s76awwa"},
        //    Lobby = _customLobby
        //};

        //_lbc.OpCreateRoom(enterRoomParams);
        _lbc.OpJoinLobby(_customLobby);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                _cachedRoomList.Remove(info.Name);
            }
            else
            {
                _cachedRoomList[info.Name] = info;
            }
        }
    }
    public void CreateRoom()
    {
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            CustomRoomPropertiesForLobby = new[]
            {
                EXP_KEY,
                MAP_KEY
            },
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                {EXP_KEY, 400 },
                {MAP_KEY, "Green Garden" }
            }
        };

        var enterRoomParams = new EnterRoomParams
        {
            RoomName = $"NewRoom {_numRoom}",
            RoomOptions = roomOptions,
            ExpectedUsers = new[] { "@sd78s76awwa" },
            Lobby = _customLobby
        };

        _lbc.OpCreateRoom(enterRoomParams);
        _numRoom++;
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
        //_cachedRoomList.Clear();

        //var sqlLobbyFilter = $"{EXP_KEY} BETWEEN 300 AND 500 AND {MAP_KEY} = 'Green Garden'";
        //var opJoinRooomParams = new OpJoinRandomRoomParams
        //{
        //    SqlLobbyFilter = sqlLobbyFilter
        //};

        //Debug.Log("On join Lobby");
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            CustomRoomPropertiesForLobby = new[]
            {
                EXP_KEY,
                MAP_KEY
            },
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                {EXP_KEY, 400 },
                {MAP_KEY, "Green Garden" }
            }
        };

        var enterRoomParams = new EnterRoomParams
        {
            RoomName = $"NewRoom {_numRoom}",
            RoomOptions = roomOptions,
            ExpectedUsers = new[] { "@sd78s76awwa" },
            Lobby = _customLobby
        };

        _lbc.OpCreateRoom(enterRoomParams);
        _numRoom++;

        Debug.Log($"{_lbc.CurrentLobby.Name}");
    }

    public void OnJoinedRoom()
    {
        Debug.Log("On join room");

        _currentRoomInfo.OpenRoom(this, _lbc.CurrentRoom);
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("On join room failsed");
        
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {

    }

    public void OnLeftLobby()
    {

    }

    public void OnLeftRoom()
    {
        Debug.Log("On left room");
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

    }
}
