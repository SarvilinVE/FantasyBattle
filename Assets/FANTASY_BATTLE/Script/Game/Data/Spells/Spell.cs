using UnityEngine;

namespace FantasyBattle.Spells
{

    [CreateAssetMenu(fileName = nameof(Spell), menuName = "Data/Spell/" + nameof(Spell), order = 0)]
    public class Spell : ScriptableObject
    {

        #region Fields

        [SerializeField]
        private string _nameSpell;

        [SerializeField]
        private Sprite _iconSpell;

        [SerializeField]
        private UnitClass _unitClass;

        [SerializeField]
        private int _unitLevels;

        [SerializeField]
        private float _costMP;

        [SerializeField]
        private float _additionalDamage;

        [SerializeField] 
        private float _durationTime;

        [SerializeField]
        private GameObject _spellPrefab;

        [SerializeField]
        private float _timeLife;

        #endregion

        #region Properties

        public string NameSpell => _nameSpell;

        public Sprite IconSpell => _iconSpell;
        public UnitClass UnitClass => _unitClass;
        public int UnitLevels => _unitLevels;
        public float CostMP => _costMP;
        public float AdditionalDamage => _additionalDamage;
        public float DurationTime => _durationTime;
        public GameObject SpellPrefab => _spellPrefab;
        public float TimeLife => _timeLife;

        #endregion

        #region UnityMethods

        #endregion
    }
}
