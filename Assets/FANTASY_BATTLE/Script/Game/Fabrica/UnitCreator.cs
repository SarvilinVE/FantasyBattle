using FantasyBattle.Abstractions;
using FantasyBattle.Enemy;

namespace FantasyBattle.Fabrica
{
    public abstract class UnitCreator
    {

        #region Properties

        public EnemyData EnemyData { get; protected set; }

        #endregion


        #region CicleLifeMethods

        public UnitCreator()
        {
        }

        #endregion


        #region AbstractMethods

        public abstract IEnemy Create(EnemyData enemyData);

        #endregion

    }
}
