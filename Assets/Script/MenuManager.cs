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
    private Button _catalog;

    [SerializeField]
    private TMP_Text _catalogText;

    [SerializeField]
    private GameObject _userInfoPanel;

    [SerializeField]
    private GameObject _catalogPanel;

    void Start()
    {
        _userInfoPanel.SetActive(true);
        _catalogPanel.SetActive(false);

        _userInfoText.color = Color.green;
        _catalogText.color = Color.black;

        _userInfo.onClick.AddListener(ShowInfo);
        _catalog.onClick.AddListener(ShowCatalog);
    }

    private void ShowCatalog()
    {
        _userInfoPanel.SetActive(false);
        _catalogPanel.SetActive(true);

        _userInfoText.color = Color.black;
        _catalogText.color = Color.green;
    }

    private void ShowInfo()
    {
        _userInfoPanel.SetActive(true);
        _catalogPanel.SetActive(false);

        _userInfoText.color = Color.green;
        _catalogText.color = Color.black;
    }
}
