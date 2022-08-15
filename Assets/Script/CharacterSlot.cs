using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    private const string M_ATTACK = "Magic Attack";
    private const string LEVEL = "Level";
    private const string XP = "XP";
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

    public TMP_Text NameCharacter => _nameCharacter;
    public TMP_Text LevelCharacter => _levelCharacter;
    public TMP_Text MattackCharacter => _mattackCharacter;
    public TMP_Text XpCharacter => _xpCharacter;
    public string CharacterId { get; set; }

    private int _level;
    private int _xp;

    private void Start()
    {
        _fightButton.onClick.AddListener(OnFight);

        GetCharacter();
    }

    private void GetCharacter()
    {
        PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
        {
            CharacterId = CharacterId
        },
        result =>
        {
            _level = result.CharacterStatistics[LEVEL];
            _xp = result.CharacterStatistics[XP];
        }, OnError);
    }

    private void OnFight()
    {
        var player = Random.Range(0.0f, 100.0f);
        var enemy = Random.Range(0.0f, 100.0f);

        if (player < enemy)
        {
            Debug.Log($"Character {_nameCharacter.text} lose");
            return;
        }

        Debug.Log($"Character {_nameCharacter.text} win. He given {REWARD_XP} XP");

        GetReward();
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

        _levelCharacter.text = $"{LEVEL}: {_level}";
        _xpCharacter.text = $"{XP}: {_xp}";

        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = CharacterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {LEVEL, _level },
                {M_ATTACK, 10 },
                {XP, _xp }
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
