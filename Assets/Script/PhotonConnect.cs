using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonConnect : MonoBehaviourPunCallbacks
{
    #region Fields

    [SerializeField]
    private Button _photonButton;

    [SerializeField]
    private TMP_Text _photonStatus;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start()
    {
        _photonButton.onClick.AddListener(onConnect);
    }

    #endregion

    #region Methods

    private void onConnect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;

            _photonStatus.text = $"Photon connected";
            _photonStatus.color = Color.green;

            var colorButton = _photonButton.colors;
            colorButton.normalColor = Color.green;
            _photonButton.colors = colorButton;

            _photonButton.onClick.RemoveAllListeners();
            _photonButton.onClick.AddListener(OnDisconnect);
        }
    }

    private void OnDisconnect()
    {
        _photonButton.onClick.RemoveAllListeners();
        _photonButton.onClick.AddListener(onConnect);

        _photonStatus.text = $"Photon disconnected";
        _photonStatus.color = Color.red;

        var colorButton = _photonButton.colors;
        colorButton.normalColor = Color.red;
        _photonButton.colors = colorButton;

        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        Debug.Log($"Photon disconnect. Cause:  {cause}");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"On JoinedLobby {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinOrCreateRoom("roomName", new RoomOptions { MaxPlayers = 2, IsVisible = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"OnJoinedRoom {PhotonNetwork.InRoom}");
    }

    private void OnDestroy()
    {
        _photonButton.onClick.RemoveAllListeners();
    }

    #endregion

}
