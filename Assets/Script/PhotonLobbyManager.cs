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

    private List<RoomSlot> _roomSlots;

    private ConnectAndJoinRandomLobby _connectLobby;

    void Start()
    {
        _roomSlots = new List<RoomSlot>();
        _createRoom.onClick.AddListener(CreateRoom);

        _connectLobby = new ConnectAndJoinRandomLobby(_serverSettings);
    }

    private void CreateRoom()
    {
        _roomSlot = Instantiate(_roomSlot, _lobbyPanel.transform);
        _connectLobby.OnCreatedRoom();
        _roomSlot.enterRoomButtonText.text = "Enter Room";
        _roomSlot.nameRoomText.text = _connectLobby.lbc.CurrentRoom.ToStringFull();
        _roomSlot.enterRoom.onClick.AddListener(EnterRoom);

        _roomSlots.Add(_roomSlot);

        
    }

    private void EnterRoom()
    {
        
    }

    // Update is called once per frame
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
