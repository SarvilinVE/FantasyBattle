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
        private UnitLevels _unitLevels;

        [SerializeField]
        private float _additionalDamage;

        #endregion

        #region Properties

        public string NameSpell => _nameSpell;

        public Sprite IconSpell => _iconSpell;
        public UnitClass UnitClass => _unitClass;
        public UnitLevels UnitLevels => _unitLevels;
        public float AdditionalDamage => _additionalDamage;

        #endregion
    }
}
