using FantasyBattle.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FantasyBattle.UI
{
    public class SkillUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _skillSlotUi;
        
        private List<SkillSlotUI> _slots;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _slots= new List<SkillSlotUI>();
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
                    _slots.Add(skillSlotUi);
                }
            }
        }
        public void RollbackSkill()
        {
            foreach(var skill in _slots) 
            {
                if(skill.IsActive)
                {
                    skill.Rollback();
                }
            }
        }

        #endregion
    }
}
