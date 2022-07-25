using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{

    #region Fields

    [SerializeField]
    private GameObject _loadingScreen;

    [SerializeField]
    private GameObject _gamePanel;

    [SerializeField]
    private Slider _bar;

    #endregion

    #region UnityMethods

    void Start()
    {
        _loadingScreen.SetActive(true);
        _gamePanel.SetActive(false);
    }

    private void Update()
    {
        if (_bar.value <= 0.99f)
        {
            _bar.value += Time.deltaTime * 0.1f;
        }
        else
        {
            _loadingScreen.SetActive(false);
            _gamePanel.SetActive(true);
        }
    }

    #endregion

}
