using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.UI
{
    public class EscMenuUI : MonoBehaviour
    {
        #region Fields  

        [SerializeField] GameObject _mainMenuHolder;
        [SerializeField] GameObject _settingWindow;

        [Header("MainMenuElements")]
        [SerializeField] Button _settingButton;
        [SerializeField] Button _returnGameButton;
        [SerializeField] Button _exitRoomButton;
        [SerializeField] Button _exitGameButton;

        [Header("SettingWindowElements")]
        [SerializeField] Slider _musicVolume;
        [SerializeField] Slider _soundVolume;
        [SerializeField] Button _returnButton;

        private bool _defaultVisibleCursor;
        private CursorLockMode _defaultLockMode;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _settingButton.onClick.AddListener(OnOpenSettingWindow);
            _returnGameButton.onClick.AddListener(OnCloseEscMenu);

            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.InRoom)
                {
                    _exitRoomButton.enabled = true;
                    _exitRoomButton.onClick.AddListener(OnExitRoom);
                }
                else
                {
                    _exitGameButton.enabled = false;
                }
            }
            else
            {
                _exitRoomButton.enabled = false;
            }

            _exitGameButton.onClick.AddListener(OnExitGame);

            _mainMenuHolder.SetActive(true);
            _settingWindow.SetActive(false);

            _defaultVisibleCursor = Cursor.visible;
            _defaultLockMode = Cursor.lockState;
            Debug.Log($"DVB BEFORE {_defaultVisibleCursor}");

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnDestroy()
        {
            _settingButton.onClick.RemoveAllListeners();
            _returnGameButton.onClick.RemoveAllListeners();
            _soundVolume.onValueChanged.RemoveAllListeners();
            _returnButton.onClick.RemoveAllListeners();
            _exitRoomButton.onClick.RemoveAllListeners();
            _exitGameButton.onClick.RemoveAllListeners();
        }

        #endregion


        #region Methods

        private void OnOpenSettingWindow()
        {
            SoundManager.PlaySoundUI(LobbyStatus.CLICK);
            _settingWindow.SetActive(true);
            _musicVolume.onValueChanged.AddListener(OnChangeVolumeMusic);
            _musicVolume.value = SoundManager.GetMusicVolume();
            _soundVolume.onValueChanged.AddListener(OnChangeVolumeSound);
            _soundVolume.value = SoundManager.GetSoundVolume();
            _returnButton.onClick.AddListener(OnReturnMainMenu);
            _mainMenuHolder.SetActive(false);
        }
        private void OnCloseEscMenu()
        {
            SoundManager.PlaySoundUI(LobbyStatus.CLICK);
            Debug.Log($"CLOSE WINDOW");

            Debug.Log($"DVB AFTER {_defaultVisibleCursor}");
            Debug.Log($"Cursor Before {Cursor.visible}");
            Cursor.lockState = _defaultLockMode;
            Cursor.visible = _defaultVisibleCursor;
            Debug.Log($"Cursor AFTER {Cursor.visible}");
            Destroy(gameObject, 0.1f);
        }
        private void OnReturnMainMenu()
        {
            SoundManager.PlaySoundUI(LobbyStatus.CLICK);
            _settingWindow.SetActive(false);
            _mainMenuHolder.SetActive(true);
        }
        private void OnChangeVolumeSound(float volumeValue)
        {
            SoundManager.SetSoundVolume(volumeValue);
        }
        private void OnChangeVolumeMusic(float volumeValue)
        {
            SoundManager.SetMusicVolume(volumeValue);
        }
        private void OnExitRoom()
        {
            SoundManager.PlaySoundUI(LobbyStatus.CLICK);
            PhotonNetwork.LeaveRoom();
            Destroy(gameObject);
        }
        private void OnExitGame()
        {
            SoundManager.PlaySoundUI(LobbyStatus.CLICK);
            Application.Quit();
        }

        #endregion

    }
}
