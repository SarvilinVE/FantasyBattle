using FantasyBattle.Spells;
using UnityEngine;

namespace FantasyBattle.Classes
{
    [CreateAssetMenu(fileName = nameof(Class), menuName = "Data/Class/" + nameof(Class), order = 0)]

    public sealed class Class : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private UnitClass _unitClass;

        [SerializeField]
        private Sprite _iconClass;

        [SerializeField]
        private SpellConteiner _spellClass;

        [SerializeField]
        private float _baseHp;

        [SerializeField]
        private float _baseMp;

        [SerializeField]
        private float _baseDamage;

        [SerializeField]
        private bool _isLocked;

        #endregion

        #region Properties

        public UnitClass UnitClass => _unitClass;
        public Sprite _IconClass => _iconClass;
        public float BaseHp => _baseHp;
        public float BaseMp => _baseMp;
        public float BaseDamage => _baseDamage;
        public bool IsLocked => _isLocked;
        public SpellConteiner SpellClass => _spellClass;

        #endregion

    }
}
