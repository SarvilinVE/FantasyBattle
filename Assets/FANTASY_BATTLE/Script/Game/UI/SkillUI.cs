using FantasyBattle.Spells;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FantasyBattle.UI
{
    public class SkillUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _skillSlotUi;
        
        private Dictionary<Spell, SkillSlotUI> _slots;
        private bool _isBlockSkill;

        public bool IsBlockSkill { get => _isBlockSkill; protected set => _isBlockSkill = value; }

        #endregion


        #region UnityMethods

        private void Start()
        {
            _slots = new Dictionary<Spell, SkillSlotUI>();
        }

        private void OnDestroy()
        {
            _slots.Clear();
        }

        #endregion


        #region Methods

        public void SetData(SpellConteiner spellConteiner, int playerLevel)
        {
            
            int numberSkill = 0;

            foreach(var spell in spellConteiner.Spells)
            {
                if (spell.UnitLevels <= playerLevel)
                {
                    numberSkill++;
                    var parentTransform = transform; 
                    var slot = Instantiate(_skillSlotUi, parentTransform);
                    var skillSlotUi = slot.GetComponent<SkillSlotUI>();
                    skillSlotUi.Init(spell, numberSkill);
                    _slots.Add(spell, skillSlotUi);
                }
            }
        }

        public void RollbackSkill()
        {
            foreach(var skill in _slots) 
            {
                skill.Value.Rollback();
            }
        }
        public Spell GetActiveSpell()
        {
            foreach (var skill in _slots)
            {
                if (skill.Value.IsActive)
                {
                    _isBlockSkill = skill.Value.IsBlockSkill;
                    return skill.Key;
                }
            }

            return _slots.Keys.First();
        }
        public void ActiveSkill(string key, int numKey)
        {
            if(numKey <= _slots.Count && numKey != 0)
            {
                foreach(var skill in _slots)
                {
                    if(skill.Value.NumberSkill.text == key)
                    {
                        skill.Value.SetActive(true);
                    }
                    else
                    {
                        skill.Value.SetActive(false);
                    }
                }
            }
        }

        #endregion
    }
}
