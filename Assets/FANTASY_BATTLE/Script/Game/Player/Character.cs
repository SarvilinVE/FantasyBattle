using Photon.Pun;
using System;
using UnityEngine;

namespace FantasyBattle.Play
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class Character : MonoBehaviourPun, IPunObservable
    {
        protected Action OnUpdateAction { get; set; }

        [Range(0, 100)]
        [SerializeField]
        private int _health = 100;

        public int Health { get => _health; set => _health = value; }
        protected abstract FireAction FireAction { get; set; }

    protected virtual void Initiate()
        {
            OnUpdateAction += Movement;
        }

        private void OnUpdate()
        {
            OnUpdateAction?.Invoke();
        }
        public abstract void Movement();
        
        void Update()
        {
            OnUpdate();
            Debug.Log($"{this.name}  {_health}");
            if(_health < 60)
            {
                GetComponent<MeshRenderer>().material.color = Color.blue;
            }
        }

        public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

        private void OnDestroy()
        {
            OnUpdateAction -= Movement;
        }
    }
}
