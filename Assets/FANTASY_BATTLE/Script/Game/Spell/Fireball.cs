using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FantasyBattle.Play;

namespace FantasyBattle.Spells
{
    public class Fireball : MonoBehaviour
    {
        protected Action OnUpdateAction { get; set; }

        [SerializeField]
        private float _speed;

        private Rigidbody _rb;
        private float _timeLife;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }
        public void Init(Vector3 targetPosition, float timeLife)
        {
            _timeLife = timeLife;
            transform.LookAt(targetPosition);
            OnUpdateAction += Move;
            Destroy(gameObject, timeLife);
            
        }
        public void Move()
        {
            var velocity = transform.forward * _speed;
            velocity.y = _rb.velocity.y;
            _rb.velocity = velocity;
            Debug.Log($"Tut");
        }

        public void OnUpdate()
        {
            OnUpdateAction?.Invoke();
        }

        private void FixedUpdate()
        {
            OnUpdate();
        }

        private void OnCollisionEnter(Collision collision)
        {
            
            if(collision.gameObject.TryGetComponent<Character>(out var component))
            {
                component.Health -= 10;
            }

            Destroy(gameObject);
        }
    }
}
