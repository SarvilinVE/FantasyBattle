using FantasyBattle.Spells;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.UI
{
    public class SkillSlotUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Image _iconSkill;
        [SerializeField] private Image _durationSkill;
        [SerializeField] private Image _panelNumberSkill;
        [SerializeField] private TMP_Text _numberSkill;
        [SerializeField] private Color _activeSkill;
        [SerializeField] private Color _inactiveSkill;

        private float _currentDurationTime;
        private bool _isActive;
        private bool _isBlockSkill;

        #endregion


        #region Properties

        public Image IconSkill { get => _iconSkill; set => _iconSkill = value; }
        public Image DurationSkill { get => _durationSkill; set => _durationSkill = value; }
        public Image PanelNumberSkill { get => _panelNumberSkill; set => _panelNumberSkill = value; }
        public TMP_Text NumberSkill { get => _numberSkill; set => _numberSkill = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }
        public bool IsBlockSkill { get => _isBlockSkill; set => _isBlockSkill = value; }

        #endregion


        #region Methods

        public void Init(Spell spell, int numberSkill)
        {
            Debug.Log($"ACTIVATE SKILL SLOT {spell.name} NUMBER SKILL {numberSkill}");
            _iconSkill.sprite = spell.IconSpell;
            _currentDurationTime = spell.DurationTime;
            _numberSkill.text = numberSkill.ToString();
            if(numberSkill == 1)
            {
                _panelNumberSkill.color = _activeSkill;
                _isActive = true;
            }
            else
            {
                _panelNumberSkill.color = _inactiveSkill;
                _isActive = false;
            }
        }
        public void Rollback()
        {
            if (_isActive)
            {
                DurationSkill.gameObject.SetActive(true);

                StartCoroutine(RollbackSkill());
            }
        }
        public IEnumerator RollbackSkill()
        {
            _isBlockSkill = true;

            var timeLeft = _currentDurationTime;
            while (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                var normalizedValue = Mathf.Clamp(timeLeft / _currentDurationTime, 0.0f, 1.0f);
                _durationSkill.fillAmount = normalizedValue;
                yield return null;
            }

            _isBlockSkill= false;
            _durationSkill.gameObject.SetActive(false);
        }
        public void SetActive(bool isSetActive)
        {
            if (isSetActive)
            {
                _panelNumberSkill.color = _activeSkill;
                _isActive = true;
            }
            else
            {
                _panelNumberSkill.color = _inactiveSkill;
                _isActive = false;
            }
        }

        #endregion
    }
}
