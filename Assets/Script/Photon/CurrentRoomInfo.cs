using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System;

public class CurrentRoomInfo : MonoBehaviour
{
    [SerializeField]
    private Button _visibleRoom;

    [SerializeField]
    private Button _openRoom;

    [SerializeField]
    private Button _startGame;

    [SerializeField]
    private TMP_Text _countPlayersRoom;

    [SerializeField]
    private TMP_Text _visibleText;

    [SerializeField]
    private TMP_Text _openText;

    private Room _room;
    private ConnectAndJoinRandomLobby _connectLobby;

    public void OpenRoom(ConnectAndJoinRandomLobby connectAndJoin, Room room)
    {
        _room = room;

        _countPlayersRoom.text = $"Players: {_room.PlayerCount}/{_room.MaxPlayers}";
        _visibleText.text = $"Visible room: {_room.IsVisible}";
        _openText.text = $"Open room: {_room.IsOpen}";
        _connectLobby = connectAndJoin;

        _visibleRoom.onClick.AddListener(SwitchVisibleRoom);
        _openRoom.onClick.AddListener(SwitchOpenRoom);
        _startGame.onClick.AddListener(StartGame);
    }
    void Start()
    {
        _visibleRoom.onClick.AddListener(SwitchVisibleRoom);
        _openRoom.onClick.AddListener(SwitchOpenRoom);
        _startGame.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        _connectLobby.lbc.OpLeaveRoom(true);
        Destroy(this);
    }

    private void SwitchOpenRoom()
    {
        if (_room.IsOpen)
        {
            var buttonColor = _openRoom.colors;
            buttonColor.normalColor = Color.red;
            _openRoom.colors = buttonColor;

            _room.IsOpen = false;
        }
        else
        {
            var buttonColor = _openRoom.colors;
            buttonColor.normalColor = Color.yellow;
            _openRoom.colors = buttonColor;

            _room.IsOpen = true;
        }
    }

    private void SwitchVisibleRoom()
    {
        if (_room.IsVisible)
        {
            var buttonColor = _visibleRoom.colors;
            buttonColor.normalColor = Color.red;
            _visibleRoom.colors = buttonColor;

            _room.IsVisible = false;
        }
        else
        {
            var buttonColor = _visibleRoom.colors;
            buttonColor.normalColor = Color.yellow;
            _visibleRoom.colors = buttonColor;

            _room.IsVisible = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        _visibleRoom.onClick.RemoveAllListeners();
        _openRoom.onClick.RemoveAllListeners();
        _startGame.onClick.RemoveAllListeners();
    }
}
