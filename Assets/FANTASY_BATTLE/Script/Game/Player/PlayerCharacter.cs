using Photon.Pun;
using UnityEngine;
using FantasyBattle.Classes;

namespace FantasyBattle.Play
{
    public sealed class PlayerCharacter : Character
    {
        #region Fields

        //[Range(0, 100)] 
        //[SerializeField] 
        //private int _health = 100;

        [Range(0.5f, 10.0f)] 
        [SerializeField] 
        private float _movingSpeed = 8.0f;

        [SerializeField] 
        private float _acceleration = 3.0f;

        [SerializeField]
        private Transform _castPoint;

        [SerializeField]
        private Class _class;

        private const float GRAVITY = -9.8f;
        private CharacterController _characterController;
        private MouseLook _mouseLook;
        private Vector3 _currentVelocity;

        protected override FireAction FireAction { get; set; }

        #endregion

        #region Methods

        protected override void Initiate()
        {
            base.Initiate();

            FireAction = gameObject.AddComponent<BallCast>();
            FireAction.spellConteiner = _class.SpellClass;
            FireAction.CastPoint = _castPoint;

            _characterController = GetComponentInChildren<CharacterController>();
            _characterController ??= gameObject.AddComponent<CharacterController>();
            _mouseLook = GetComponentInChildren<MouseLook>();
            _mouseLook ??= gameObject.AddComponent<MouseLook>();
        }

        public override void Movement()
        {
            if (_mouseLook != null && _mouseLook.PlayerCamera != null)
            {
                _mouseLook.PlayerCamera.enabled = photonView.IsMine;
            }


            if (photonView.IsMine)
            {
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
        }

        #endregion

        #region UnityMethods

        private void Start()
        {
            Initiate();
        }

        #endregion

        #region IPunObservable realization
        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //Vector3 pos = transform.position;
            //Quaternion rot = transform.rotation;
            int health = Health;
            //stream.Serialize(ref pos);
            //stream.Serialize(ref rot);
            stream.Serialize(ref health);
            if (stream.IsReading)
            {
                //transform.position = pos;
                //transform.rotation = rot;
                Health = health;
            }
        }

        #endregion
    }
}
