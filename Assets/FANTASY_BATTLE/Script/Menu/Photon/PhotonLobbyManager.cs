using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.Menu
{
    public class PhotonLobbyManager : MonoBehaviourPunCallbacks
    {
        //[SerializeField]
        //private ServerSettings _serverSettings;

        [SerializeField]
        private GameObject _mainPanel;

        [SerializeField]
        private Button _createRoom;

        //[SerializeField]
        //private GameObject _lobbyPanel;

        [SerializeField]
        private TMP_Text _stateLobby;

        //[SerializeField]
        //private RoomSlot _roomSlot;

        //[SerializeField]
        //private CurrentRoomInfo _roomInfoPrefab;

        //[SerializeField]
        //private GameObject _roomInfoParent;
        [Header("Lobby Panel")]
        public GameObject RoomListPanel;

        public GameObject RoomListContent;
        public GameObject RoomListEntryPrefab;

        [Header("Create Room Panel")]
        public GameObject CreateRoomPanel;

        public TMP_InputField RoomNameInputField;
        public TMP_InputField MaxPlayersInputField;
        public Button CreateRoomButton;

        [Header("Inside Room Panel")]
        public GameObject InsideRoomPanel;
        public GameObject GroupOnePlayerPanel;
        public GameObject GroupTwoPlayerPanel;
        public Button StartGameButton;
        public Button LeaveRoomButton;
        public GameObject PlayerListEntryPrefab;

        private Dictionary<string, RoomInfo> _cachedRoomList;
        private Dictionary<string, GameObject> _roomListEntries;
        private Dictionary<int, GameObject> _playerListEntries;
        private Hashtable _propertiesPlayers = new Hashtable();

        #region NEW_VERSON

        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            _cachedRoomList = new Dictionary<string, RoomInfo>();
            _roomListEntries = new Dictionary<string, GameObject>();

            _createRoom.onClick.AddListener(OnOpenCreateRoomPanel);
            CreateRoomButton.onClick.AddListener(OnCreateRoomButtonClicked);
            LeaveRoomButton.onClick.AddListener(OnLeaveGameButtonClicked);
            StartGameButton.onClick.AddListener(OnStartGameButtonClicked);

            string playerName = PlayerPrefs.GetString(LobbyStatus.USER_NAME);

            if (!playerName.Equals(""))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                Debug.LogError("Player Name is invalid.");
            }

            //Сразу в лобби
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log($"{PhotonNetwork.NetworkClientState}");
            _stateLobby.text = PhotonNetwork.NetworkClientState.ToString();

            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            ClearRoomListView();

            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }

        public override void OnJoinedLobby()
        {
            _cachedRoomList.Clear();
            ClearRoomListView();
            _stateLobby.text = PhotonNetwork.NetworkClientState.ToString();

            PhotonNetwork.LocalPlayer.SetCustomProperties(
            new Hashtable
            {
                { LobbyStatus.GROUP_COVEN, LobbyStatus.RED_COVEN }
            });
        }

        public override void OnLeftLobby()
        {
            _cachedRoomList.Clear();
            ClearRoomListView();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            _stateLobby.text = message;
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            _stateLobby.text = message;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            string roomName = "Room " + Random.Range(1000, 10000);

            RoomOptions options = new RoomOptions { MaxPlayers = 8 };

            PhotonNetwork.CreateRoom(roomName, options, null);
        }

        public override void OnJoinedRoom()
        {
            // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
            _cachedRoomList.Clear();

            InsideRoomPanel.SetActive(true);

            if (_playerListEntries == null)
            {
                _playerListEntries = new Dictionary<int, GameObject>();
            }

            var currentRoom = PhotonNetwork.CurrentRoom;
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(PlayerListEntryPrefab);

                Debug.Log($"[JoinRoom1] {p.NickName} {p.CustomProperties[LobbyStatus.GROUP_COVEN]}");

                SortingPlayers(p);

                if (p.CustomProperties[LobbyStatus.GROUP_COVEN].ToString() == LobbyStatus.BLUE_COVEN)
                {
                    entry.transform.SetParent(GroupTwoPlayerPanel.transform);

                    currentRoom.SetCustomProperties(InsertOrUpdateProperties(LobbyStatus.BLUE_COVEN_COUNT_PLAYERS,
                        (int)currentRoom.CustomProperties[LobbyStatus.BLUE_COVEN_COUNT_PLAYERS] + 1, currentRoom.CustomProperties));
                }
                else
                {
                    entry.transform.SetParent(GroupOnePlayerPanel.transform);

                    currentRoom.SetCustomProperties(InsertOrUpdateProperties(LobbyStatus.RED_COVEN_COUNT_PLAYERS,
                        (int)currentRoom.CustomProperties[LobbyStatus.RED_COVEN_COUNT_PLAYERS] + 1, currentRoom.CustomProperties));
                }

                Debug.Log($"[JoinRoom2] {p.NickName} {p.CustomProperties[LobbyStatus.GROUP_COVEN]}");

                entry.transform.localScale = Vector3.one;
                entry.GetComponent<PlayerListEntry>().Initialize(p.ActorNumber, p.NickName);

                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(LobbyStatus.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
                }

                _playerListEntries.Add(p.ActorNumber, entry);
            }

            StartGameButton.gameObject.SetActive(CheckPlayersReady());

            Hashtable props = new Hashtable
            {
                {LobbyStatus.PLAYER_LOADED_LEVEL, false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            _stateLobby.text = $"[Join Room] RED : {currentRoom.CustomProperties[LobbyStatus.RED_COVEN_COUNT_PLAYERS]} BLUE: {currentRoom.CustomProperties[LobbyStatus.BLUE_COVEN_COUNT_PLAYERS]}";
            Debug.Log($"{_stateLobby.text}");
        }

        private void SortingPlayers(Player player)
        {
            var currentRoom = PhotonNetwork.CurrentRoom;

            if ((int)currentRoom.CustomProperties[LobbyStatus.RED_COVEN_COUNT_PLAYERS] > 
                (int)currentRoom.CustomProperties[LobbyStatus.BLUE_COVEN_COUNT_PLAYERS])
            {
                player.SetCustomProperties(InsertOrUpdateProperties(LobbyStatus.GROUP_COVEN, LobbyStatus.BLUE_COVEN, _propertiesPlayers));
            }
            else
            {
                player.SetCustomProperties(InsertOrUpdateProperties(LobbyStatus.GROUP_COVEN, LobbyStatus.RED_COVEN, _propertiesPlayers));
            }
        }

        public override void OnLeftRoom()
        {
            Debug.Log($"OnLeftRoom: {PhotonNetwork.LocalPlayer.NickName} leave room");

            InsideRoomPanel.SetActive(false);

            foreach (GameObject entry in _playerListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            _playerListEntries.Clear();
            _playerListEntries = null;

            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);

            if(newPlayer.CustomProperties[LobbyStatus.GROUP_COVEN].ToString() == LobbyStatus.BLUE_COVEN)
            {
                entry.transform.SetParent(GroupTwoPlayerPanel.transform);

                newPlayer.SetCustomProperties(InsertOrUpdateProperties(LobbyStatus.GROUP_COVEN, LobbyStatus.BLUE_COVEN, _propertiesPlayers));
            }
            else
            {
                entry.transform.SetParent(GroupOnePlayerPanel.transform);

                newPlayer.SetCustomProperties(InsertOrUpdateProperties(LobbyStatus.GROUP_COVEN, LobbyStatus.RED_COVEN, _propertiesPlayers));
            }

            entry.transform.localScale = Vector3.one;
            _playerListEntries.Add(newPlayer.ActorNumber, entry);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());

            _stateLobby.text = $"[Enter Room] {newPlayer.NickName} RED : {PhotonNetwork.CurrentRoom.CustomProperties[LobbyStatus.RED_COVEN_COUNT_PLAYERS]} BLUE: {PhotonNetwork.CurrentRoom.CustomProperties[LobbyStatus.BLUE_COVEN_COUNT_PLAYERS]}";
            Debug.Log($"{_stateLobby.text}");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            object typeCoven;
            otherPlayer.CustomProperties.TryGetValue(LobbyStatus.GROUP_COVEN, out typeCoven);

            Debug.Log($"{typeCoven} leave room");

            //var currentRoom = PhotonNetwork.CurrentRoom;
            //if ((string)typeCoven == LobbyStatus.RED_COVEN)
            //{
            //    currentRoom.SetCustomProperties(InsertOrUpdateProperties(LobbyStatus.RED_COVEN_COUNT_PLAYERS,
            //        (int)currentRoom.CustomProperties[LobbyStatus.RED_COVEN_COUNT_PLAYERS] - 1, currentRoom.CustomProperties));
            //}
            //else
            //{
            //    currentRoom.SetCustomProperties(InsertOrUpdateProperties(LobbyStatus.BLUE_COVEN_COUNT_PLAYERS,
            //        (int)currentRoom.CustomProperties[LobbyStatus.BLUE_COVEN_COUNT_PLAYERS] - 1, currentRoom.CustomProperties));
            //}

            Debug.Log($"OnPlayerLeftRoom: {otherPlayer.NickName} leave room");

            Destroy(_playerListEntries[otherPlayer.ActorNumber].gameObject);
            _playerListEntries.Remove(otherPlayer.ActorNumber);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                StartGameButton.gameObject.SetActive(CheckPlayersReady());
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            Debug.Log($"RoomPropertiesUpdate RED: {propertiesThatChanged[LobbyStatus.RED_COVEN_COUNT_PLAYERS]} BLUE: {propertiesThatChanged[LobbyStatus.BLUE_COVEN_COUNT_PLAYERS]}");
        }
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (_playerListEntries == null)
            {
                _playerListEntries = new Dictionary<int, GameObject>();
            }

            GameObject entry;
            if (_playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                object isPlayerReady;
                if (changedProps.TryGetValue(LobbyStatus.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
                }

                if (changedProps.TryGetValue(LobbyStatus.GROUP_COVEN, out isPlayerReady))
                {
                    entry.GetComponent<PlayerListEntry>().Initialize(targetPlayer.ActorNumber, $"{targetPlayer.NickName} ({isPlayerReady})");
                }
            }

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        #region UI CALLBACKS

        public void OnBackButtonClicked()
        {
            //if (PhotonNetwork.InLobby)
            //{
            //    PhotonNetwork.LeaveLobby();
            //}

            //SetActivePanel(SelectionPanel.name);
        }

        public void OnOpenCreateRoomPanel()
        {
            CreateRoomPanel.SetActive(true);
        }

        public void OnCreateRoomButtonClicked()
        {
            string roomName = $"{RoomNameInputField.text}";
            roomName = (roomName.Equals(string.Empty)) ? $"Room {Random.Range(1000, 10000)} ({PhotonNetwork.LocalPlayer.NickName})" :
                $"{roomName} {PhotonNetwork.LocalPlayer.NickName}";

            byte maxPlayers;
            byte.TryParse(MaxPlayersInputField.text, out maxPlayers);
            maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

            RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };

            Hashtable propertiesRoom = new Hashtable
            {
                {LobbyStatus.BLUE_COVEN_COUNT_PLAYERS, 0 },
                {LobbyStatus.RED_COVEN_COUNT_PLAYERS, 0 }
            };

            options.CustomRoomProperties = propertiesRoom;


            PhotonNetwork.CreateRoom(roomName, options, null);
            CreateRoomPanel.SetActive(false);
        }

        public void OnJoinRandomRoomButtonClicked()
        {
        }

        public void OnLeaveGameButtonClicked()
        {
            object typeCoven;
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(LobbyStatus.GROUP_COVEN, out typeCoven);
            
            Debug.Log($"{typeCoven} leave room");

            var currentRoom = PhotonNetwork.CurrentRoom;
            if ((string)typeCoven == LobbyStatus.RED_COVEN)
            {
                currentRoom.SetCustomProperties(InsertOrUpdateProperties(LobbyStatus.RED_COVEN_COUNT_PLAYERS,
                    (int)currentRoom.CustomProperties[LobbyStatus.RED_COVEN_COUNT_PLAYERS] - 1, currentRoom.CustomProperties));
            }
            else
            {
                currentRoom.SetCustomProperties(InsertOrUpdateProperties(LobbyStatus.BLUE_COVEN_COUNT_PLAYERS,
                    (int)currentRoom.CustomProperties[LobbyStatus.BLUE_COVEN_COUNT_PLAYERS] - 1, currentRoom.CustomProperties));
            }

            PhotonNetwork.LeaveRoom();
        }

        public void OnLoginButtonClicked()
        {
            //string playerName = PlayerNameInput.text;

            //if (!playerName.Equals(""))
            //{
            //    PhotonNetwork.LocalPlayer.NickName = playerName;
            //    PhotonNetwork.ConnectUsingSettings();
            //}
            //else
            //{
            //    Debug.LogError("Player Name is invalid.");
            //}
        }

        public void OnRoomListButtonClicked()
        {
            //if (!PhotonNetwork.InLobby)
            //{
            //    PhotonNetwork.JoinLobby();
            //}

            //SetActivePanel(RoomListPanel.name);
        }

        public void OnStartGameButtonClicked()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.LoadLevel(2);
        }

        #endregion

        private bool CheckPlayersReady()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return false;
            }

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(LobbyStatus.PLAYER_READY, out isPlayerReady))
                {
                    if (!(bool)isPlayerReady)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void ClearRoomListView()
        {
            foreach (GameObject entry in _roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            _roomListEntries.Clear();
        }

        public void LocalPlayerPropertiesUpdated()
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        private void SetActivePanel(string activePanel)
        {
            //LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
            //SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
            //CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
            //JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
            //RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
            InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (_cachedRoomList.ContainsKey(info.Name))
                    {
                        _cachedRoomList.Remove(info.Name);
                    }

                    continue;
                }

                // Update cached room info
                if (_cachedRoomList.ContainsKey(info.Name))
                {
                    _cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    _cachedRoomList.Add(info.Name, info);
                }
            }
        }

        private void UpdateRoomListView()
        {
            foreach (RoomInfo info in _cachedRoomList.Values)
            {
                GameObject entry = Instantiate(RoomListEntryPrefab);
                entry.transform.SetParent(RoomListContent.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

                _roomListEntries.Add(info.Name, entry);
            }
        }

        private Hashtable InsertOrUpdateProperties(object key, object value, Hashtable entries)
        {
            if(entries.ContainsKey(key))
            {
                entries[key] = value;
            }
            else
            {
                entries.Add(key, value);
            }

            return entries;
        }

        private void OnDestroy()
        {
            _cachedRoomList.Clear();
            _roomListEntries.Clear();
            _propertiesPlayers.Clear();

            _createRoom.onClick.RemoveAllListeners();
            CreateRoomButton.onClick.RemoveAllListeners();
            LeaveRoomButton.onClick.RemoveAllListeners();
            StartGameButton.onClick.RemoveAllListeners();
        }

        #endregion
    }
}


    #region OLD_VERSON
    //void Start()
    //{
    //    _createRoom.onClick.AddListener(CreateRoom);

    //    _connectLobby = new ConnectAndJoinRandomLobby(_serverSettings, _roomInfoPrefab);
    //}

    //private void CreateRoom()
    //{
    //    _roomSlot = Instantiate(_roomSlot, _lobbyPanel.transform);
    //    _roomSlot.enterRoomButtonText.text = "Enter Room";
    //    _roomSlot.nameRoomText.text = $"Room {_numberRoom}";
    //    _roomSlot.RoomParams = EnterRoomParam(_roomSlot.nameRoomText.text, TypedLobby.Default);
    //    _roomSlot.enterRoom.onClick.AddListener(EnterRoom);

    //    _createRoom.enabled = false;
    //    _numberRoom++;
    //}
    //private EnterRoomParams EnterRoomParam(string roomName, TypedLobby typedLobby)
    //{
    //    var roomOptions = new RoomOptions
    //    {
    //        MaxPlayers = 4,
    //        IsVisible = true
    //    };

    //    var enterRoomParams = new EnterRoomParams
    //    {
    //        RoomName = roomName,
    //        RoomOptions = roomOptions,
    //        ExpectedUsers = new[] { "@sd78s76awwa" },
    //        Lobby = typedLobby
    //    };

    //    return enterRoomParams;
    //}
    //private void EnterRoom()
    //{
    //    var roomWindow = CreateWindowRoom();
    //    _connectLobby.CreateRoom(roomWindow, _roomSlot.RoomParams);
    //}
    //private CurrentRoomInfo CreateWindowRoom()
    //{
    //    return Instantiate(_roomInfoPrefab, _roomInfoParent.transform);
    //}

    //void Update()
    //{
    //    _connectLobby.Execute(_stateLobby);
    //}

    //private void OnDestroy()
    //{
    //    _createRoom.onClick.RemoveAllListeners();
    //    _connectLobby.RemoveCallback();
    //    _roomSlot.enterRoom.onClick.RemoveAllListeners();
    //}

    #endregion
