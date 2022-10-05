using Photon.Pun;
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

        public bool controllable = true;

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
                PhotonNetwork.Instantiate(_playerPrefab[0].name, _redSpawnPoints[Random.Range(0, _redSpawnPoints.Length)].position,
                            _redSpawnPoints[Random.Range(0, _redSpawnPoints.Length)].rotation);

                return;
            }
        }

        public void SetupBot(GameObject botPrefab)
        {
            PhotonNetwork.InstantiateRoomObject(botPrefab.name, _blueSpawnPoints[0].position, _blueSpawnPoints[0].rotation).
                GetComponent<BotCharacter>().Coven = LobbyStatus.BLUE_COVEN;
        }

        #endregion

    }
}
