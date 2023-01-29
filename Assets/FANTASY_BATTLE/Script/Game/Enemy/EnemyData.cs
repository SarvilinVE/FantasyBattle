using UnityEngine;

namespace FantasyBattle.Enemy
{
    public sealed class EnemyData
    {

        #region Properties

        public string PrefabName { get; set; }
        public int Hp { get; set; }
        public Vector3 StartPostion { get; set; }
        public Quaternion StratRotation { get; set; }

        #endregion

    }
}
