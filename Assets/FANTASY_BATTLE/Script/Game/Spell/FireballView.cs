using Photon.Realtime;
using UnityEngine;

namespace FantasyBattle.Spells
{
    public class FireballView : MonoBehaviour
    {

        #region Fields

        [SerializeField] private float _speed;
        [SerializeField] private Spell _spellType;

        #endregion


        #region Properties

        public Player Owner { get; private set; }
        public int DamageSpell { get; private set; }

        #endregion


        #region Methods

        public void Init (Player owner, Vector3 targetPosition, int damage, float lag)
        {
            Owner = owner;
            DamageSpell = damage + (int)_spellType.AdditionalDamage;

            transform.forward = targetPosition;

            var rb = GetComponent<Rigidbody>();
            //rb.velocity = transform.forward * _speed;
            rb.velocity = targetPosition * _speed;
            rb.position += rb.velocity * lag;
        }

        #endregion


        #region UnityMethods

        private void Start()
        {
            Destroy(gameObject, _spellType.TimeLife);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }

        #endregion
    }
}
