using FantasyBattle.Play;
using Photon.Pun;
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
            _enemyView.OnTakeDamage += OnTakeDamage;
        }

        private void OnTakeDamage(int damage)
        {
            if(CurrentHp - damage > 0)
            {
                CurrentHp -= damage;
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
