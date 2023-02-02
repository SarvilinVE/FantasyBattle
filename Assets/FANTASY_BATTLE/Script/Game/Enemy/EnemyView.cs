using ExitGames.Client.Photon;
using FantasyBattle.Abstractions;
using FantasyBattle.Enemy;
using FantasyBattle.Enums;
using FantasyBattle.Spells;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Networking.UnityWebRequest;
using static UnityEngine.UI.CanvasScaler;
using static UnityEngine.UI.GridLayoutGroup;
using Color = UnityEngine.Color;
using Hashtable = ExitGames.Client.Photon.Hashtable;
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
        [SerializeField]
        private int _damage;
        [SerializeField]
        private float _attackSpeed;
        [Space]
        [Header("NavMesh Settings")]
        [SerializeField]
        private float _delayToMove;
        [SerializeField]
        private Vector2 _genericPointsRange;
        [SerializeField]
        private float _durationMove;
        [SerializeField]
        private float _viewRadius;
        [SerializeField]
        private float _attackRadius;

        //private PhotonView photonView;
        private Vector3 _botPostion;
        private Quaternion _botRotation;
        private Vector3 _currentVelocity = Vector3.zero;
        private NavMeshAgent _navMesh;
        private EnemyState _enemyState;
        private int _currentHp;
        private int _maxHp;
        private float _waitTimeToMove;
        private float _timerMove;
        private PlayerView _mainTarget;
        private bool _isPauseAttack = false;



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
        private void Awake()
        {
            //photonView = GetComponent<PhotonView>();
        }
        private void Start()
        {
            _navMesh = GetComponent<NavMeshAgent>();
        }
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


            _enemyState = EnemyState.Patroling;
            _waitTimeToMove = 0.0f;
            _timerMove = _durationMove;
            OnUpdateAction += Movement;
        }

        public void Movement()
        {
            //----Старая версия---

            //if (photonView.IsMine)
            //{
            //    transform.Translate(0, 0, _speed * Time.deltaTime);
            //    Ray ray = new Ray(transform.position, transform.forward);
            //    RaycastHit hit;
            //    if (Physics.SphereCast(ray, 0.75f, out hit))
            //    {
            //        var hitObject = hit.transform.gameObject;

            //        if (hitObject.TryGetComponent<IPlayer>(out var player))
            //        {
            //            _enemyState = EnemyState.Fight;
            //        }
            //        else if (hit.distance < _obstacleRange)
            //        {
            //            _enemyState = EnemyState.Patroling;
            //            float angle = Random.Range(-100, 100);
            //            transform.Rotate(0, angle, 0);
            //        }
            //    }
            //}
            //else
            //{
            //    //transform.position = Vector3.SmoothDamp(transform.position, _botPostion, ref _currentVelocity, _speed * Time.deltaTime);
            //    //transform.rotation = Quaternion.Lerp(transform.rotation, _botRotation, 0);
            //    //_currentHp = _correctionHp;
            //}

            if (photonView.IsMine)
            {
                switch (_enemyState)
                {
                    case EnemyState.Patroling:
                        {
                            SearchTarget();
                            Patroling();
                            break;
                        }
                    case EnemyState.Purshit:
                        {
                            SearchTarget();
                            Purshit();
                            break;
                        }
                    case EnemyState.Fight:
                        {
                            Attack();
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        private void Patroling()
        {
            if(_timerMove <= 0)
            {
                _navMesh.speed = _speed;
                _timerMove = _delayToMove;
                _navMesh.SetDestination(GetGenericPoint());
            }
            else
            {
                _timerMove -= Time.deltaTime;
            }

            if (_navMesh.remainingDistance <= _navMesh.stoppingDistance)
            {
                if (_waitTimeToMove <= 0)
                {
                    _navMesh.speed = _speed;
                    _waitTimeToMove = _delayToMove;
                    _timerMove = _delayToMove;
                    _navMesh.SetDestination(GetGenericPoint());
                }
                else
                {
                    _navMesh.speed = 0.0f;
                    _waitTimeToMove -= Time.deltaTime;
                }
            }
            else
            {
                ResetPosition();
                _timerMove = _delayToMove;
            }
        }
        private void SearchTarget()
        {
            Collider[] targetsInRange = Physics.OverlapSphere(transform.position,
                _viewRadius);
            //Debug.DrawLine(transform.position, targetsInRange[0].transform.position, Color.red);
            //if (!targetsInRange[0].gameObject.TryGetComponent<PlayerView>(out var player)) 
            //{
            //    _enemyState = EnemyState.Patroling;
            //    return;
            //}

            foreach(var target in targetsInRange)
            {
                Debug.DrawLine(transform.position, target.transform.position, Color.red);
                if (!target.gameObject.TryGetComponent<PlayerView>(out var playerView))
                {
                    continue;
                }

                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                float distanceToTarget = Vector3.Magnitude(target.transform.position - transform.position);

                bool angleCheck = Vector3.Angle(transform.forward, directionToTarget) < 180.0f / 2;
                if (angleCheck == false)
                {
                    continue;
                }

                _enemyState = EnemyState.Purshit;
                _mainTarget = target.gameObject.GetComponent<PlayerView>();
                _navMesh.SetDestination(target.transform.position);
            }
        }
        private void Purshit()
        {
            Debug.DrawLine(transform.position, _mainTarget.gameObject.transform.position, Color.blue, 2);
            if (_mainTarget == null)
            {
                _enemyState = EnemyState.Patroling;
                return;
            }

            if (_navMesh.remainingDistance - _navMesh.stoppingDistance <= _attackRadius)
            {
                _enemyState = EnemyState.Fight;
            }
        }
        private void Attack()
        {
            if(_mainTarget == null)
            {
                _enemyState = EnemyState.Purshit;
                return;
            }

            if (_isPauseAttack == false)
            {
                _mainTarget.gameObject.GetComponent<PhotonView>().RPC("DamageHp", RpcTarget.All, _damage);
                _isPauseAttack = true;
                StartCoroutine(DurationAttack());
            }

            _enemyState = EnemyState.Purshit;
        }

        public IEnumerator DurationAttack()
        {
            yield return new WaitForSeconds(_attackSpeed);
            _isPauseAttack = false;
        }
        private Vector3 GetGenericPoint()
        {
            Vector3 result;
            var dis = Random.Range(_genericPointsRange.x, _genericPointsRange.y);
            var randomPoint = Random.insideUnitSphere * dis;

            if(NavMesh.SamplePosition(transform.position + randomPoint, out var hit, dis, _navMesh.areaMask))
            {
                result = hit.position;
            }
            else
            {
                result = Vector3.zero;
            }

            Debug.DrawRay(result, Vector3.up, Color.blue, 2.0f);
            Debug.DrawLine(transform.position, result, Color.green, 2);
            return result;
        }
        private void ResetPosition()
        {
            if (_navMesh.isStopped)
            {
                _navMesh.speed = _speed;
                _navMesh.SetDestination(GetGenericPoint());
            }
        }
        public void TakeDamage(int damage, Player owner)
        {
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


        #region UnityMethods

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<FireballView>(out var fireBall))
            {
                if (photonView.IsMine)
                {


                    if (_currentHp - fireBall.DamageSpell > 0)
                    {
                        _currentHp -= fireBall.DamageSpell;

                        var playerDamage = Convert.ToInt32(fireBall.Owner.CustomProperties[LobbyStatus.CHARACTER_COUNTDAMAGE]);
                        playerDamage += fireBall.DamageSpell;

                        var hashTab = new Hashtable
                        {
                            { LobbyStatus.CHARACTER_COUNTDAMAGE, playerDamage.ToString() }
                        };
                        fireBall.Owner.SetCustomProperties(hashTab);
                    }
                    else
                    {
                        var playerKill = Convert.ToInt32(fireBall.Owner.CustomProperties[LobbyStatus.CHARACTER_KILLS]);
                        playerKill++;

                        var playerDamage = Convert.ToInt32(fireBall.Owner.CustomProperties[LobbyStatus.CHARACTER_COUNTDAMAGE]);
                        playerDamage += _currentHp;

                        var hashTab = new Hashtable
                        {
                            { LobbyStatus.CHARACTER_KILLS, playerKill.ToString() },
                            { LobbyStatus.CHARACTER_COUNTDAMAGE, playerDamage.ToString() }
                        };
                        fireBall.Owner.SetCustomProperties(hashTab);

                        _currentHp = 0;

                        var currentCountEnemies = Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties[LobbyStatus.CURRENT_COUNT_ENEMIES]) - 1;
                        var roomProps = new Hashtable
                        {
                            {LobbyStatus.CURRENT_COUNT_ENEMIES, currentCountEnemies }
                        };
                        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);

                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
        }

        #endregion


        #region IPunObservable

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                Debug.Log($"{photonView.ViewID} Writing HP {_currentHp}");

                //stream.SendNext(transform.position);
                //stream.SendNext(transform.rotation);
                stream.SendNext(_maxHp);
                stream.SendNext(_currentHp);
            }
            else
            {
                Debug.Log($"{photonView.ViewID} After Reading HP {_currentHp}");

                //_botPostion = (Vector3)stream.ReceiveNext();
                //_botRotation = (Quaternion)stream.ReceiveNext();
                _maxHp = (int)stream.ReceiveNext();
                _currentHp = (int)stream.ReceiveNext();

                Debug.Log($"{photonView.ViewID} Before Reading HP {_currentHp}");
            }
        }

        #endregion

    }
}
