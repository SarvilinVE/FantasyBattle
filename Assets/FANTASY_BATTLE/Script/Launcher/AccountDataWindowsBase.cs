using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FantasyBattle.Launcher
{
    public class AccountDataWindowsBase : MonoBehaviour
    {
        [SerializeField]
        private InputField _usernameField;

        [SerializeField]
        private InputField _passwordField;

        protected string _username;
        protected string _password;

        private void Start()
        {
            SoundManager.SetMusicVolume(0.1f);
            SoundManager.SetSoundVolume(0.5f);
            SoundManager.PlayMusic(LobbyStatus.MENU_THEME);
            SubscriptionElementsUi();
        }

        protected virtual void SubscriptionElementsUi()
        {
            _usernameField.onValueChanged.AddListener(UpdateUsername);
            _passwordField.onValueChanged.AddListener(UpdatePassword);
        }

        private void UpdateUsername(string username)
        {
            _username = username;
        }

        private void UpdatePassword(string password)
        {
            _password = password;
        }

        protected void EnterInGameScene()
        {
            if (PlayerPrefs.HasKey(LobbyStatus.CHARACTER_ID))
            {
                PlayerPrefs.DeleteKey(LobbyStatus.CHARACTER_ID);
                PlayerPrefs.DeleteKey(LobbyStatus.NAME_CLASS);
                Debug.Log("Clear Prefs");
            }

            
            SceneManager.LoadScene(1);
        }
    }
}
