using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentRoomInfo : MonoBehaviour
{
    [SerializeField]
    private Button _visibleRoom;

    [SerializeField]
    private Button _openRoom;

    [SerializeField]
    private Button _startGame;

    [SerializeField]
    private TMP_Text _nameRoomText;

    [SerializeField]
    private TMP_Text _countPlayersRoom;

    [SerializeField]
    private TMP_Text _visibleText;

    [SerializeField]
    private TMP_Text _openText;

    private ConnectAndJoinRandomLobby _connectLobby;
    private void Awake()
    {
        _nameRoomText.text = $"Name room:";
        _countPlayersRoom.text = $"Players:";
        _visibleText.text = $"Visible room:";
        _openText.text = $"Open room:";
    }
    public void OpenRoom(ConnectAndJoinRandomLobby connectAndJoin)
    {
        _connectLobby = connectAndJoin;

        _nameRoomText.text = $"Name room: {_connectLobby.lbc.CurrentRoom.Name}";
        _countPlayersRoom.text = $"Players: {_connectLobby.lbc.CurrentRoom.PlayerCount}/{_connectLobby.lbc.CurrentRoom.MaxPlayers}";
        _visibleText.text = $"Visible room: {_connectLobby.lbc.CurrentRoom.IsVisible}";
        _openText.text = $"Open room: {_connectLobby.lbc.CurrentRoom.IsOpen}";

        _visibleRoom.onClick.AddListener(SwitchVisibleRoom);
        _openRoom.onClick.AddListener(SwitchOpenRoom);
        _startGame.onClick.AddListener(StartGame);

        Debug.Log($"open room finish");
    }

    private void StartGame()
    {
        if (_connectLobby.lbc.CurrentRoom.IsOpen)
        {
            _connectLobby.lbc.CurrentRoom.IsOpen = false;
        }

        Debug.Log("Start Game");
        _connectLobby.lbc.OpLeaveRoom(_connectLobby.lbc.CurrentRoom.IsOpen);
    }

    private void SwitchOpenRoom()
    {
        Debug.Log($"{_connectLobby.lbc.CurrentRoom.IsOpen}");
        if (_connectLobby.lbc.CurrentRoom.IsOpen)
        {
            var buttonColor = _visibleRoom.colors;
            buttonColor.normalColor = Color.red;
            _openRoom.colors = buttonColor;

            _connectLobby.lbc.CurrentRoom.IsOpen = false;
        }
        else
        {
            var buttonColor = _visibleRoom.colors;
            buttonColor.normalColor = Color.yellow;
            _openRoom.colors = buttonColor;

            _connectLobby.lbc.CurrentRoom.IsOpen = true;
        }

        _openText.text = $"Open room: {_connectLobby.lbc.CurrentRoom.IsOpen}";
    }

    private void SwitchVisibleRoom()
    {
        if (_connectLobby.lbc.CurrentRoom.IsVisible)
        {
            var buttonColor = _visibleRoom.colors;
            buttonColor.normalColor = Color.red;
            _visibleRoom.colors = buttonColor;

            _connectLobby.lbc.CurrentRoom.IsVisible = false;
        }
        else
        {
            var buttonColor = _visibleRoom.colors;
            buttonColor.normalColor = Color.yellow;
            _visibleRoom.colors = buttonColor;

            _connectLobby.lbc.CurrentRoom.IsVisible = true;
        }

        _visibleText.text = $"Visible room: {_connectLobby.lbc.CurrentRoom.IsVisible}";
    }
    public void LeaveRoom()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        _visibleRoom.onClick.RemoveAllListeners();
        _openRoom.onClick.RemoveAllListeners();
        _startGame.onClick.RemoveAllListeners();
    }
}
