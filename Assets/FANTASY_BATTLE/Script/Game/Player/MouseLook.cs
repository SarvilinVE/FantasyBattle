using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyBattle.Play
{

    public class MouseLook : MonoBehaviour
    {

        #region Fields

        public Camera PlayerCamera => _camera;
        [Range(0.1f, 10.0f)]
        [SerializeField] private float _sensitivity = 2.0f;
        [Range(-90.0f, .0f)]
        [SerializeField] private float _minVert = -45.0f;
        [Range(0.0f, 90.0f)]
        [SerializeField] private float _maxVert = 45.0f;
        private float _rotationX = .0f;
        private float _rotationY = .0f;
        private Camera _camera;

        #endregion


        #region Methods

        public void Initiation()
        {
            Camera.main.gameObject.SetActive(false);
            _camera = GetComponentInChildren<Camera>();
            var listener= GetComponentInChildren<AudioListener>();
            listener.enabled = true;
            var rb = GetComponentInChildren<Rigidbody>();
            if (rb != null)
                rb.freezeRotation = true;
        }
        public void Rotation()
        {
            _rotationX -= Input.GetAxis("Mouse Y") * _sensitivity;
            _rotationY += Input.GetAxis("Mouse X") * _sensitivity;
            _rotationX = Mathf.Clamp(_rotationX, _minVert, _maxVert);
            transform.rotation = Quaternion.Euler(0, _rotationY, 0);
            _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        }

        #endregion

    }
}
