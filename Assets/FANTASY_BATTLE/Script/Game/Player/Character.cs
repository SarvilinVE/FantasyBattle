using FantasyBattle.Classes;
using Photon.Pun;
using System;
using UnityEngine;

namespace FantasyBattle.Play
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class Character : MonoBehaviourPun, IPunObservable
    {
        protected Action OnUpdateAction { get; set; }

        [SerializeField]
        private Class _classType;

        [SerializeField]
        private ClassConteiner _classConteiner;

        private int _health;
        private int _MaxHealth;
        private int _mana;
        private int _MaxMana;

        [Space]
        [Tooltip("CharacterUI")]
        [Header("CharacterUI")]
        [SerializeField]
        private GameObject _slotUIObject;
        private GameObject _slotUI;

        public Class ClassType { get => _classType; set => _classType = value; }

        public ClassConteiner ClassConteiner { get => _classConteiner; }
        public GameObject SlotUI { get => _slotUI; set => _slotUI = value; }
        public int Health { get => _health; set => _health = value; }
        public int MaxHealth { get => _MaxHealth; set => _MaxHealth = value; }
        public int Mana { get => _mana; set => _mana = value; }
        public int MaxMana { get => _MaxMana; set => _MaxMana = value; }

        protected virtual void Initiate()
        {
            if (photonView.IsMine)
            {
                OnUpdateAction += Movement;

                //_slotUI = Instantiate(_slotUIObject, FindObjectOfType<PlayerUI>().gameObject.transform);

                _health = Convert.ToInt32(PhotonNetwork.LocalPlayer.CustomProperties[LobbyStatus.CHARACTER_HP]);
                _mana = Convert.ToInt32(PhotonNetwork.LocalPlayer.CustomProperties[LobbyStatus.CHARACTER_MP]);

                _MaxHealth = _health;
                _MaxMana = _mana;

                //_slotUI.GetComponent<SlotUI>().InitSlot(PhotonNetwork.LocalPlayer.NickName, _classType._IconClass, Health, Mana);
            }
        }

        private void OnUpdate()
        {
            if (!photonView.IsMine) return;
            OnUpdateAction?.Invoke();
        }
        public abstract void Movement();

        void Update()
        {
            if (!photonView.IsMine) return;

            OnUpdate();
        }

        public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

        private void OnDestroy()
        {
            OnUpdateAction -= Movement;
        }
    }
}
