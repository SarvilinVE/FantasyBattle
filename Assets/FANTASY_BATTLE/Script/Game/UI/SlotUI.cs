using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FantasyBattle.Play
{
    public class SlotUI : MonoBehaviour
    {
        //[SerializeField]
        //private PlayerUI _parentHolder;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _nameText;

        [SerializeField]
        private Slider _barHP;

        [SerializeField]
        private Slider _barMP;

        public Slider BarHP { get => _barHP; set => _barHP = value; }
        public Slider BarMP { get => _barMP; set => _barMP = value; }

        public void Awake()
        {
            
        }

        public void InitSlot(string PlayerName, Sprite icon, float hp, float mp)
        {
            _nameText.text = PlayerName;
            _icon.sprite = icon;
            _barHP.maxValue = hp;
            _barHP.value = hp;
            _barMP.maxValue = mp;
            _barMP.value = mp;

            transform.gameObject.name = $"{PlayerName} slot";
        }
    }
}
