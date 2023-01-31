using ExitGames.Client.Photon;
using FantasyBattle.Abstractions;
using FantasyBattle.Enemy;
using FantasyBattle.Enums;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FantasyBattle.Play
{
    public class EnemyView : MonoBehaviourPun, IPunObservable
    {

        #region Fields

        public event Action<int, Player> OnTakeDamage;
        public event Action OnUpdateAction;
        public event Action<EnemyView> OnDiedEnemy;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _obstacleRange = 0.5f;

        [SerializeField]
        private GameObject _fireball;

        private PhotonView _photonView;
        public string Coven { get; set; }

        private Vector3 _botPostion;
        private Quaternion _botRotation;
        private Vector3 _currentVelocity = Vector3.zero;
        private EnemyState _enemyState;
        private int _currentHp;
        private int _correctionHp;
        private int _maxHp;
        private Player _player;

        #endregion


        #region Properties

        public int CurrentHp
        {
            get { return _currentHp; }
            set { _currentHp = value; }
        }
        public int MaxHp
        {
            get { return _maxHp; }
            set { _maxHp = value; }
        }

        #endregion


        #region UnityMethods

        private void Update()
        {
            OnUpdate();
        }
        private void OnDestroy()
        {
            //PhotonNetwork.Destroy(gameObject);
        }

        #endregion


        #region Methods

        public void Init(EnemyData enemyData)
        {
            _maxHp = enemyData.Hp;
            _currentHp = enemyData.Hp;

            OnUpdateAction += Movement;
        }

        public void Movement()
        {
            if (photonView.IsMine)
            {
                transform.Translate(0, 0, _speed * Time.deltaTime);
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                if (Physics.SphereCast(ray, 0.75f, out hit))
                {
                    var hitObject = hit.transform.gameObject;

                    if (hitObject.TryGetComponent<IPlayer>(out var player))
                    {
                        _enemyState = EnemyState.Fight;
                    }
                    else if (hit.distance < _obstacleRange)
                    {
                        _enemyState = EnemyState.Patroling;
                        float angle = Random.Range(-100, 100);
                        transform.Rotate(0, angle, 0);
                    }
                }
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, _botPostion, ref _currentVelocity, _speed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, _botRotation, 0);
                //_currentHp = _correctionHp;
            }
        }
        public void Fire(GameObject hitObject)
        {
            //var position = transform.TransformPoint(Vector3.forward * 1.5f);
            //var rotation = transform.rotation;
            //var fireball = Instantiate(_fireball, position, rotation);
            //fireball.GetComponent<Fireball>().Init(_photonView.Owner, hitObject.transform.position, 5);
        }

        public void TakeDamage(int damage, Player owner)
        {
            //if (!photonView.IsMine) return;

            //OnTakeDamage?.Invoke(takeDamage, owner);
            //_player = owner;

            if (_currentHp - damage > 0)
            {
                _currentHp -= damage;

                var playerDamage = Convert.ToInt32(owner.CustomProperties[LobbyStatus.CHARACTER_COUNTDAMAGE]);
                playerDamage += damage;

                var hashTab = new Hashtable
                {
                    { LobbyStatus.CHARACTER_COUNTDAMAGE, playerDamage.ToString() }
                };
                owner.SetCustomProperties(hashTab);
            }
            else
            {
                OnDiedEnemy?.Invoke(this);

                var playerKill = Convert.ToInt32(owner.CustomProperties[LobbyStatus.CHARACTER_KILLS]);
                playerKill++;

                var playerDamage = Convert.ToInt32(owner.CustomProperties[LobbyStatus.CHARACTER_COUNTDAMAGE]);
                playerDamage += _currentHp;

                var hashTab = new Hashtable
                {
                    { LobbyStatus.CHARACTER_KILLS, playerKill.ToString() },
                    { LobbyStatus.CHARACTER_COUNTDAMAGE, playerDamage.ToString() }
                };
                owner.SetCustomProperties(hashTab);

                _currentHp = 0;

                PhotonNetwork.Destroy(gameObject);
            }
        }

        public void OnUpdate()
        {
            OnUpdateAction?.Invoke();
        }

        #endregion


        #region IPunObservable

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(_maxHp);
                stream.SendNext(_currentHp);
            }
            else
            {
                _botPostion = (Vector3)stream.ReceiveNext();
                _botRotation= (Quaternion)stream.ReceiveNext();
                _maxHp = (int)stream.ReceiveNext();
                _currentHp = (int)stream.ReceiveNext();
            }
        }

        #endregion

    }
}
