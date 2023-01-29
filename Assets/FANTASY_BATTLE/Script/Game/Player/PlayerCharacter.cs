using Photon.Pun;
using UnityEngine;
using FantasyBattle.Classes;
using FantasyBattle.Spells;
using ExitGames.Client.Photon;
using FantasyBattle.UI;
using System;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace FantasyBattle.Play
{
    public sealed class PlayerCharacter : Character
    {
        #region Fields
        public event Action<bool> onFire;

        
        private SkillUI _skillUi;

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
        private int _playerLevel;

        public bool controllable = true;
        private bool _isRestoringMp = false;

        protected override FireAction FireAction { get; set; }

        #endregion


        #region Methods

        protected override void Initiate()
        {
            base.Initiate();

            _photonView = GetComponent<PhotonView>();

            gameObject.name = $"{photonView.ViewID} {gameObject.name}";
            //_slot = SlotUI.GetComponent<SlotUI>();

            _characterController = GetComponent<CharacterController>();
            _characterController ??= gameObject.AddComponent<CharacterController>();
            //_mouseLook = GetComponentInChildren<MouseLook>();
            _mouseLook = GetComponent<MouseLook>();
            _mouseLook ??= gameObject.AddComponent<MouseLook>();

            _playerClass = ClassType;

            if(photonView.IsMine)
            {
                _skillUi = FindObjectOfType<SkillUI>();

                _playerLevel = Convert.ToInt32(PhotonNetwork.LocalPlayer.CustomProperties[LobbyStatus.CHARACTER_LEVEL]);
                _skillUi.SetData(_playerClass.SpellClass, _playerLevel);
            }
        }

        public override void Movement()
        {
            //if (!this.photonView.AmOwner || !controllable)
            //{
            //    return;
            //}

            //if (this.photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
            //{
            //    return;
            //}

            if (_mouseLook != null && _mouseLook.PlayerCamera != null)
            {
                _mouseLook.PlayerCamera.enabled = this.photonView.IsMine;
            }

            if (this.photonView.IsMine)
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

                RestoreMana();

                if(int.TryParse(Input.inputString,out int numKey))
                {
                    _skillUi.ActiveSkill(Input.inputString, numKey);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    this.photonView.RPC("Fire", RpcTarget.AllViaServer, _castPoint.position, _castPoint.rotation);
                }
            }
        }

        [PunRPC]
        public void Fire(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
        {
            var spell = _skillUi.GetActiveSpell();

            if (_skillUi.IsBlockSkill)
            {
                return;
            }

            if (Mana - (int)spell.CostMP >= 0)
            {
                GameObject fireball;
                fireball = PhotonNetwork.InstantiateRoomObject(spell.SpellPrefab.name, position, Quaternion.identity);
                fireball.GetComponent<Fireball>().Init(_photonView.Owner, (rotation * Vector3.forward), spell.TimeLife, (int)(spell.AdditionalDamage + _playerClass.BaseDamage));
                Mana -= (int)spell.CostMP;
                UpdateUI();
                _skillUi.RollbackSkill();

            }
        }

        private void UpdateUI()
        {
            if (this.photonView.IsMine)
            {
                var hashTab = new Hashtable
                {
                    {LobbyStatus.CURRENT_HP, Health.ToString() },
                    {LobbyStatus.CURRENT_MP, Mana.ToString() },
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(hashTab);
            }
        }

        private void RestoreMana()
        {
            if (Mana < MaxMana)
            {
                if(_isRestoringMp == true)
                {
                    return;
                }
                StartCoroutine(RestoreMP());
            }
        }

        #endregion


        #region UnityMethods

        private void Start()
        {
            Initiate();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            PhotonNetwork.Destroy(gameObject);
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


        #region Corutines

        public IEnumerator RestoreMP()
        {
            _isRestoringMp = true;

            while (true)
            {
                if (Mana + ClassType.SpeedRestoreMp < MaxMana)
                {
                    Mana += (int)ClassType.SpeedRestoreMp;
                    UpdateUI();
                }
                else
                {
                    Mana = MaxMana;
                    _isRestoringMp = false;
                    UpdateUI();
                    yield break;
                }
                yield return new WaitForSeconds(1.0f);
            }
        }

        #endregion
    }
}
