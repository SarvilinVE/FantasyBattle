using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.Menu
{
    public class LoadingScene : MonoBehaviour
    {

        #region Fields

        [SerializeField]
        private GameObject _loadingScreen;

        [SerializeField]
        private GameObject _gamePanel;

        [SerializeField]
        private Slider _bar;

        [SerializeField]
        private float _deltaTimer;

        #endregion

        #region UnityMethods

        void Start()
        {
            _loadingScreen.SetActive(true);
            _gamePanel.SetActive(false);
        }

        private void Update()
        {
            if (_bar.value <= 0.99f)
            {
                _bar.value += Time.deltaTime * _deltaTimer;
            }
            else
            {
                _loadingScreen.SetActive(false);
                _gamePanel.SetActive(true);
            }
        }

        #endregion

    }
}
