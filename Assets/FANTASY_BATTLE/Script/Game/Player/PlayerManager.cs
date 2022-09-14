using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
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

        #endregion

        #region IPunObservable

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }

        #endregion


        #region Methods

        public void SetupPlayer()
        {
            if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(LobbyStatus.GROUP_COVEN, out var group))
            {
                if (group.ToString() == "Red coven")
                {
                    PhotonNetwork.Instantiate(_playerPrefab[0].name, _redSpawnPoints[0].position, _redSpawnPoints[0].rotation);
                }
                else
                {
                    PhotonNetwork.Instantiate(_playerPrefab[0].name, _blueSpawnPoints[0].position, _redSpawnPoints[0].rotation);
                }
            }
        }

        #endregion
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
