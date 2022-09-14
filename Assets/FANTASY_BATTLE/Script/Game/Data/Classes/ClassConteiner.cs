using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Classes
{
    [CreateAssetMenu(fileName = nameof(ClassConteiner), menuName = "Data/Class/" + nameof(ClassConteiner), order = 0)]

    public sealed class ClassConteiner : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private List<Class> _classConteiner;

        #endregion

        #region Properties

        public List<Class> GetClass => _classConteiner;

        #endregion
    }
}
