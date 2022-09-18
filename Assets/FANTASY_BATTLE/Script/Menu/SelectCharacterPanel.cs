using System.Collections;
using System.Collections.Generic;
using FantasyBattle.Classes;
using UnityEngine;

namespace FantasyBattle.Menu
{
    public class SelectCharacterPanel : MonoBehaviour
    {
        [SerializeField]
        private ClassConteiner _classConteiner;

        [SerializeField]
        private Transform _parent;

        [SerializeField]
        private GameObject _classCard;
        private List<ClassCard> _classCards = new List<ClassCard>();
        public List<ClassCard> ClassCards => _classCards;

        

        public void Start()
        {
            //foreach(var classCard in _classConteiner.GetClass)
            //{
            //    var card = Instantiate(_classCard, _parent).GetComponent<ClassCard>();
            //    card.ShowCard(classCard.IsLocked, classCard._IconClass, classCard.name, classCard.BaseHp, classCard.BaseMp, classCard.BaseDamage);
            //    _classCards.Add(card);
            //}
            //Debug.Log($"[SelectCharacterPanel] {_classCards.Count}");
        }

        public void ShowClassCards()
        {
            if (_classCards.Count == 0)
            {
                foreach (var classCard in _classConteiner.GetClass)
                {
                    var card = Instantiate(_classCard, _parent).GetComponent<ClassCard>();
                    card.ShowCard(classCard.IsLocked, classCard._IconClass, classCard.name, classCard.BaseHp, classCard.BaseMp, classCard.BaseDamage);
                    _classCards.Add(card);
                }
            }
            Debug.Log($"[SelectCharacterPanel] {_classCards.Count}");
        }
        public List<ClassCard> ClassCardsReturn()
        {
            return _classCards;
        }
        private void OnDestroy()
        {
            _classCards.Clear();
        }
    }
}
