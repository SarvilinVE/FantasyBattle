using FantasyBattle.Classes;
using FantasyBattle.Spells;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Play
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private GameObject _slot;
        [SerializeField] private ClassConteiner _classConteiner;

        private Dictionary<int, SlotUI> _slots = new Dictionary<int, SlotUI>();
        public Transform ParentUI { get => transform; }

        public void CreateSlot() 
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var slot = Instantiate(_slot, transform);
                var slotPlayer = slot.GetComponent<SlotUI>();

                var health = Convert.ToInt32(player.CustomProperties[LobbyStatus.CHARACTER_HP]);
                var mana = Convert.ToInt32(player.CustomProperties[LobbyStatus.CHARACTER_MP]);
                var classType = GetClass(player);

                slotPlayer.InitSlot(player.NickName, GetIconClass(classType), health, mana);
                _slots.Add(player.ActorNumber, slotPlayer);
            }
        }

        private static Sprite GetIconClass(Class classType)
        {
            return classType._IconClass;
        }

        public void PlayersInfoUpdate() 
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var health = Convert.ToInt32(player.CustomProperties[LobbyStatus.CURRENT_HP]);
                var mana = Convert.ToInt32(player.CustomProperties[LobbyStatus.CURRENT_MP]);

                if(_slots.TryGetValue(player.ActorNumber, out var slot))
                {
                    slot.UpdateData(health, mana);
                }
            }
        }
        private Class GetClass(Player player)
        {
            var classPlayer = player.CustomProperties[LobbyStatus.NAME_CLASS].ToString();

            foreach (var playerClass in _classConteiner.GetClass)
            {
                if(classPlayer == playerClass.name)
                {
                    return playerClass;
                }
            }

            Debug.LogWarning($"Error GetClass in ParantUI. Set defaul class");
            return _classConteiner.GetClass[0];
        }
    }
}
