using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class SignInWindow : AccountDataWindowsBase
{
    #region Fields

    [SerializeField]
    private Button _signInButton;

    [SerializeField]
    private Canvas _restoreAccountWindow;

    #endregion

    #region Methods

    protected override void SubscriptionElementsUi()
    {
        base.SubscriptionElementsUi();

        _restoreAccountWindow.enabled = false;
        _signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password
        }, result =>
        {
            Debug.Log($"Success: {_username}");
            EnterInGameScene();
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}");
            _restoreAccountWindow.enabled = true;
        }); 
    }

    #endregion
}