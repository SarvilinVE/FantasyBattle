namespace FantasyBattle.Abstractions
{
    public interface IRestore
    {

        #region Methods

        public void RestoringHp(int restoreHp);
        public void RestoringMp(int restoreMp);

        #endregion

    }
}
