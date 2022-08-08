using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System;

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

    void Start()
    {
        _roomSlots = new List<RoomSlot>();
        _createRoom.onClick.AddListener(CreateRoom);

        _connectLobby = new ConnectAndJoinRandomLobby(_serverSettings, _roomInfoPrefab, _roomInfoParent);
    }

    private void CreateRoom()
    {
        _roomSlot = Instantiate(_roomSlot, _lobbyPanel.transform);
        _connectLobby.OnCreatedRoom();
        _roomSlot.enterRoomButtonText.text = "Enter Room";
        _roomSlot.nameRoomText.text = "Room";
        _roomSlot.enterRoom.onClick.AddListener(EnterRoom);

        _roomSlots.Add(_roomSlot);

        Instantiate(_roomInfoPrefab, _roomInfoParent.transform);
        
    }

    private void EnterRoom()
    {
        
    }

    void Update()
    {
        _connectLobby.Execute(_stateLobby);
    }

    private void OnDestroy()
    {
        _createRoom.onClick.RemoveAllListeners();

        foreach(var slot in _roomSlots)
        {
            slot.enterRoom.onClick.RemoveAllListeners();
        }
    }
}
