using FantasyBattle.Enemy;
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

        [SerializeField]
        private int _enemyMaxCount;

        public bool controllable = true;
        private List<EnemyView> _enemys = new List<EnemyView>();
        private int _countCreationEnemy = 0;
        private GameObject _botPrefab;


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

        public void SetupBot(GameObject botPrefab, Transform enemyTransform)
        {
            EnemyData enemyData = new EnemyData
            {
                PrefabName = botPrefab.name,
                Hp = 20,
                StartPostion = enemyTransform.position,
                StratRotation = enemyTransform.rotation
            };

            var enemy = PhotonNetwork.InstantiateRoomObject(enemyData.PrefabName, enemyData.StartPostion, enemyData.StratRotation, 0);
            var enemyView = enemy.GetComponent<EnemyView>();
            enemyView.Init(enemyData);
        }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Instance = this;
        }
        private void OnDestroy()
        {
            Debug.Log($"DESTROY PlayerManager");
            Destroy(Instance);
        }

        #endregion

    }
}
