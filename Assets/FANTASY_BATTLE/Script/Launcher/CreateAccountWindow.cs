using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.Launcher
{
    public class CreateAccountWindow : AccountDataWindowsBase
    {
        [SerializeField]
        private InputField _emailField;

        [SerializeField]
        private Button _creatAccountButton;

        private string _email;

        protected override void SubscriptionElementsUi()
        {
            base.SubscriptionElementsUi();

            _emailField.onValueChanged.AddListener(UpdateEmail);
            _creatAccountButton.onClick.AddListener(CreateAccount);
        }

        private void UpdateEmail(string email)
        {
            _email = email;
        }

        private void CreateAccount()
        {
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
            {
                Username = _username,
                Password = _password,
                Email = _email
            }, result =>
            {
                Debug.Log($"Success: {_username}");

                EnterInGameScene();
            }, error =>
            {
                Debug.LogError($"Fail: {error.ErrorMessage}");
            });


        }
    }
}
