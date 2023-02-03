using UnityEngine;

namespace FantasyBattle.Abstractions
{
    public interface IPlayer : IDamage,IRestore
    {

        #region Properties

        public string Name { get; }
        public int CurrentHealth { get; set; }
        public int Health { get; set; }
        public int CurrentMana { get; set; }
        public int Mana { get; set; }

        #endregion

        #region Methods

        public void CreatePlayer();
        public void Movement();
        public void Attack();

        #endregion
    }
}
