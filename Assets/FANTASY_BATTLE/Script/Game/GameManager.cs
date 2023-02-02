
using System.Collections;
using FantasyBattle.Data;
using FantasyBattle.Enums;
using FantasyBattle.Play;
using FantasyBattle.UI;
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
        private GameSettings _settings;

        [Header("Result Holder")]
        [SerializeField]
        private GameObject _resultHolder;

        [Header("Game UI")]
        [SerializeField]
        private GameObject _gameUI;

        [Header("PLayer UI")]
        [SerializeField]
        private GameObject _playerSlotHolder;

        [Header("End Game UI")]
        [SerializeField]
        private GameObject _EndGameHolderUI;

        [Header("Play Manager")]
        [SerializeField]
        private PlayerManager _playerManager;

        private bool _isStart = false;
        private int _countBots;
        private ResultGameState _resultGameState;

        #region UNITY

        private void Awake()
        {
            Instance = this;
            //PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            Hashtable props = new Hashtable
            {
                {LobbyStatus.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            Hashtable roomProps = new Hashtable
            {
                {LobbyStatus.CURRENT_COUNT_ENEMIES, _settings.MaxCountEnemiesForWin }
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);

            _resultGameState = ResultGameState.None;
            _countBots = 0;
        }
        public override void OnEnable()
        {
            base.OnEnable();

            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        }

        #endregion

        #region COROUTINES

        private IEnumerator SpawnBot()
        {
            while ((int)PhotonNetwork.CurrentRoom.CustomProperties[LobbyStatus.CURRENT_COUNT_ENEMIES] - 1 > 0)
            {
                yield return new WaitForSeconds(Random.Range(LobbyStatus.ENEMY_SPAWN_TIME, LobbyStatus.ENEMY_MAX_SPAWN_TIME));

                if (_countBots < _settings.CountsimultaneousEnemies)
                {
                    var enemyObject = _settings.BotPrefab[Random.Range(0, _settings.BotPrefab.Count - 1)];
                    var enemyPosition = _settings.StartEnemiesPosition[Random.Range(0, _settings.StartEnemiesPosition.Count - 1)]
                        .transform;

                    _playerManager.SetupBot(enemyObject, enemyPosition);
                    _countBots++;
                }
            }
        }

        private IEnumerator EndOfGame(string winner)
        {
            SetActivePanel(_resultHolder.name);

            yield return new WaitForSecondsRealtime(5);

            SetActivePanel(_EndGameHolderUI.name);
            _EndGameHolderUI.GetComponent<EndGameUI>().ShowText(winner);

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
                StartCoroutine(SpawnBot());
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            CheckEndOfGame();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(LobbyStatus.CURRENT_HP))
            {
                CheckEndOfGame();
                return;
            }

            if (_isStart == false)
            {
                SetActivePanel(_resultHolder.name);
            }
            if (_isStart = true && _gameUI.activeSelf)
            {
                _playerSlotHolder.GetComponent<PlayerUI>().PlayersInfoUpdate();
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

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(LobbyStatus.CURRENT_COUNT_ENEMIES))
            {
                if (propertiesThatChanged.TryGetValue(LobbyStatus.CURRENT_COUNT_ENEMIES, out var currentCountEnemies))
                {
                    if ((int)currentCountEnemies == _settings.MaxCountEnemiesForWin)
                    {
                        return;
                    }
                }

                if((int)currentCountEnemies == 0)
                {
                    _resultGameState = ResultGameState.Win;
                    CheckEndOfGame();
                }

                if (_countBots - 1 >= 0)
                {
                    _countBots--;
                }
            }
        }

        #endregion


        private void StartGame()
        {
            //_isStart = true;

            //_playerSlotHolder.GetComponent<PlayerUI>().CreateSlot();
            // on rejoin, we have to figure out if the spaceship exists or not
            // if this is a rejoin (the ship is already network instantiated and will be setup via event) we don't need to call PN.Instantiate


            //float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
            //float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
            //float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
            //Vector3 position = new Vector3(x, 0.0f, z);
            //Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

            //PhotonNetwork.Instantiate("Spaceship", position, rotation, 0);      // avoid this call on rejoin (ship was network instantiated before)

            PlayerManager.Instance.SetupPlayer(PhotonNetwork.LocalPlayer);

            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(SpawnBot());
            }

            _isStart = true;

            _playerSlotHolder.GetComponent<PlayerUI>().CreateSlot();
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
            if (_resultGameState == ResultGameState.Win)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    StopAllCoroutines();
                }

                string resultText = $"Ñongratulations {PhotonNetwork.LocalPlayer.NickName}. You have won";
                StartCoroutine(EndOfGame(resultText));
            }
            else
            {
                string resultText = $"Sorry {PhotonNetwork.LocalPlayer.NickName}. You died and lost";
                StartCoroutine(EndOfGame(resultText));
            }
            //bool allDestroyed = true;

            //foreach (Player p in PhotonNetwork.PlayerList)
            //{
            //    object currentHP;
            //    if (p.CustomProperties.TryGetValue(LobbyStatus.CURRENT_HP, out currentHP))
            //    {
            //        if ((int)currentHP > 0)
            //        {
            //            allDestroyed = false;
            //            break;
            //        }
            //    }
            //}

            //if (allDestroyed)
            //{
            //    if (PhotonNetwork.IsMasterClient)
            //    {
            //        StopAllCoroutines();
            //    }

            //    string winner = "";
            //    int score = -1;

            //    foreach (Player p in PhotonNetwork.PlayerList)
            //    {
            //        if (p.GetScore() > score)
            //        {
            //            winner = p.NickName;
            //            score = p.GetScore();
            //        }
            //    }

            //    StartCoroutine(EndOfGame(winner, score));
            //}
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
            _EndGameHolderUI.SetActive(activePanel.Equals(_EndGameHolderUI.name));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _resultHolder.SetActive(true);
                
                //SetActivePanel(_resultHolder.name);
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                _resultHolder.SetActive(false);
                //SetActivePanel(_gameUI.name);
            }

            if(Input.GetKey(KeyCode.Escape))
            {

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

