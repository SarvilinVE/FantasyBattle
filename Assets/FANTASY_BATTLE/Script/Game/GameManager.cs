
using System.Collections;
using FantasyBattle.Play;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace FantasyBattle.Battle
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance = null;

        [SerializeField]
        private GameObject[] _botPrefabs;

        [Header("Result Holder")]
        [SerializeField]
        private GameObject _resultHolder;

        [Header("Game UI")]
        [SerializeField]
        private GameObject _gameUI;

        [Header("PLayer UI")]
        [SerializeField]
        private GameObject _playerSlotHolder;

        [Header("Play Manager")]
        [SerializeField]
        private PlayerManager _playerManager;

        #region UNITY

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Hashtable props = new Hashtable
            {
                {LobbyStatus.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        public override void OnEnable()
        {
            base.OnEnable();

            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        }

        #endregion

        #region COROUTINES

        //private IEnumerator SpawnBot()
        //{

        //}

        private IEnumerator EndOfGame(string winner, int score)
        {
            yield return new WaitForSecondsRealtime(5);
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnDisconnected(DisconnectCause cause)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                //StartCoroutine(SpawnAsteroid());
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            CheckEndOfGame();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(LobbyStatus.PLAYER_LIVES))
            {
                CheckEndOfGame();
                return;
            }

            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }


            // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
            int startTimestamp;
            bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

            if (changedProps.ContainsKey(LobbyStatus.PLAYER_LOADED_LEVEL))
            {
                if (CheckAllPlayerLoadedLevel())
                {
                    if (!startTimeIsSet)
                    {
                        SetActivePanel(_resultHolder.name);

                        CountdownTimer.SetStartTime();
                    }
                }
                else
                {
                    // not all players loaded yet. wait:
                    Debug.Log("setting text waiting for players! ");
                    //InfoText.text = "Waiting for other players...";
                }
            }

        }

        #endregion

        private void StartGame()
        {
            Debug.Log("StartGame!");

            // on rejoin, we have to figure out if the spaceship exists or not
            // if this is a rejoin (the ship is already network instantiated and will be setup via event) we don't need to call PN.Instantiate


            //float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
            //float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
            //float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
            //Vector3 position = new Vector3(x, 0.0f, z);
            //Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

            //PhotonNetwork.Instantiate("Spaceship", position, rotation, 0);      // avoid this call on rejoin (ship was network instantiated before)

            _playerManager.SetupPlayer(_playerSlotHolder);

            if (PhotonNetwork.IsMasterClient)
            {
                var countPlayerOnline = PhotonNetwork.CurrentRoom.PlayerCount;
                while(countPlayerOnline < PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    _playerManager.SetupBot(_botPrefabs[0]);
                    countPlayerOnline++;
                }
            }
        }

        private bool CheckAllPlayerLoadedLevel()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedLevel;

                if (p.CustomProperties.TryGetValue(LobbyStatus.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
                {
                    if ((bool)playerLoadedLevel)
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }

        private void CheckEndOfGame()
        {
            bool allDestroyed = true;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object lives;
                if (p.CustomProperties.TryGetValue(LobbyStatus.PLAYER_LIVES, out lives))
                {
                    if ((int)lives > 0)
                    {
                        allDestroyed = false;
                        break;
                    }
                }
            }

            if (allDestroyed)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    StopAllCoroutines();
                }

                string winner = "";
                int score = -1;

                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (p.GetScore() > score)
                    {
                        winner = p.NickName;
                        score = p.GetScore();
                    }
                }

                StartCoroutine(EndOfGame(winner, score));
            }
        }

        private void OnCountdownTimerIsExpired()
        {
            StartGame();
            SetActivePanel(_gameUI.name);
        }

        private void SetActivePanel(string activePanel)
        {
            _resultHolder.SetActive(activePanel.Equals(_resultHolder.name));
            _gameUI.SetActive(activePanel.Equals(_gameUI.name));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SetActivePanel(_resultHolder.name);
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                SetActivePanel(_gameUI.name);
            }
        }
    }
}

        #region OLD_VERSON
    //    private void Awake()
    //    {
    //        _playerManager.SetupPlayer(true);
    //        SetActivePanel(_resultHolder.name);
    //        StartCoroutine("ShowTabPlayers");
    //    }

    //    IEnumerator ShowTabPlayers()
    //    {
    //        yield return new WaitForSecondsRealtime(5.0f);
    //        SetActivePanel(_gameUI.name);

    //        if (PhotonNetwork.MasterClient.IsMasterClient)
    //        {
    //            if (PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
    //            {
    //                _playerManager.SetupPlayer(false);
    //            }
    //        }
    //        //_playerManager.SetupPlayer();
    //    }

    //    private void SetActivePanel(string activePanel)
    //    {
    //        _resultHolder.SetActive(activePanel.Equals(_resultHolder.name));
    //        _gameUI.SetActive(activePanel.Equals(_gameUI.name));
    //    }
    //    void Start()
    //    {

    //    }

    //    void Update()
    //    {
    //        if (Input.GetKeyDown(KeyCode.Tab))
    //        {
    //            SetActivePanel(_resultHolder.name);
    //        }

    //        if (Input.GetKeyUp(KeyCode.Tab))
    //        {
    //            SetActivePanel(_gameUI.name);
    //        }
    //    }
    //}

    #endregion

