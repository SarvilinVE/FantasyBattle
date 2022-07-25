using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        SceneManager.LoadScene(1);
    }
}
