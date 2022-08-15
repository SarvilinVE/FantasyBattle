using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabCharacterManager : MonoBehaviour
{
    private const string CUPPON = "Character_cuppon";
    private const string M_ATTACK = "Magic Attack";
    private const string LEVEL = "Level";
    private const string XP = "XP";

    [SerializeField]
    private Button _emptySlot;

    [SerializeField]
    private GameObject _characterCreateWindow;

    [SerializeField]
    private Button _createCharacterButton;

    [SerializeField]
    private TMP_InputField _nameCharacter;

    [SerializeField]
    private GameObject _parentCharacterSlot;

    [SerializeField]
    private CharacterSlot _characterSlot;

    private List<CharacterSlot> _slots = new List<CharacterSlot>();

    void Start()
    {
        GetCharacters();

        _emptySlot.onClick.AddListener(CharacterCreateWindow);
        _createCharacterButton.onClick.AddListener(CreateCharacter);
    }

    private void CreateCharacter()
    {
        if (_nameCharacter.text == "")
            return;

        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _nameCharacter.text,
            ItemId = CUPPON
        },
        result =>
        {
            UpdateCharacterStatisticsRequest(result.CharacterId);
        }, OnError);
    }

    private void UpdateCharacterStatisticsRequest(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {LEVEL, 1 },
                {M_ATTACK, 10 },
                {XP, 0 }
            }
        }, result =>
            {
                Debug.Log($"Character create complete");

                _characterCreateWindow.SetActive(false);
                GetCharacters();
            }, OnError);
    }

    private void CharacterCreateWindow()
    {
        _characterCreateWindow.SetActive(true);
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
                    var level = result.CharacterStatistics[LEVEL];
                    var mAttack = result.CharacterStatistics[M_ATTACK];
                    var xp = result.CharacterStatistics[XP];

                    var slot = Instantiate(_characterSlot, _parentCharacterSlot.transform);
                    slot.NameCharacter.text = character.CharacterName;
                    slot.LevelCharacter.text = $"{LEVEL}: {level}";
                    slot.MattackCharacter.text = $"{M_ATTACK}: {mAttack}";
                    slot.XpCharacter.text = $"{XP}: {xp}";
                    slot.CharacterId = character.CharacterId;
                    _slots.Add(slot);

                }, OnError) ;
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
