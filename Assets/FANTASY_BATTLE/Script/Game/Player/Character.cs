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

        [Range(0, 200)]
        [SerializeField]
        private int _health = 200;

        [Range(0, 200)]
        [SerializeField]
        private int _mana = 200;

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
        public int Mana { get => _mana; set => _mana = value; }
        protected abstract FireAction FireAction { get; set; }

    protected virtual void Initiate()
        {
            OnUpdateAction += Movement;

            _slotUI = Instantiate(_slotUIObject, FindObjectOfType<PlayerUI>().gameObject.transform);
            _slotUI.GetComponent<SlotUI>().InitSlot(PhotonNetwork.LocalPlayer.NickName, _classType._IconClass, Health, Mana);
        }

        private void OnUpdate()
        {
            OnUpdateAction?.Invoke();
        }
        public abstract void Movement();
        
        void Update()
        {
            OnUpdate();
        }

        public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

        private void OnDestroy()
        {
            OnUpdateAction -= Movement;
        }
    }
}
