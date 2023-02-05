using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle
{
    public class BottomPanelUI : MonoBehaviour
    {

        #region Fields

        [SerializeField] GameObject _escMenu;
        [SerializeField] TMP_Text _stateMessage;
        [SerializeField] Button _exitButton;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _exitButton.onClick.AddListener(OnExit);
        }
        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                OnExit();
            }
        }
        private void OnDestroy()
        {
            _exitButton.onClick.RemoveAllListeners();
        }

        #endregion


        #region Methods

        private void OnExit()
        {
            SoundManager.PlaySoundUI(LobbyStatus.CLICK);
            var parent = FindObjectOfType<Canvas>();
            Instantiate(_escMenu, parent.transform);
        }

        #endregion

    }
}
