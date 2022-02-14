
using System;
using UnityEditor;
using UnityEngine;

namespace Prototype.Scripts
{
    public class Tower : MonoBehaviour, IDamageable
    {
        [SerializeField] private TowerSO towerSo;
        void Init()
        {
            health = towerSo.maxHp;
        }

        private void OnEnable()
        {
            Init();
        }

        public float health { get; set; }

        private Enemy target;
        
        private void UpdateNearestEnemy()
        {
            Enemy nearestEnemy = null;
            var distance = Mathf.Infinity;
            foreach (var enemy in Enemies.instance.enemies)
            {
                var enemyDistance = (transform.position - enemy.transform.position).magnitude;
                
                if(enemyDistance<distance)
                    nearestEnemy = enemy;
            }

            if (distance < towerSo.range)
            {
                target = nearestEnemy;
                Enemies.instance.SetTarget(target,gameObject);
            }
                
        }

        private void Shoot()
        {
            if (target is not null)
            {
                target.Damage(towerSo.damage);
            }
        }
        
        public void Damage(float dmg)
        {
            health -= dmg;
        }
    }
}

