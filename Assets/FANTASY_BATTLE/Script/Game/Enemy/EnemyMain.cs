using FantasyBattle.Abstractions;

namespace FantasyBattle.Enemy
{
    public abstract class EnemyMain : IEnemy
    {
        protected int _currentHp;
        protected int _maxHp;
        public int CurrentHp { get => _currentHp; set => _currentHp = value; }
        public int MaxHp { get => _maxHp; set => _maxHp = value; }

        public abstract void Attack();

        public abstract void CreateEnemy(EnemyData enemyData);

        public abstract void Movement();
    }
}
