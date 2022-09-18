using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.Menu
{

    public class CharacterSlot : MonoBehaviour
    {
        private const int REWARD_XP = 100;

        [SerializeField]
        private TMP_Text _nameCharacter;

        [SerializeField]
        private TMP_Text _levelCharacter;

        [SerializeField]
        private TMP_Text _mattackCharacter;

        [SerializeField]
        private TMP_Text _xpCharacter;

        [SerializeField]
        private Button _fightButton;

        private Button _lobby;

        public TMP_Text NameCharacter => _nameCharacter;
        public TMP_Text LevelCharacter => _levelCharacter;
        public TMP_Text MattackCharacter => _mattackCharacter;
        public TMP_Text XpCharacter => _xpCharacter;
        public string CharacterId { get; set; }

        private string _name;
        private int _level;
        private int _xp;

        private void Start()
        {
            _fightButton.onClick.AddListener(OnFight);

            GetCharacter();
        }

        public void SetButton(Button button)
        {
            _lobby = button;
        }

        private void GetCharacter()
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
            {
                CharacterId = CharacterId
            },
            result =>
            {
                _level = result.CharacterStatistics[LobbyStatus.CHARACTER_LEVEL];
                _xp = result.CharacterStatistics[LobbyStatus.CHARACTER_EXP];
            }, OnError);
        }

        private void OnFight()
        {
            PlayerPrefs.SetString(LobbyStatus.CHARACTER_ID, CharacterId);
            PlayerPrefs.SetString(LobbyStatus.NAME_CLASS, _nameCharacter.text);
            PlayerPrefs.Save();
            Debug.Log($"Character {PlayerPrefs.GetString(LobbyStatus.CHARACTER_ID)} select. GO TO LOBBY");
            _lobby.enabled = true;
            //var player = Random.Range(0.0f, 100.0f);
            //var enemy = Random.Range(0.0f, 100.0f);

            //if (player < enemy)
            //{
            //    Debug.Log($"Character {_nameCharacter.text} lose");
            //    return;
            //}

            //Debug.Log($"Character {_nameCharacter.text} win. He given {REWARD_XP} XP");

            //GetReward();
        }

        private void GetReward()
        {
            if (_xp + REWARD_XP >= 1000)
            {
                _level++;
                _xp = 0;
            }
            else
            {
                _xp += REWARD_XP;
            }

            _levelCharacter.text = $"{LobbyStatus.CHARACTER_LEVEL}: {_level}";
            _xpCharacter.text = $"{LobbyStatus.CHARACTER_EXP}: {_xp}";

            PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
            {
                CharacterId = CharacterId,
                CharacterStatistics = new Dictionary<string, int>
            {
                {LobbyStatus.CHARACTER_LEVEL, _level },
                {LobbyStatus.CHARACTER_EXP, _xp }
            }
            }, result =>
            {
                Debug.Log($"Character {_nameCharacter.text} update complete {_xp}");
            }, OnError);
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
        private void OnDestroy()
        {
            _fightButton.onClick.RemoveAllListeners();
        }
    }
}
