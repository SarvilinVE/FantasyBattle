using Photon.Pun;
using System;
using UnityEngine;

namespace FantasyBattle.Play
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class Character : MonoBehaviourPun, IPunObservable
    {
        protected Action OnUpdateAction { get; set; }

        //protected abstract FireAction {get; set;}

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
        }

        public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

        private void OnDestroy()
        {
            OnUpdateAction -= Movement;
        }
    }
}
