using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.Launcher
{
    public class EnterInGameWindow : MonoBehaviour
    {
        [SerializeField]
        private Button _signInButton;

        [SerializeField]
        private Button _createAcoountButton;

        [SerializeField]
        private Canvas _enterInGameCanvas;

        [SerializeField]
        private Canvas _createAccountCanvas;

        [SerializeField]
        private Canvas _signInCanvas;

        [SerializeField]
        private Canvas _restoreAccountCanvas;

        [SerializeField]
        private Button _backRestoreButton;

        [SerializeField]
        private Button _backSignInButton;

        [SerializeField]
        private Button _backCreateAccountButton;

        private void Start()
        {
            _signInButton.onClick.AddListener(OpenSingInWindow);
            _createAcoountButton.onClick.AddListener(OpenCreateAccountWindow);

            _backCreateAccountButton.onClick.AddListener(OnOpenOptionWindow);
            _backRestoreButton.onClick.AddListener(OnOpenOptionWindow);
            _backSignInButton.onClick.AddListener(OnOpenOptionWindow);
        }

        private void OnDestroy()
        {
            _signInButton.onClick.RemoveAllListeners();
            _createAcoountButton.onClick.RemoveAllListeners();
        }

        private void OpenSingInWindow()
        {
            SoundManager.PlaySoundUI(LobbyStatus.CLICK);

            _enterInGameCanvas.enabled = false;
            _restoreAccountCanvas.enabled = false;
            _signInCanvas.enabled = true;
        }

        private void OpenCreateAccountWindow()
        {
            SoundManager.PlaySoundUI(LobbyStatus.CLICK);

            _enterInGameCanvas.enabled = false;
            _restoreAccountCanvas.enabled = false;
            _createAccountCanvas.enabled = true;
        }

        private void OnOpenOptionWindow()
        {
            _enterInGameCanvas.enabled = true;
            _restoreAccountCanvas.enabled = false;
            _signInCanvas.enabled = false;
        }
    }
}
