using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomSlot : MonoBehaviour
{
    [SerializeField]
    private Button _enterRoom;

    [SerializeField]
    private TMP_Text _enterRoomButtonText;

    [SerializeField]
    private TMP_Text _nameRoomText;

    public Button enterRoom => _enterRoom;
    public TMP_Text enterRoomButtonText => _enterRoomButtonText;
    public TMP_Text nameRoomText => _nameRoomText;
}
