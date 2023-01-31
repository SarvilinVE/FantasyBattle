using FantasyBattle.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace FantasyBattle.Battle
{
    public class ResultManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject _userHolder;

        [SerializeField]
        private GameObject _dataUserHolder;
        private Dictionary<int, GameObject> _dataUsers = new Dictionary<int, GameObject>();

        void Awake()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var entry = Instantiate(_dataUserHolder, _userHolder.transform);
                var dataUserHolder = entry.GetComponent<DataUserHolder>();
                var nameUser = $"{player.ActorNumber}.  {player.NickName}";
                var killsUser = $"{player.GetScore()}";
                var damageUser = $"{player.GetScore()}";

                dataUserHolder.ShowDataUserHolder(nameUser, killsUser, damageUser);
                _dataUsers.Add(player.ActorNumber, entry);
            }
        }

        public void ShowTab()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                foreach(var userData in _dataUsers)
                {
                    if(userData.Key == player.ActorNumber)
                    {
                        var dataUserHolder = userData.Value.GetComponent<DataUserHolder>();
                        var nameUser = $"{player.ActorNumber}.  {player.NickName}";
                        var killsUser = $"{player.CustomProperties[LobbyStatus.CHARACTER_KILLS]}";
                        var damageUser = $"{player.CustomProperties[LobbyStatus.CHARACTER_COUNTDAMAGE]}";

                        dataUserHolder.ShowDataUserHolder(nameUser, killsUser, damageUser);
                    }
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            ShowTab();
        }
    }
}
