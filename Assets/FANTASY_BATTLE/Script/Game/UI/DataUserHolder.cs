using TMPro;
using UnityEngine;


namespace FantasyBattle.UI
{
    public class DataUserHolder : MonoBehaviour
    {

        #region Fields

        [SerializeField] private TMP_Text _userNameText;
        [SerializeField] private TMP_Text _userCountKillsText;
        [SerializeField] private TMP_Text _userDamageText;

        #endregion


        #region Methods

        public void ShowDataUserHolder(string userName, string userCountKills, string userDamage)
        {
            _userNameText.text = userName;
            _userCountKillsText.text = userCountKills;
            _userDamageText.text = userDamage;
        }

        #endregion

    }
}
