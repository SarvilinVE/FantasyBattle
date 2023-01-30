using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FantasyBattle.Play;
using Photon.Pun;
using Photon.Realtime;

namespace FantasyBattle.Spells
{
    public class Fireball : MonoBehaviour
    {
        protected Action OnUpdateAction { get; set; }
        public Action<bool> OnDestroyFireball { get; set; }

        [SerializeField]
        private float _speed;

        private Rigidbody _rb;
        private bool _isTarget = false;
        private float _timeLife;
        private int _damage;
        private Player _ownerPlayer;

        private void Start()
        {
            
        }
        public void Init(Player owner, Vector3 targetPosition, float timeLife, int damage)
        {
            //transform.LookAt(targetPosition);
            //OnUpdateAction += Move;
            _timeLife = timeLife;
            _ownerPlayer = owner;
            //StartCoroutine(LifeFireball());
            var info = new PhotonMessageInfo();
            float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

            transform.forward = targetPosition;

            _rb = GetComponent<Rigidbody>();
            _rb.velocity = transform.forward * _speed;
            //_rb.position += _rb.velocity * lag;
            //Destroy(gameObject, timeLife);
            StartCoroutine(LifeFireball());
            _damage = damage;

        }
        IEnumerator LifeFireball()
        {
            yield return new WaitForSeconds(_timeLife);
            PhotonNetwork.Destroy(gameObject);
        }
        //public void Move()
        //{
        //    var velocity = transform.forward * _speed;
        //    _rb.velocity = velocity;
        //}

        public void OnUpdate()
        {
            //OnUpdateAction?.Invoke();
        }

        private void FixedUpdate()
        {
            OnUpdate();
        }

        private void OnCollisionEnter(Collision collision)
        {

            //if(collision.gameObject.TryGetComponent<Character>(out var component))
            //{
            //    component.Health -= 10;
            //    _isTarget = true;
            //}
            //else
            //{
            //    _isTarget = true;
            //}

            //OnDestroyFireball?.Invoke(_isTarget);
            //Destroy(gameObject);
            if(collision.gameObject.TryGetComponent<EnemyView>(out var enemyView))
            {
                enemyView.TakeDamage(_damage, _ownerPlayer);
            }

            PhotonNetwork.Destroy(gameObject);
        }
    }
}
