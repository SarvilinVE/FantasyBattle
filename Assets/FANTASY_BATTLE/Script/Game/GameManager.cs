using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FantasyBattle.Play;

namespace FantasyBattle.Battle
{
    public class GameManager : MonoBehaviour
    {
        [Header("Result Holder")]
        [SerializeField]
        private GameObject _resultHolder;

        [Header("Game UI")]
        [SerializeField]
        private GameObject _gameUI;

        [Header("Play Manager")]
        [SerializeField]
        private PlayerManager _playerManager;

        private void Awake()
        {
            _playerManager.SetupPlayer();
            SetActivePanel(_resultHolder.name);
            StartCoroutine("ShowTabPlayers");
        }

        IEnumerator ShowTabPlayers()
        {
            yield return new WaitForSecondsRealtime(5.0f);
            SetActivePanel(_gameUI.name);
            //_playerManager.SetupPlayer();
        }

        private void SetActivePanel(string activePanel)
        {
            _resultHolder.SetActive(activePanel.Equals(_resultHolder.name));
            _gameUI.SetActive(activePanel.Equals(_gameUI.name));
        }
        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SetActivePanel(_resultHolder.name);
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                SetActivePanel(_gameUI.name);
            }
        }
    }
}
