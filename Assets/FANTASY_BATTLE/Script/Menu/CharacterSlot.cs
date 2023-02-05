using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.Menu
{

    public class CharacterSlot : MonoBehaviour
    {
        #region Fieldas

        public static string SELECTED = "Selected";
        public static string SELECT = "Select?";

        [SerializeField]
        private TMP_Text _nameCharacter;

        [SerializeField]
        private TMP_Text _levelCharacter;

        [SerializeField]
        private TMP_Text _mattackCharacter;

        [SerializeField]
        private TMP_Text _xpCharacter;

        [SerializeField]
        private Button _selectButton;

        [SerializeField]
        private TMP_Text _selectButtonText;

        private Button _lobby;

        #endregion


        #region Properties

        public TMP_Text NameCharacter => _nameCharacter;
        public TMP_Text LevelCharacter => _levelCharacter;
        public TMP_Text MattackCharacter => _mattackCharacter;
        public TMP_Text XpCharacter => _xpCharacter;
        public string CharacterId { get; set; }

        #endregion


        #region UnityMethods

        private void Start()
        {
            if(PlayerPrefs.HasKey(LobbyStatus.CHARACTER_ID))
            {
                if (PlayerPrefs.GetString(LobbyStatus.CHARACTER_ID) == CharacterId)
                {
                    _selectButtonText.text = SELECTED;
                }
                else
                {
                    _selectButtonText.text = SELECT;
                }

                _lobby.enabled = true;
            }
            else
            {
                _selectButtonText.text = SELECT;

                _lobby.enabled = false;
            }

            _selectButton.onClick.AddListener(OnSelect);

            //GetCharacter();
        }
        private void OnDestroy()
        {
            _selectButton.onClick.RemoveAllListeners();
        }

        #endregion


        #region Methods

        public void SetButton(Button button)
        {
            _lobby = button;
        }

        //private void GetCharacter()
        //{
        //    PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
        //    {
        //        CharacterId = CharacterId
        //    },
        //    result =>
        //    {
        //        _level = result.CharacterStatistics[LobbyStatus.CHARACTER_LEVEL];
        //        _xp = result.CharacterStatistics[LobbyStatus.CHARACTER_EXP];
        //    }, OnError);
        //}

        private void OnSelect()
        {
            SoundManager.PlaySoundUI(LobbyStatus.CLICK);

            if (_selectButtonText.text == SELECT)
            {
                PlayerPrefs.SetString(LobbyStatus.CHARACTER_ID, CharacterId);
                PlayerPrefs.SetString(LobbyStatus.NAME_CLASS, _nameCharacter.text);
                PlayerPrefs.Save();

                _selectButtonText.text = SELECTED;
                _lobby.enabled = true;
            }
            else
            {
                PlayerPrefs.DeleteKey(LobbyStatus.CHARACTER_ID);
                PlayerPrefs.DeleteKey(LobbyStatus.NAME_CLASS);
                PlayerPrefs.Save();

                _selectButtonText.text = SELECT;
                _lobby.enabled = false;
            };
            //if (PlayerPrefs.HasKey(LobbyStatus.CHARACTER_ID))
            //{
            //    //if(PlayerPrefs.)
            //    PlayerPrefs.SetString(LobbyStatus.CHARACTER_ID, CharacterId);
            //    PlayerPrefs.SetString(LobbyStatus.NAME_CLASS, _nameCharacter.text);
            //    PlayerPrefs.Save();
            //    _selectButtonText.text = $"Selected";
            //}
            //else
            //{
            //    PlayerPrefs.DeleteKey(LobbyStatus.CHARACTER_ID);
            //    PlayerPrefs.DeleteKey(LobbyStatus.NAME_CLASS);

            //    _selectButtonText.text = $"Selecte?";
            //}

            Debug.Log($"Character {PlayerPrefs.GetString(LobbyStatus.CHARACTER_ID)} select. GO TO LOBBY");
            //_lobby.enabled = true;
        }

        private void OnError(PlayFabError error)
        {
            var errorMesssage = error.GenerateErrorReport();
            if (errorMesssage.Contains("Item not owned"))
            {
                Debug.Log($"No character cuppon. Buy it!");
                return;
            }

            Debug.LogError($"{errorMesssage}");
        }

        #endregion

    }
}
