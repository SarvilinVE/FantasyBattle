using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _userId;

    [SerializeField]
    private TMP_Text _userName;

    [SerializeField]
    private TMP_Text _DateCreationAccount;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        _userId.text = $"User ID: {result.AccountInfo.PlayFabId}";
        _userName.text = $"User Name: {result.AccountInfo.Username}";
        _DateCreationAccount.text = $"Date of creation account: {result.AccountInfo.Created}";
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Error: {errorMessage}");
    }
}