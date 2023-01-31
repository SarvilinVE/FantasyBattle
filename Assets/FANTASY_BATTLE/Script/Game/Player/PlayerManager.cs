using FantasyBattle.Abstractions;
using FantasyBattle.Enemy;
using FantasyBattle.Fabrica;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Play
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        #region Fields

        public static PlayerManager Instance = null;

        [SerializeField]
        private GameObject[] _playerPrefab;

        [SerializeField]
        private Transform _parentObject;

        [SerializeField]
        private GameObject _unitInfoUi;

        private GameObject _playerCharacter;

        [SerializeField]
        private Transform[] _redSpawnPoints;

        [SerializeField]
        private Transform[] _blueSpawnPoints;

        [SerializeField]
        private int _enemyCount;

        public bool controllable = true;
        private List<IEnemy> _enemys = new List<IEnemy>();


        #endregion


        #region Methods

        public void SetupPlayer(Player localPlayer)
        {

            if (localPlayer.CustomProperties.TryGetValue(LobbyStatus.NAME_CLASS, out var playerPrefab))
            {
                var player = (string)playerPrefab;

                var transformPlayer = _redSpawnPoints[Random.Range(0, _redSpawnPoints.Length)];
                PhotonNetwork.Instantiate(player, transformPlayer.position,
                            transformPlayer.rotation, 0);

                Debug.Log($"Create Player MAsterClient {localPlayer.ActorNumber}");
            }
            //else
            //{
            //    if (localPlayer.CustomProperties.TryGetValue(LobbyStatus.NAME_CLASS, out var playerPrefab))
            //    {
            //        var player = (string)playerPrefab;

            //        var transformPlayer = _redSpawnPoints[Random.Range(0, _redSpawnPoints.Length)];
            //        PhotonNetwork.Instantiate(player, transformPlayer.position,
            //                    transformPlayer.rotation, 0);
            //        Debug.Log($"Create Player Not MAsterClient {localPlayer.ActorNumber}");
            //    }
            //}
        }

        public void SetupBot(GameObject botPrefab)
        {
            //PhotonNetwork.InstantiateRoomObject(botPrefab.name, _blueSpawnPoints[0].position, _blueSpawnPoints[0].rotation).
            //    GetComponent<BotCharacter>().Coven = LobbyStatus.ENEMY_TAG;
            if (_enemyCount <= _enemys.Count)
            {

                EnemyData enemyData = new EnemyData
                {
                    PrefabName = botPrefab.name,
                    Hp = 200,
                    StartPostion = _blueSpawnPoints[0].position,
                    StratRotation = _blueSpawnPoints[0].rotation
                };

                UnitCreator enemyCreator = new EnemyMagCreator();
                _enemys.Add(enemyCreator.Create(enemyData));
            }
        }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            foreach (var enemy in _enemys)
            {
                enemy.Movement();
            }
        }

        #endregion

    }
}
