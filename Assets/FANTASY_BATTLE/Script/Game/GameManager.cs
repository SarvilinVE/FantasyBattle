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

        [Header("Play Manager")]
        [SerializeField]
        private PlayerManager _playerManager;

        private void Awake()
        {
            _resultHolder.SetActive(true);
            StartCoroutine("ShowTabPlayers");
        }

        IEnumerator ShowTabPlayers()
        {
            yield return new WaitForSecondsRealtime(5.0f);
            _resultHolder.SetActive(false);
            _playerManager.SetupPlayer();
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
