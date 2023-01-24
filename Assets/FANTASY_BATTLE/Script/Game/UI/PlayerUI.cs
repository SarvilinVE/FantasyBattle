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

        private List<SlotUI> _slots = new List<SlotUI>();
        public Transform ParentUI { get => transform; }

        public void CreateSlot() 
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var slot = Instantiate(_slot, transform);
                var health = Convert.ToInt32(player.CustomProperties[LobbyStatus.CHARACTER_HP]);
                var mana = Convert.ToInt32(player.CustomProperties[LobbyStatus.CHARACTER_MP]);

                var classType = GetClass(player);

                slot.GetComponent<SlotUI>().InitSlot(player.NickName, GetIconClass(classType), health, mana);
            }
        }

        private static Sprite GetIconClass(Class classType)
        {
            return classType._IconClass;
        }

        public void PlayersInfoUpdate() 
        {

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
