using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Battle
{
    public class GameManager : MonoBehaviour
    {
        [Header("Result Holder")]
        [SerializeField]
        private GameObject _resultHolder;

        private void Awake()
        {
            _resultHolder.SetActive(true);
            StartCoroutine("ShowTabPlayers");
        }

        IEnumerator ShowTabPlayers()
        {
            yield return new WaitForSecondsRealtime(5.0f);
            _resultHolder.SetActive(false);
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
