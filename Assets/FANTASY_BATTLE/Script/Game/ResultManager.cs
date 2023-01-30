using FantasyBattle.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;

namespace FantasyBattle.Battle
{
    public class ResultManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject _userHolder;

        [SerializeField]
        private GameObject _dataUserHolder;

        void Awake()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var entry = Instantiate(_dataUserHolder, _userHolder.transform);
                var dataUserHolder = entry.GetComponent<DataUserHolder>();
                var nameUser = $"{player.ActorNumber} {player.NickName}";
                var killsUser = $"{player.GetScore()}";
                var damageUser = $"{player.GetScore()}";
                dataUserHolder.ShowDataUserHolder(nameUser, killsUser, damageUser);
            }
        }

        public void ShowTab()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var entry = Instantiate(_dataUserHolder, _userHolder.transform);
                var dataUserHolder = entry.GetComponent<DataUserHolder>();
                var nameUser = $"{player.ActorNumber} {player.NickName}";
                var killsUser = $"{player.GetScore()}";
                var damageUser = $"{player.GetScore()}";
                dataUserHolder.ShowDataUserHolder(nameUser, killsUser, damageUser);
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
