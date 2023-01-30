using FantasyBattle.Play;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

namespace FantasyBattle.Enemy
{
    public sealed class EnemyMag : EnemyMain
    {

        #region Fields

        private EnemyView _enemyView;

        #endregion


        #region CircleLifeMethods

        public EnemyMag(EnemyData enemyData) 
        {
            CreateEnemy(enemyData);
        }

        #endregion


        #region AbstractClassRealization

        public override void Attack()
        {
            throw new System.NotImplementedException();
        }

        public override void CreateEnemy(EnemyData enemyData)
        {
            var enemy = PhotonNetwork.InstantiateRoomObject(enemyData.PrefabName, enemyData.StartPostion,
                enemyData.StratRotation);
            _enemyView = enemy.GetComponent<EnemyView>();
            CurrentHp = enemyData.Hp;
            MaxHp = CurrentHp;

            _enemyView.CurrentHp = CurrentHp;
            _enemyView.MaxHp = MaxHp;
            _enemyView.OnTakeDamage += OnTakeDamage;
        }

        private void OnTakeDamage(int damage, Player owner)
        {
            if(CurrentHp - damage > 0)
            {
                CurrentHp -= damage;
                _enemyView.CurrentHp = CurrentHp;
                owner.AddScore(damage);
                Debug.Log($"{owner.ActorNumber} give {owner.GetScore()}");
            }
            else
            {
                CurrentHp = 0;
                _enemyView.CurrentHp = CurrentHp;

            }
        }

        public override void Movement()
        {
            _enemyView.Movement();
        }

        #endregion

    }
}
