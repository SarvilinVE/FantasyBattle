using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Data
{
    [CreateAssetMenu(fileName = nameof(GameSettings), menuName = "Data/GameData/" + nameof(GameSettings), order = 0)]
    public class GameSettings : ScriptableObject
    {

        #region Fields

        [Header ("Enemies Settings")]
        [SerializeField] private List<GameObject> _botPrefab = new List<GameObject>();
        [SerializeField] private List<GameObject> _startEnemiesPosition = new List<GameObject>();

        [Space]
        [Header("Game Settings")]
        [SerializeField] private int _maxCountEnemiesForWin;
        [Tooltip ("The number of enemies at the same time in the game")]
        [SerializeField] private int _countSimultaneousEnemies;

        [Space]
        [Header("Level up settings")]
        [SerializeField] private LevelUpSettings _levelUpSettings;

        #endregion


        #region Properties

        public List<GameObject> BotPrefab => _botPrefab;
        public List<GameObject> StartEnemiesPosition => _startEnemiesPosition;
        public int MaxCountEnemiesForWin => _maxCountEnemiesForWin;
        public int CountsimultaneousEnemies => _countSimultaneousEnemies;
        public LevelUpSettings LevelUpSettings => _levelUpSettings;

        #endregion

    }
}
