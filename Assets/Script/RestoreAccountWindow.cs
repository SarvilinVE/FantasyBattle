using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class RestoreAccountWindow : AccountDataWindowsBase
{

    #region Fields

    [SerializeField]
    private InputField _emailField;

    [SerializeField]
    private Button _sendButton;

    [SerializeField]
    private Canvas _signInWindow;

    private string _emailRestore;

    #endregion

    #region Methods

    protected override void SubscriptionElementsUi()
    {
        base.SubscriptionElementsUi();

        _signInWindow.enabled = false;
        _emailField.onValueChanged.AddListener(UpdateRestoreEmail);
        _sendButton.onClick.AddListener(SendMail);
    }

    private void UpdateRestoreEmail(string mail)
    {
        _emailRestore = mail;
    }

    private void SendMail()
    {
        PlayFabClientAPI.SendAccountRecoveryEmail(new SendAccountRecoveryEmailRequest
        {
            Email = _emailRestore,
            TitleId = "5475E"
        }, 
        resultCallback =>
        {
            _signInWindow.enabled = true;
            Debug.Log($"Mail send to {_emailRestore}");
            GetComponent<Canvas>().enabled = false;
        }, 
        errorCallback =>
        {
            Debug.LogError($"Fail:{_username} {errorCallback.ErrorMessage}");
        });
    }

    private void OnDestroy()
    {
        _emailField.onValueChanged.RemoveAllListeners();
        _sendButton.onClick.RemoveAllListeners();
    }

    #endregion
}
