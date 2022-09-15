using FantasyBattle.Spells;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Play
{
    public abstract class FireAction : MonoBehaviourPun, IPunObservable
    {
        [SerializeField]
        private SpellConteiner _spellConteiner;

        [SerializeField]
        private Transform _castPoint;

        public SpellConteiner spellConteiner { get; set; }
        public Transform CastPoint { get => _castPoint; set =>_castPoint = value; }

        public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

        public abstract void Shooting();
        protected virtual void Start()
        {

        }
    }
}
