using FantasyBattle.Abstractions;
using FantasyBattle.Enemy;
using FantasyBattle.Fabrica;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Play
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Fields

        [SerializeField]
        private GameObject[] _playerPrefab;

        [SerializeField]
        private Transform _parentObject;

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


        #region IPunObservable

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        #endregion


        #region Methods

        public void SetupPlayer(GameObject parent)
        {
            if (controllable)
            {
                var transformPlayer = _redSpawnPoints[Random.Range(0,_redSpawnPoints.Length)];
                PhotonNetwork.Instantiate(_playerPrefab[0].name, transformPlayer.position,
                            transformPlayer.rotation);
            }
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

        private void Update()
        {
            foreach(var enemy in _enemys)
            {
                enemy.Movement();
            }
        }

        #endregion

    }
}
