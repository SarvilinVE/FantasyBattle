using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace FantasyBattle.Menu
{
    public class PlayFabAccountManager : MonoBehaviour
    {
        private const string HEALTH_POINT = "Health point";
        private const string MANA_POINT = "Mana point";

        [SerializeField]
        private TMP_Text _userId;

        [SerializeField]
        private TMP_Text _userName;

        [SerializeField]
        private TMP_Text _dateCreationAccount;

        [SerializeField]
        private TMP_Text _currentHP;

        [SerializeField]
        private TMP_Text _currentMP;

        [SerializeField]
        private int _startHP;

        [SerializeField]
        private int _startMP;

        private string _playFabId;
        public string PlayFabId => _playFabId;

        private void Start()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        }

        private void OnGetAccount(GetAccountInfoResult result)
        {
            _userId.text = $"User ID: {result.AccountInfo.PlayFabId}";
            _userName.text = $"User Name: {result.AccountInfo.Username}";
            _dateCreationAccount.text = $"Date of creation account: {result.AccountInfo.Created}";

            _playFabId = result.AccountInfo.PlayFabId;
            SetUserData();

            PlayerPrefs.SetString(LobbyStatus.USER_NAME, result.AccountInfo.Username);
        }

        private void OnError(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Error: {errorMessage}");
        }

        public void SetUserData()
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
            {
                {HEALTH_POINT, _startHP.ToString() },
                {MANA_POINT, _startMP.ToString() }
            }
            },
            result =>
            {
                Debug.Log($"Update data complete");
                GetUserData();
            }, OnError);
        }

        public void GetUserData()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest
            {
                PlayFabId = PlayFabId
            },
            result =>
            {
                _currentHP.text = result.Data[HEALTH_POINT].Value;
                _currentMP.text = result.Data[MANA_POINT].Value;
            }, OnError);
        }
    }
}