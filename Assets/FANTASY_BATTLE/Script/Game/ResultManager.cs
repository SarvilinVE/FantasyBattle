using Photon.Pun;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FantasyBattle.Battle
{
    public class ResultManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject _userHolder;

        [SerializeField]
        private TMP_Text _userInfoText;

        void Awake()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var entry = Instantiate(_userInfoText, _userHolder.transform);
                entry.text = $"{player.ActorNumber} {player.NickName}";
                Debug.Log($"{player.NickName}");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
