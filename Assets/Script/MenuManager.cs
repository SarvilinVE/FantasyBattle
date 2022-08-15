using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Button _userInfo;

    [SerializeField]
    private TMP_Text _userInfoText;

    [SerializeField]
    private Button _characterInfo;

    [SerializeField]
    private TMP_Text _characterInfoText;

    [SerializeField]
    private Button _catalog;

    [SerializeField]
    private TMP_Text _catalogText;

    [SerializeField]
    private Button _lobby;

    [SerializeField]
    private TMP_Text _lobbyText;

    [SerializeField]
    private GameObject _userInfoPanel;

    [SerializeField]
    private GameObject _characterInfoPanel;

    [SerializeField]
    private GameObject _catalogPanel;

    [SerializeField]
    private GameObject _lobbyPanel;

    void Start()
    {
        _userInfoPanel.SetActive(true);
        _characterInfoPanel.SetActive(false);
        _catalogPanel.SetActive(false);
        _lobbyPanel.SetActive(false);

        _userInfoText.color = Color.green;
        _characterInfoText.color = Color.black;
        _catalogText.color = Color.black;
        _lobbyText.color = Color.black;

        _userInfo.onClick.AddListener(ShowInfo);
        _characterInfo.onClick.AddListener(ShowCharacterInfo);
        _catalog.onClick.AddListener(ShowCatalog);
        _lobby.onClick.AddListener(ShowLobby);
    }

    private void ShowCharacterInfo()
    {
        _userInfoPanel.SetActive(false);
        _characterInfoPanel.SetActive(true);
        _catalogPanel.SetActive(false);
        _lobbyPanel.SetActive(false);

        _userInfoText.color = Color.black;
        _characterInfoText.color = Color.green;
        _catalogText.color = Color.black;
        _lobbyText.color = Color.black;
    }

    private void ShowLobby()
    {
        _userInfoPanel.SetActive(false);
        _characterInfoPanel.SetActive(false);
        _catalogPanel.SetActive(false);
        _lobbyPanel.SetActive(true);

        _userInfoText.color = Color.black;
        _characterInfoText.color = Color.black;
        _catalogText.color = Color.black;
        _lobbyText.color = Color.green;
    }

    private void ShowCatalog()
    {
        _userInfoPanel.SetActive(false);
        _characterInfoPanel.SetActive(false);
        _catalogPanel.SetActive(true);
        _lobbyPanel.SetActive(false);

        _userInfoText.color = Color.black;
        _characterInfoText.color = Color.black;
        _catalogText.color = Color.green;
        _lobbyText.color = Color.black;
    }

    private void ShowInfo()
    {
        _userInfoPanel.SetActive(true);
        _characterInfoPanel.SetActive(false);
        _catalogPanel.SetActive(false);
        _lobbyPanel.SetActive(false);

        _userInfoText.color = Color.green;
        _characterInfoText.color = Color.black;
        _catalogText.color = Color.black;
        _lobbyText.color = Color.black;
    }

    private void OnDestroy()
    {
        _userInfo.onClick.RemoveAllListeners();
        _characterInfo.onClick.RemoveAllListeners();
        _catalog.onClick.RemoveAllListeners();
        _lobby.onClick.RemoveAllListeners();
    }
}
