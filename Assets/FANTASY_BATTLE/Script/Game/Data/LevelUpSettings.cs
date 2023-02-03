using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Data
{
    [CreateAssetMenu(fileName = nameof(LevelUpSettings), menuName = "Data/GameData/" + nameof(LevelUpSettings), order = 0)]
    public class LevelUpSettings : ScriptableObject
    {
        #region Fields

        [Header("Level/ExpRequred/BaseExpForBattle")]
        [SerializeField] private List<Vector3> _data;

        #endregion


        #region Properties

        public List<Vector3> Data => _data;

        #endregion

    }
}
