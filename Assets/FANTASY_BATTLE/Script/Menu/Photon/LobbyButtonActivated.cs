using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.Menu
{
    public class LobbyButtonActivated : MonoBehaviour
    {
        [SerializeField]
        private Button _thisButton;

        private void Awake()
        {
            _thisButton.enabled = false;
        }
        void Update()
        {
            //Debug.Log($"{PlayerPrefs.GetString(LobbyStatus.CHARACTER_ID)}");
            //if (PlayerPrefs.GetString(LobbyStatus.CHARACTER_ID) == LobbyStatus.EMPTY)
            //{
            //    _thisButton.enabled = false;
            //}
            //else
            //{
            //    _thisButton.enabled = true;
            //}
        }
    } 
}
