using FantasyBattle.Abstractions;
using FantasyBattle.Enemy;

namespace FantasyBattle.Fabrica
{
    public sealed class EnemyWarriorCreator : UnitCreator
    {

        #region CicleLifeMethods

        public EnemyWarriorCreator() : base()
        {
        }

        #endregion


        #region AbstractMethodsRealization

        public override IEnemy Create(EnemyData enemyData)
        {
            return new EnemyWarrior(enemyData);
        }

        #endregion

    }
}
