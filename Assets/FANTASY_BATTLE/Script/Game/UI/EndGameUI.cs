using TMPro;
using UnityEngine;

namespace FantasyBattle.UI
{
    public class EndGameUI : MonoBehaviour
    {

        #region Fields

        [SerializeField] private TMP_Text _resultStatusText;

        #endregion


        #region Methods

        public void ShowText(string text)
        {
            _resultStatusText.text = text;
        }

        #endregion

    }
}
