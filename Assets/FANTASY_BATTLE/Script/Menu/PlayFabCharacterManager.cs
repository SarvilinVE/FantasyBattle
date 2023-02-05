using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.Menu
{

    public class PlayFabCharacterManager : MonoBehaviour
    {
        private const string CUPPON = "Character_cuppon";


        [SerializeField]
        private Button _emptySlot;

        [SerializeField]
        private GameObject _selectCharacterPanel;

        [SerializeField]
        private Button _createCharacterButton;

        [SerializeField]
        private TMP_InputField _nameCharacter;

        [SerializeField]
        private GameObject _parentCharacterSlot;

        [SerializeField]
        private CharacterSlot _characterSlot;

        [SerializeField]
        private Button _lobby;

        private List<CharacterSlot> _slots = new List<CharacterSlot>();

        void Start()
        {
            GetCharacters();

            _emptySlot.onClick.AddListener(CharacterCreateWindow);
        }
        public void CreateCharacter(ClassCard card)
        {
            SoundManager.PlaySoundUI(LobbyStatus.CLICK);

            PlayerPrefs.GetString(LobbyStatus.NAME_CLASS, card.NameClass.text);

            PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
            {
                CharacterName = card.NameClass.text,
                ItemId = CUPPON
            },
            result =>
            {
                UpdateCharacterStatisticsRequest(result.CharacterId, card);
            }, OnError);
        }

        private void UpdateCharacterStatisticsRequest(string characterId, ClassCard card)
        {
            PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
            {
                CharacterId = characterId,
                CharacterStatistics = new Dictionary<string, int>
                {
                    {LobbyStatus.CHARACTER_LEVEL, 1 },
                    {LobbyStatus.CHARACTER_EXP, 0 },
                    {LobbyStatus.CHARACTER_HP, int.Parse(card.ClassHP.text) },
                    {LobbyStatus.CHARACTER_MP, int.Parse(card.ClassMP.text) },
                    {LobbyStatus.CHARACTER_DAMAGE, int.Parse(card.ClassDamage.text) },
                }
            }, result =>
                {
                    Debug.Log($"Character create complete");

                    _selectCharacterPanel.SetActive(false);

                    _emptySlot.gameObject.SetActive(true);
                    foreach (var slot in _slots)
                    {
                        slot.gameObject.SetActive(true);
                    }
                    GetCharacters();
                }, OnError);
        }

        private void CharacterCreateWindow()
        {
            SoundManager.PlaySoundUI(LobbyStatus.CLICK);

            _emptySlot.gameObject.SetActive(false);
            foreach(var slot in _slots)
            {
                slot.gameObject.SetActive(false);
            }

            _selectCharacterPanel.SetActive(true);
            _selectCharacterPanel.GetComponent<SelectCharacterPanel>().ShowClassCards();
            var classCards = _selectCharacterPanel.GetComponent<SelectCharacterPanel>().ClassCardsReturn();
            foreach(var card in classCards)
            {
                card.SelectClass.onClick.AddListener(() => CreateCharacter(card));
            }
        }

        private void GetCharacters()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    Debug.Log($"Character count: {result.Characters.Count}");
                    ShowCharacterInSlots(result.Characters);
                }, OnError);
        }

        private void ShowCharacterInSlots(List<CharacterResult> characters)
        {
            if (_slots.Count > 0)
            {
                foreach (var slot in _slots)
                    Destroy(slot.gameObject);

                _slots.Clear();
            }

            if (characters.Count > 0)
            {
                foreach (var character in characters)
                {
                    PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                    {
                        CharacterId = character.CharacterId
                    },
                    result =>
                    {
                        var level = result.CharacterStatistics[LobbyStatus.CHARACTER_LEVEL];
                        var mAttack = result.CharacterStatistics[LobbyStatus.CHARACTER_DAMAGE];
                        var xp = result.CharacterStatistics[LobbyStatus.CHARACTER_EXP];

                        var slot = Instantiate(_characterSlot, _parentCharacterSlot.transform);
                        slot.SetButton(_lobby);
                        slot.NameCharacter.text = character.CharacterName;
                        slot.LevelCharacter.text = $"Level: {level}";
                        slot.MattackCharacter.text = $"Damage: {mAttack}";
                        slot.XpCharacter.text = $"Exp: {xp}";
                        slot.CharacterId = character.CharacterId;
                        _slots.Add(slot);

                    }, OnError);
                }
            }
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
            _slots.Clear();
            _emptySlot.onClick.RemoveAllListeners();
            _createCharacterButton.onClick.RemoveAllListeners();
        }
    }
}
