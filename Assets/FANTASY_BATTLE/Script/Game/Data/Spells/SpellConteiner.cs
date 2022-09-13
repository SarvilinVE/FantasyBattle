using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Spells
{
    [CreateAssetMenu(fileName = nameof(SpellConteiner), menuName = "Data/Spell/" + nameof(SpellConteiner), order = 0)]
    public sealed class SpellConteiner : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private List<Spell> _spells;

        #endregion

        #region Properties

        public List<Spell> Spells => _spells;

        #endregion
    }
}
