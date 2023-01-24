namespace FantasyBattle.Abstractions
{
    public interface IDamage
    {

        #region Methods

        public void DamageHp(int damage);
        public void DamageMp(int damage);

        #endregion
    }
}
