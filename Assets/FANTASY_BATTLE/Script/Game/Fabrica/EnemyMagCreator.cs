using FantasyBattle.Abstractions;
using FantasyBattle.Enemy;
using UnityEngine;

namespace FantasyBattle.Fabrica
{
    public sealed class EnemyMagCreator : UnitCreator
    {

        #region CicleLifeMethods

        public EnemyMagCreator() : base() 
        { 
        }

        #endregion


        #region AbstractMethodsRealization

        public override IEnemy Create(EnemyData enemyData)
        {
            Debug.Log($"{enemyData.PrefabName} enemyCreator");
            return new EnemyMag(enemyData);
        }

        #endregion

    }
}
