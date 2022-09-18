using System.Collections;
using System.Collections.Generic;
using FantasyBattle.Spells;
using Photon.Pun;
using UnityEngine;

namespace FantasyBattle.Play
{
    public class BotCharacter : Character
    {
        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _obstacleRange = 0.5f;

        [SerializeField]
        private GameObject _fireball;

        private PhotonView _photonView;
        public string Coven { get; set; }
        protected override FireAction FireAction { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        //protected override void Initiate()
        //{
        //    base.Initiate();
        //    this.gameObject.tag = Coven;
        //}
        public override void Movement()
        {

        }

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            transform.Translate(0, 0, _speed * Time.deltaTime);
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, 0.75f, out hit))
                {
                var hitObject = hit.transform.gameObject;
                
                if (hitObject.GetComponent<Character>())
                {
                    Fire(hitObject);
                }
                else if (hit.distance < _obstacleRange)
                {
                    float angle = Random.Range(-100, 100);
                    transform.Rotate(0, angle, 0);
                }
            }
        }

        public void Fire(GameObject hitObject)
        {
            var position = transform.TransformPoint(Vector3.forward * 1.5f);
            var rotation = transform.rotation;
            PhotonNetwork.Instantiate(_fireball.name, position, rotation).GetComponent<Fireball>().Init(hitObject.transform.position, 5);
        }
        private void Start()
        {
            //Initiate();
        }
    }
}
