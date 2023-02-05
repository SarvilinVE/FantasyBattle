
using FantasyBattle.Data;
using FantasyBattle.Enums;
using FantasyBattle.Play;
using FantasyBattle.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

namespace FantasyBattle.Battle
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance = null;

        [SerializeField]
        private GameSettings _settings;

        [SerializeField]
        private LevelUpSettings _levelUpSettings;

        [Header("Result Holder")]
        [SerializeField]
        private GameObject _resultHolder;

        [Header("Game UI")]
        [SerializeField]
        private GameObject _gameUI;

        [Header("UnitInfo UI")]
        [SerializeField]
        private GameObject _unitInfoUi;

        [Header("PLayer UI")]
        [SerializeField]
        private GameObject _playerSlotHolder;

        [Header("End Game UI")]
        [SerializeField]
        private GameObject _EndGameHolderUI;

        [Header("Esc Menu UI")]
        [SerializeField]
        private GameObject _escMenuUi;

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
            SoundManager.StopMusic();
            SoundManager.PlayMusic(LobbyStatus.GAME_MUSIC);

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
            Debug.Log($"START GAMEMANAGER");
        }
        public override void OnEnable()
        {
            base.OnEnable();

            Debug.Log($"ENABLE GAMEMANAGER");
            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        }
        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }
        private void OnDestroy()
        {
            Debug.Log($"DESTROY GameManager");
            Destroy(Instance);
        }

        #endregion

        #region COROUTINES

        private IEnumerator SpawnBot()
        {
            while (Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties[LobbyStatus.CURRENT_COUNT_ENEMIES]) - 1 > 0)
            {
                yield return new WaitForSeconds(Random.Range(LobbyStatus.ENEMY_SPAWN_TIME, LobbyStatus.ENEMY_MAX_SPAWN_TIME));

                if (_countBots < _settings.CountsimultaneousEnemies)
                {
                    var enemyObject = _settings.BotPrefab[Random.Range(0, _settings.BotPrefab.Count)];
                    var enemyPosition = _settings.StartEnemiesPosition[Random.Range(0, _settings.StartEnemiesPosition.Count)]
                        .transform;

                    _playerManager.SetupBot(enemyObject, enemyPosition);
                    _countBots++;
                }
            }
        }

        private IEnumerator EndOfGame(string winner, ResultGameState resultGameState)
        {
            SetActivePanel(_resultHolder.name);

            yield return new WaitForSecondsRealtime(4);

            SetActivePanel(_EndGameHolderUI.name);
            _EndGameHolderUI.GetComponent<EndGameUI>().ShowText(winner);

            yield return new WaitForSecondsRealtime(2.5f);

            _EndGameHolderUI.GetComponent<EndGameUI>().ShowText(SetReward(PhotonNetwork.LocalPlayer, resultGameState, winner));
            yield return new WaitForSecondsRealtime(2.5f);
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnDisconnected(DisconnectCause cause)
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Clear();

            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public override void OnLeftRoom()
        {
            if (PhotonNetwork.IsMasterClient)
                StopAllCoroutines();

            SoundManager.StopMusic();
            SoundManager.PlayMusic(LobbyStatus.MENU_THEME);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            PhotonNetwork.LocalPlayer.CustomProperties.Clear();

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
            if(_resultGameState == ResultGameState.EndGame)
            {
                return;
            }

            if (targetPlayer == PhotonNetwork.LocalPlayer)
            {
                if (changedProps.ContainsKey(LobbyStatus.CURRENT_HP))
                {
                    if (Convert.ToInt32(PhotonNetwork.LocalPlayer.CustomProperties[LobbyStatus.CURRENT_HP]) == 0)
                    {
                        _resultGameState = ResultGameState.Died;
                        CheckEndOfGame();
                        return;
                    }
                }
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
            if(_resultGameState == ResultGameState.EndGame)
            {
                return;
            }

            if (propertiesThatChanged.ContainsKey(LobbyStatus.CURRENT_COUNT_ENEMIES))
            {
                if (propertiesThatChanged.TryGetValue(LobbyStatus.CURRENT_COUNT_ENEMIES, out var currentCountEnemies))
                {
                    if ((int)currentCountEnemies == _settings.MaxCountEnemiesForWin)
                    {
                        return;
                    }
                }

                if ((int)currentCountEnemies == 0)
                {
                    _resultGameState = ResultGameState.Win;
                    CheckEndOfGame();
                    return;
                }

                if (_countBots > 0)
                {
                    _countBots--;
                }
            }
        }

        #endregion


        private void StartGame()
        {

            PlayerManager.Instance.SetupPlayer(PhotonNetwork.LocalPlayer);

            if (PhotonNetwork.IsMasterClient)
            {
                //StopAllCoroutines();
                StartCoroutine(SpawnBot());
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

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
                StartCoroutine(EndOfGame(resultText, _resultGameState));
                _resultGameState = ResultGameState.EndGame;
            }
            if (_resultGameState == ResultGameState.Died)
            {
                //if (PhotonNetwork.IsMasterClient)
                //{
                //    StopAllCoroutines();
                //}

                string resultText = $"Sorry {PhotonNetwork.LocalPlayer.NickName}. You died and lost";
                StartCoroutine(EndOfGame(resultText, _resultGameState));
                _resultGameState = ResultGameState.EndGame;
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

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                var parent = FindObjectOfType<Canvas>();
                Instantiate(_escMenuUi, parent.transform);
            }
        }
        private string SetReward(Player player, ResultGameState resultGameState, string defaultText)
        {
            int baseExp = 0;
            int giveExp = 0;

            var currentLevel = Convert.ToInt32(player.CustomProperties[(LobbyStatus.CHARACTER_LEVEL)]);
            var currentExp = Convert.ToInt32(player.CustomProperties[LobbyStatus.CHARACTER_EXP]);
            var kills = Convert.ToInt32(player.CustomProperties[LobbyStatus.CHARACTER_KILLS]);
            var damage = Convert.ToInt32(player.CustomProperties[LobbyStatus.CHARACTER_COUNTDAMAGE]);

            foreach (var lvl in _levelUpSettings.Data)
            {
                if (lvl.x == currentLevel)
                {
                    baseExp = (int)lvl.z;

                    if (resultGameState == ResultGameState.Win)
                    {
                        giveExp = baseExp * kills + damage;
                    }
                    else
                    {
                        giveExp = (baseExp * kills + damage) / 2;
                    }

                    break;
                }
            }
            Debug.Log($"{kills} * {baseExp} + {damage}");
            var expForMessage = giveExp;
            var tempLevel = currentLevel;

            foreach (var lvl in _levelUpSettings.Data)
            {
                if ((int)lvl.x == tempLevel)
                {
                    if ((int)lvl.y < giveExp + currentExp)
                    {
                        giveExp = currentExp + giveExp - (int)lvl.y;
                        tempLevel ++;
                        continue;
                    }

                    string message = LobbyStatus.EMPTY;

                    if (tempLevel == currentLevel)
                    {
                        giveExp = currentExp + giveExp;
                        message = $"You have gained {expForMessage} experience";
                    }
                    else
                    {
                        message = $"You have gained {expForMessage} experience and increased your level to {tempLevel}";
                    }

                    PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
                    {
                        CharacterId = player.CustomProperties[LobbyStatus.CHARACTER_ID].ToString(),
                        CharacterStatistics = new Dictionary<string, int>
                            {
                                {LobbyStatus.CHARACTER_LEVEL, tempLevel },
                                {LobbyStatus.CHARACTER_EXP, giveExp}
                            }
                    }, result =>
                        {
                            Debug.Log($"Character {player.NickName} update complete {giveExp}");
                        }, OnError);

                    return message;
                }
                
            }
            return defaultText;
        }
        private void OnError(PlayFabError error)
        {
            var errorMesssage = error.GenerateErrorReport();
            if (errorMesssage.Contains("Item not owned"))
            {
                Debug.Log($"No character cuppon. Buy it!");
                return;
            }

            Debug.LogError($"{errorMesssage}");
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

