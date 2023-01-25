using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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

        public bool controllable = true;

        private Vector3 _botPosition;
        private Quaternion _botRotation;

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
            PhotonNetwork.InstantiateRoomObject(botPrefab.name, _blueSpawnPoints[0].position, _blueSpawnPoints[0].rotation).
                GetComponent<BotCharacter>().Coven = LobbyStatus.BLUE_COVEN;
        }

        #endregion


        #region IPunObservable

        

        #endregion
    }
}
