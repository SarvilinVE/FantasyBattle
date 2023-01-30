using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Play
{
    public class PlayerView : MonoBehaviourPun, IPunObservable
    {
        #region Fields

        private const float GRAVITY = -9.8f;
        protected Action OnUpdateAction { get; set; }

        [Range(0.5f, 10.0f)]
        [SerializeField]
        private float _movingSpeed = 8.0f;

        [SerializeField]
        private float _acceleration = 3.0f;

        private MouseLook _mouseLook;
        private CharacterController _characterController;

        #endregion

        private void Start()
        {
            if (!photonView.IsMine) return;

            Initiate();
            OnUpdateAction += Movement;
        }
        public void Initiate()
        {
            if (!photonView.IsMine) return;

            OnUpdateAction += Movement;

            _mouseLook = GetComponent<MouseLook>();
            _characterController = GetComponent<CharacterController>();
        }
        private void OnUpdate()
        {
            if (!photonView.IsMine) return;

            OnUpdateAction?.Invoke();
        }
        public void Movement()
        {
            if (this.photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                _mouseLook.PlayerCamera.gameObject.name = "CameraEnable";
                return;
            }

            if (!photonView.IsMine) return;

            _mouseLook.PlayerCamera.enabled = true;
            

            var moveX = Input.GetAxis("Horizontal") * _movingSpeed;
            var moveZ = Input.GetAxis("Vertical") * _movingSpeed;
            var movement = new Vector3(moveX, 0, moveZ);
            movement = Vector3.ClampMagnitude(movement, _movingSpeed);
            movement *= Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movement *= _acceleration;
            }
            movement.y = GRAVITY;
            movement = transform.TransformDirection(movement);
            _characterController.Move(movement);
            _mouseLook.Rotation();
        }

        void Update()
        {
            if (!photonView.IsMine) return;

            OnUpdate();
        }



        #region IPunObservable

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        #endregion
    }
}
