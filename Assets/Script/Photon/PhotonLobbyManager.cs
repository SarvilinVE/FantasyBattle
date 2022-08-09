using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobbyManager : MonoBehaviour
{
    [SerializeField]
    private ServerSettings _serverSettings;

    [SerializeField]
    private Button _createRoom;

    [SerializeField]
    private GameObject _lobbyPanel;

    [SerializeField]
    private TMP_Text _stateLobby;

    [SerializeField]
    private RoomSlot _roomSlot;

    [SerializeField]
    private CurrentRoomInfo _roomInfoPrefab;

    [SerializeField]
    private GameObject _roomInfoParent;

    private List<RoomSlot> _roomSlots;

    private ConnectAndJoinRandomLobby _connectLobby;

    private int _numberRoom = 1;

    void Start()
    {
        _roomSlots = new List<RoomSlot>();
        _createRoom.onClick.AddListener(CreateRoom);

        _connectLobby = new ConnectAndJoinRandomLobby(_serverSettings, _roomInfoPrefab);
    }

    private void CreateRoom()
    {
        _roomSlot = Instantiate(_roomSlot, _lobbyPanel.transform);
        _roomSlot.enterRoomButtonText.text = "Enter Room";
        _roomSlot.nameRoomText.text = $"Room {_numberRoom}";
        _roomSlot.RoomParams = EnterRoomParam(_roomSlot.nameRoomText.text, TypedLobby.Default);
        _roomSlot.enterRoom.onClick.AddListener(EnterRoom);

        _roomSlots.Add(_roomSlot);
        _numberRoom++;
    }
    private EnterRoomParams EnterRoomParam(string roomName, TypedLobby typedLobby)
    {
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true
        };

        var enterRoomParams = new EnterRoomParams
        {
            RoomName = roomName,
            RoomOptions = roomOptions,
            ExpectedUsers = new[] { "@sd78s76awwa" },
            Lobby = typedLobby
        };

        return enterRoomParams;
    }
    private void EnterRoom()
    {
        var roomWindow = CreateWindowRoom();
        _connectLobby.CreateRoom(roomWindow, _roomSlot.RoomParams);
    }
    private CurrentRoomInfo CreateWindowRoom()
    {
        return Instantiate(_roomInfoPrefab, _roomInfoParent.transform);
    }

    void Update()
    {
        _connectLobby.Execute(_stateLobby);
    }

    private void OnDestroy()
    {
        _createRoom.onClick.RemoveAllListeners();
        _connectLobby.RemoveCallback();

        foreach(var slot in _roomSlots)
        {
            slot.enterRoom.onClick.RemoveAllListeners();
        }
    }
}
