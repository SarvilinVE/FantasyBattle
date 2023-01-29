using UnityEngine;

namespace FantasyBattle.Enemy
{
    public sealed class EnemyWarrior : EnemyMain
    {

        #region CircleLifeMethods

        public EnemyWarrior(EnemyData enemyData)
        {
            Debug.Log($" ENEMY MAG CREATE");
        }

        #endregion

        #region AbstractClassRealization

        public override void Attack()
        {
            throw new System.NotImplementedException();
        }

        public override void CreateEnemy(EnemyData enemyData)
        {
            throw new System.NotImplementedException();
        }

        public override void Movement()
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}
