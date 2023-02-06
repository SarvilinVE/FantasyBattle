using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FantasyBattle.Launcher
{
    public class AccountDataWindowsBase : MonoBehaviour
    {
        private const string CUPPON = "Character_cuppon";

        [SerializeField]
        private InputField _usernameField;

        [SerializeField]
        private InputField _passwordField;

        protected string _username;
        protected string _password;

        private CatalogItem _catalogItem;

        private string _typeCurrency;

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

            //PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            //{
            //    CatalogVersion = _catalogItem.CatalogVersion,
            //    ItemId = CUPPON
            //},
            //result =>
            //{
            //    Debug.Log($"Получил купон");
            //},
            //OnLoginError);


            SceneManager.LoadScene(1);
        }

        private void OnLoginError(PlayFabError error)
        {
            Debug.Log($"{error.ErrorMessage}");
        }
    }
}
