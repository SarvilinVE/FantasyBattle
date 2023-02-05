using FantasyBattle.Abstractions;
using FantasyBattle.Classes;
using FantasyBattle.Spells;
using FantasyBattle.UI;
using Photon.Pun;
using PlayFab.Internal;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace FantasyBattle.Play
{
    public class PlayerView : MonoBehaviourPun, IPunObservable, IPlayer
    {

        #region Fields

        private const float GRAVITY = -9.8f;
        protected Action OnUpdateAction { get; set; }

        [Header ("Class settings")]
        [SerializeField]
        private Class _classType;

        [SerializeField]
        private ClassConteiner _classConteiner;

        [Space]
        [Header ("Movement settings")]
        [Range(0.5f, 10.0f)]
        [SerializeField]
        private float _movingSpeed = 8.0f;

        [SerializeField]
        private float _acceleration = 3.0f;

        [SerializeField]
        private Transform _castPoint;

        [Space]
        [Header ("Visual Settings")]
        [SerializeField]
        private UnitInfoUI _unitInfoUi;

        private AudioSource _audioSource;

        private MouseLook _mouseLook;
        private CharacterController _characterController;

        private int _health;
        private int _currentHealth;
        private int _mana;
        private int _currentMana;
        private SkillUI _skillUi;
        private int _playerLevel;
        private bool _isRestoringMp = false;
        private string _playerName;
        private Renderer _renderer;
        private Collider _collider;
        private bool _isEnabale = true;
        private bool _isControllable;

        #endregion


        #region Properties IPlayer

        public string Name { get => _playerName; }
        public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
        public int Health { get => _health; set => _health = value; }
        public int CurrentMana { get => _currentMana; set => _currentMana = value; }
        public int Mana { get => _mana; set => _mana = value; }

        #endregion


        #region UnityMethods

        private void Start()
        {
            if (!photonView.IsMine) return;

            _isControllable= true;
            CreatePlayer();
            OnUpdateAction += Movement;
        }
        void Update()
        {
            if (!photonView.IsMine) return;
            if (!_isControllable) return;

            OnUpdate();
        }
        private void OnDestroy()
        {
            if (!photonView.IsMine) return;

            //_unitInfoUi.gameObject.SetActive(true);
            PhotonNetwork.Destroy(photonView.gameObject);
        }

        #endregion


        #region IPlayer

        public void CreatePlayer()
        {
            if (!photonView.IsMine) return;
            if (!_isControllable) return;

            _renderer = GetComponent<Renderer>();
            _collider = GetComponent<Collider>();
            _audioSource = GetComponent<AudioSource>();
            _isControllable = true;

            var parent = GameObject.Find("GameUI");
            _unitInfoUi = Instantiate(_unitInfoUi.gameObject, parent.transform).GetComponent<UnitInfoUI>();
            _unitInfoUi.gameObject.SetActive(false);

            _mouseLook = GetComponent<MouseLook>();
            _mouseLook.Initiation();
            _characterController = GetComponent<CharacterController>();

            _health = Convert.ToInt32(PhotonNetwork.LocalPlayer.CustomProperties[LobbyStatus.CHARACTER_HP]);
            _mana = Convert.ToInt32(PhotonNetwork.LocalPlayer.CustomProperties[LobbyStatus.CHARACTER_MP]);
            _currentHealth = _health;
            _currentMana = _mana;

            _skillUi = FindObjectOfType<SkillUI>();

            _playerLevel = Convert.ToInt32(PhotonNetwork.LocalPlayer.CustomProperties[LobbyStatus.CHARACTER_LEVEL]);
            _skillUi.SetData(_classType.SpellClass, _playerLevel);

            _playerName = PhotonNetwork.LocalPlayer.NickName;
            gameObject.name = $"{photonView.ViewID} {gameObject.name}";
        }
        public void Attack()
        {
            throw new NotImplementedException();
        }
        [PunRPC]
        public void DamageHp(int damage)
        {
            if (!_isControllable) return;

            if (_currentHealth - damage > 0)
            {
                _currentHealth -= damage;
            }
            else
            {
                _currentHealth = 0;

                _isControllable = false;
                _collider.enabled = false;
                _renderer.enabled = false;
            }

            UpdateUI();
        }

        public void DamageMp(int damage)
        {
            throw new NotImplementedException();
        }

        public void RestoringHp(int restoreHp)
        {
            throw new NotImplementedException();
        }

        public void RestoringMp(int restoreMp)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Methods

        private void OnUpdate()
        {
            if (!photonView.IsMine) return;

            OnUpdateAction?.Invoke();
        }
        public void Movement()
        {
            if (this.photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                _mouseLook.PlayerCamera.gameObject.name = "CameraEnable";
                return;
            }

            if (!photonView.IsMine) 
            {
                _collider.enabled = _isEnabale;
                _renderer.enabled = _isEnabale;
            }

            _mouseLook.PlayerCamera.enabled = true;
            

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
            SearchUnit();

            if (int.TryParse(Input.inputString, out int numKey))
            {
                _skillUi.ActiveSkill(Input.inputString, numKey);
            }

            if (Input.GetMouseButtonDown(0))
            {
                SpellCast();
            }
        }
        private void RestoreMana()
        {
            if (!photonView.IsMine) return;

            if (_currentMana < _mana)
            {
                if (_isRestoringMp == true)
                {
                    return;
                }
                StartCoroutine(RestoreMP());
            }
        }
        private void SearchUnit()
        {
            if (!photonView.IsMine) return;

            Ray ray = new Ray(_castPoint.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200.0f))
            {
                var hitObject = hit.transform.gameObject;

                if (hitObject.TryGetComponent<PlayerView>(out var player))
                {
                    Debug.Log($"SEE PLAYER {player.Name}  {player.CurrentHealth}HP   {player.CurrentMana}MP");
                    _unitInfoUi.SetData(player.Name, player.CurrentHealth, player.Health, true);
                }

                if (hitObject.TryGetComponent<EnemyView>(out var enemy))
                {
                    _unitInfoUi.SetData(enemy.name, enemy.CurrentHp, enemy.MaxHp, true);
                }
                else
                {
                    _unitInfoUi.gameObject.SetActive(false);
                }
            }
        }
        private void UpdateUI()
        {
            if (this.photonView.IsMine)
            {
                var hashTab = new Hashtable
                {
                    {LobbyStatus.CURRENT_HP, _currentHealth.ToString() },
                    {LobbyStatus.CURRENT_MP, _currentMana.ToString() },
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(hashTab);
            }
        }
        private void SpellCast()
        {
            var spell = _skillUi.GetActiveSpell();

            if (_skillUi.IsBlockSkill)
            {
                return;
            }

            if (_currentMana - (int)spell.CostMP >= 0)
            {
                _audioSource.clip = Resources.Load<AudioClip>($"Sound/{LobbyStatus.AVADA_KEDAVRA}");
                _audioSource.Play();

                photonView.RPC("FireBall", RpcTarget.AllViaServer, _castPoint.position, _castPoint.rotation, spell.NameSpell);

                _currentMana -= (int)spell.CostMP;
                UpdateUI();
                _skillUi.RollbackSkill();
            }
        }

        [PunRPC]
        public void FireBall(Vector3 position, Quaternion rotation, string spellName, PhotonMessageInfo info)
        {
            float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

                GameObject fireball;
                fireball = (GameObject)Instantiate(Resources.Load(spellName), position, Quaternion.identity);
                fireball.GetComponent<FireballView>().Init(photonView.Owner, (rotation * Vector3.forward), (int)_classType.BaseDamage, Mathf.Abs(lag));
        }
        //public void Fire(Vector3 position, Quaternion rotation)
        //{
        //    if (!photonView.IsMine) return;

        //    var spell = _skillUi.GetActiveSpell();

        //    if (_skillUi.IsBlockSkill)
        //    {
        //        return;
        //    }

        //    if (_currentMana - (int)spell.CostMP >= 0)
        //    {
        //        GameObject fireball;
        //        fireball = PhotonNetwork.Instantiate(spell.SpellPrefab.name, position, Quaternion.identity);
        //        fireball.GetComponent<Fireball>().Init(photonView.Owner, (rotation * Vector3.forward), spell.TimeLife, (int)(spell.AdditionalDamage + _classType.BaseDamage));
        //        _currentMana -= (int)spell.CostMP;
        //        UpdateUI();
        //        _skillUi.RollbackSkill();

        //    }
        //}

        #endregion


        #region Coroutines

        public IEnumerator RestoreMP()
        {
            _isRestoringMp = true;

            while (true)
            {
                if (_currentMana + _classType.SpeedRestoreMp < _mana)
                {
                    _currentMana += (int)_classType.SpeedRestoreMp;
                    UpdateUI();
                }
                else
                {
                    _currentMana = _mana;
                    _isRestoringMp = false;
                    UpdateUI();
                    yield break;
                }
                yield return new WaitForSeconds(1.0f);
            }
        }

        #endregion


        #region IPunObservable

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(_currentMana);
                stream.SendNext(_currentHealth);
                stream.SendNext(_health);
                stream.SendNext(_playerName);
                stream.SendNext(_isEnabale);
            }
            else
            {
                // Network player, receive data
                _currentMana = (int)stream.ReceiveNext();
                _currentHealth = (int)stream.ReceiveNext();
                _health = (int)stream.ReceiveNext();
                _playerName = (string)stream.ReceiveNext();
                _isEnabale = (bool)stream.ReceiveNext();
            }
        }

        

        #endregion

    }
}
