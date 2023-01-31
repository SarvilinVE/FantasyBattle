using ExitGames.Client.Photon;
using FantasyBattle.Play;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
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
            var enemy = PhotonNetwork.Instantiate(enemyData.PrefabName, enemyData.StartPostion,
                enemyData.StratRotation, 0);
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
                var playerDamage = Convert.ToInt32(owner.CustomProperties[LobbyStatus.CHARACTER_COUNTDAMAGE]);
                playerDamage += damage;

                var hashTab = new Hashtable
                {
                    { LobbyStatus.CHARACTER_COUNTDAMAGE, playerDamage.ToString() }
                };
                owner.SetCustomProperties(hashTab);
            }
            else
            {
                var playerKill = Convert.ToInt32(owner.CustomProperties[LobbyStatus.CHARACTER_KILLS]);
                playerKill++;

                var playerDamage = Convert.ToInt32(owner.CustomProperties[LobbyStatus.CHARACTER_COUNTDAMAGE]);
                playerDamage += CurrentHp;

                var hashTab = new Hashtable
                {
                    { LobbyStatus.CHARACTER_KILLS, playerKill.ToString() },
                    { LobbyStatus.CHARACTER_COUNTDAMAGE, playerDamage.ToString() }
                };
                owner.SetCustomProperties(hashTab);

                CurrentHp = 0;
                _enemyView.CurrentHp = CurrentHp;

                //PhotonNetwork.Destroy(_enemyView.gameObject);
            }
        }

        public override void Movement()
        {
            _enemyView.Movement();
        }

        #endregion

    }
}
