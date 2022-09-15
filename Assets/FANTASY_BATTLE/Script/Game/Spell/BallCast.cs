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
                    _spellPrefab = Instantiate(spellConteiner.Spells[0].SpellPrefab);
                    _spellPrefab.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                    _spellPrefab.transform.rotation = transform.rotation;
                    Debug.Log("Hit " + hit.point);
                    _spellPrefab.GetComponent<Fireball>().Init(hit.point, spellConteiner.Spells[0].TimeLife);
                }
            }
        }

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //if (_spellPrefab != null)
            //{
            //    Vector3 pos = _spellPrefab.transform.position;
            //    Quaternion rot = _spellPrefab.transform.rotation;
            //    stream.Serialize(ref pos);
            //    stream.Serialize(ref rot);
            //    if (stream.IsReading)
            //    {
            //        transform.position = pos;
            //        transform.rotation = rot;
            //    }
            //}
        }
    }
}
