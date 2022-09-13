using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

namespace FantasyBattle.Launcher
{
    public class PlayfabLogin : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private TMP_Text _playfabStatus;

        [SerializeField]
        private TMP_Text _photonStatus;

        [SerializeField]
        private Button _logIn;

        private const string AuthGuidKey = "auth_guid";

        #endregion

        #region UnityMethods

        private void Start()
        {
            _logIn.onClick.AddListener(OnLogin);

            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
                PlayFabSettings.staticSettings.TitleId = "5475E";
        }

        #endregion

        #region Methods

        private void OnLogin()
        {
            var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
            var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

            var request = new LoginWithCustomIDRequest
            {
                CustomId = id,
                CreateAccount = !needCreation
            };

            PlayFabClientAPI.LoginWithCustomID(request,
                success =>
                {
                    PlayerPrefs.SetString(AuthGuidKey, id);
                    OnLoginSuccess(success);
                }, OnLoginError);
        }

        private void OnLoginError(PlayFabError error)
        {
            _playfabStatus.text = "PlayFab connect error";
            _playfabStatus.color = Color.red;

            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Error: {errorMessage}");
        }

        private void OnLoginSuccess(LoginResult result)
        {
            _playfabStatus.text = "PlayFab connect complete";
            _playfabStatus.color = Color.green;

            Debug.Log("Complete!!");
        }

        private void OnDestroy()
        {
            _logIn.onClick.RemoveAllListeners();
        }

        #endregion
    }
}