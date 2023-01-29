using FantasyBattle.Enums;
using FantasyBattle.Spells;
using Photon.Pun;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FantasyBattle.Play
{
    public class EnemyView : MonoBehaviourPun, IPunObservable
    {

        #region Fields

        public event Action<int> OnTakeDamage;

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

        #endregion


        #region Properties

        public int CurrentHp
        {
            get { return _currentHp; }
            set { _currentHp = value; }
        }

        #endregion


        #region UnityMethods

        private void Update()
        {
            
        }
        private void OnDestroy()
        {
            PhotonNetwork.Destroy(photonView);
        }

        #endregion


        #region Methods

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

                    if (hitObject.GetComponent<PlayerCharacter>())
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
            }

            Debug.Log($"{gameObject.name} current HP {_currentHp}");
            
        }
        public void Fire(GameObject hitObject)
        {
            //var position = transform.TransformPoint(Vector3.forward * 1.5f);
            //var rotation = transform.rotation;
            //var fireball = Instantiate(_fireball, position, rotation);
            //fireball.GetComponent<Fireball>().Init(_photonView.Owner, hitObject.transform.position, 5);
        }

        public void TakeDamage(int takeDamage)
        {
            OnTakeDamage?.Invoke(takeDamage);
        }

        #endregion


        #region IPunObservable

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(_currentHp);
            }
            else
            {
                _botPostion = (Vector3)stream.ReceiveNext();
                _botRotation= (Quaternion)stream.ReceiveNext();
                _currentHp = (int)stream.ReceiveNext();
            }
        }

        #endregion

    }
}
