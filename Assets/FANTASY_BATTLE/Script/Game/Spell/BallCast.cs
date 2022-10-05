using FantasyBattle.Spells;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Play
{
    public sealed class BallCast : FireAction
    {
        private GameObject _spellPrefab;
        private Camera _camera;

        protected override void Start()
        {
            base.Start();

            _camera = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shooting();
            }

            if (Input.anyKey && !Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

        }

        public override void Shooting()
        {
            if (photonView.IsMine)
            {
                var point = new Vector3(_camera.pixelWidth / 2,
                    _camera.pixelHeight / 2, 0);

                var ray = _camera.ScreenPointToRay(point);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    _spellPrefab = PhotonNetwork.InstantiateRoomObject(spellConteiner.Spells[0].SpellPrefab.name,
                        transform.TransformPoint(Vector3.forward * 0.7f), transform.rotation);
                    //_spellPrefab.GetComponent<Fireball>().Init(hit.point, spellConteiner.Spells[0].TimeLife);
                    _spellPrefab.GetComponent<Fireball>().OnDestroyFireball += DestroySpell;
                }
            }
        }

        public void DestroySpell(bool isTarget)
        {
            if (isTarget)
            {
                PhotonNetwork.Destroy(_spellPrefab.gameObject);

            }
        }
        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

        }
    }
}
