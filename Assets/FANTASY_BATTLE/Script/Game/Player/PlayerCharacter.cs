using Photon.Pun;
using UnityEngine;
using FantasyBattle.Classes;
using FantasyBattle.Spells;

namespace FantasyBattle.Play
{
    public sealed class PlayerCharacter : Character
    {
        #region Fields

        //[Range(0, 100)] 
        //[SerializeField] 
        //private int _health = 100;

        [Range(0.5f, 10.0f)] 
        [SerializeField] 
        private float _movingSpeed = 8.0f;

        [SerializeField] 
        private float _acceleration = 3.0f;

        [SerializeField]
        private Transform _castPoint;

        private const float GRAVITY = -9.8f;
        private CharacterController _characterController;
        private MouseLook _mouseLook;
        private PhotonView _photonView;
        private SlotUI _slot;
        private Class _playerClass;

        public bool controllable = true;

        protected override FireAction FireAction { get; set; }

        #endregion

        #region Methods

        protected override void Initiate()
        {
            base.Initiate();

            //this.gameObject.tag = PhotonNetwork.LocalPlayer.CustomProperties[LobbyStatus.GROUP_COVEN].ToString();

            _photonView = GetComponent<PhotonView>();

            //FireAction = gameObject.AddComponent<BallCast>();
            //FireAction.spellConteiner = ClassType.SpellClass;
            //FireAction.CastPoint = _castPoint;

            _slot = SlotUI.GetComponent<SlotUI>();

            _characterController = GetComponentInChildren<CharacterController>();
            _characterController ??= gameObject.AddComponent<CharacterController>();
            _mouseLook = GetComponentInChildren<MouseLook>();
            _mouseLook ??= gameObject.AddComponent<MouseLook>();

            _playerClass = ClassType;
            Debug.Log($"Class: {_playerClass.UnitClass}");
            Debug.Log($"SpellClass: {_playerClass.SpellClass.name}");
            Debug.Log($"SpellName: {_playerClass.SpellClass.Spells[0].NameSpell}");
        }

        public override void Movement()
        {
            if (!_photonView.AmOwner || !controllable)
            {
                return;
            }

            if (_photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return;
            }

            if (_mouseLook != null && _mouseLook.PlayerCamera != null)
            {
                _mouseLook.PlayerCamera.enabled = _photonView.IsMine;
            }

            if (_photonView.IsMine)
            {

                var moveX = Input.GetAxis("Horizontal") * _movingSpeed;
                var moveZ = Input.GetAxis("Vertical") * _movingSpeed;
                var movement = new Vector3(moveX, 0, moveZ);
                movement = Vector3.ClampMagnitude(movement, _movingSpeed);
                movement *= Time.deltaTime;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    movement *= _acceleration;
                }
                movement.y = GRAVITY;
                movement = transform.TransformDirection(movement);
                _characterController.Move(movement);
                _mouseLook.Rotation();

                UpdateUI();

                if (Input.GetMouseButtonDown(0))
                {
                    //Fire();
                    _photonView.RPC("Fire", RpcTarget.AllViaServer, _castPoint.position, _castPoint.rotation);
                }
            }
        }

        [PunRPC]
        public void Fire(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
        {
            GameObject fireball;
            fireball = Instantiate(_playerClass.SpellClass.Spells[0].SpellPrefab, position, Quaternion.identity);
            fireball.GetComponent<Fireball>().Init(_photonView.Owner, (rotation * Vector3.forward), 5);
            Mana -= (int)_playerClass.SpellClass.Spells[0].CostMP;
        }

        private void UpdateUI()
        {
            if (_photonView.IsMine)
            {
                _slot.BarHP.value = Health;
                _slot.BarMP.value = Mana;
            }
        }

        private void RestoreMana()
        {

        }

        #endregion

        #region UnityMethods

        private void Start()
        {
            Initiate();
        }

        #endregion

        #region IPunObservable realization
        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //if (stream.IsWriting)
            //{
            //    // We own this player: send the others our data
            //    stream.SendNext(this.Health);
            //}
            //else
            //{
            //    // Network player, receive data
            //    this.Health = (int)stream.ReceiveNext();
            //}
        }

        #endregion
    }
}
