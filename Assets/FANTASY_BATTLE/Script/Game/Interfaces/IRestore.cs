namespace FantasyBattle.Abstractions
{
    public interface IRestore
    {

        #region Methods

        public void RestoreHp(int restoreHp);
        public void RestoreMp(int restoreMp);

        #endregion

    }
}
