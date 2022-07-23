using UnityEngine;
using UnityEngine.UI;

public class EnterInGameWindow : MonoBehaviour
{
    [SerializeField]
    private Button _signInButton;

    [SerializeField]
    private Button _createAcoountButton;

    [SerializeField]
    private Canvas _enterInGameCanvas;

    [SerializeField]
    private Canvas _createAccountCanvas;

    [SerializeField]
    private Canvas _signInCanvas;

    private void Start()
    {
        _signInButton.onClick.AddListener(OpenSingInWindow);
        _createAcoountButton.onClick.AddListener(OpenCreateAccountWindow);
    }

    private void OnDestroy()
    {
        _signInButton.onClick.RemoveAllListeners();
        _createAcoountButton.onClick.RemoveAllListeners();
    }

    private void OpenSingInWindow()
    {
        _enterInGameCanvas.enabled = false;
        _signInCanvas.enabled = true;
    }

    private void OpenCreateAccountWindow()
    {
        _enterInGameCanvas.enabled = false;
        _createAccountCanvas.enabled = true;
    }
}
