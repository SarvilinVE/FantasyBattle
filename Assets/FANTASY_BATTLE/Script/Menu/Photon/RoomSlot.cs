using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSlot : MonoBehaviour
{
    [SerializeField]
    private Button _enterRoom;

    [SerializeField]
    private TMP_Text _enterRoomButtonText;

    [SerializeField]
    private TMP_Text _nameRoomText;

    private EnterRoomParams _roomParams;

    public Button enterRoom => _enterRoom;
    public TMP_Text enterRoomButtonText => _enterRoomButtonText;
    public TMP_Text nameRoomText => _nameRoomText;
    public EnterRoomParams RoomParams { get => _roomParams; set => _roomParams = value; }
}
