using FantasyBattle.Enemy;

namespace FantasyBattle.Abstractions
{
    public interface IEnemy
    {

        #region Properties

        public int CurrentHp { get; set; }
        public int MaxHp { get; set; }

        #endregion


        #region Methods

        public void CreateEnemy(EnemyData enemyData);
        public void Movement();
        public void Attack();

        #endregion

    }
}
