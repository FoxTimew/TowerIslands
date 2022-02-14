using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Prototype.Scripts
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemySO enemySO;

        public GameObject target;
        public float health { get; set; }

        private Tween tween;
        
        void Init()
        {
            tween = transform.DOMove(Vector3.zero, 12);
            health = enemySO.maxHp;
            Enemies.instance.enemies.Add(this);
            Move();
        }
        
        private void OnEnable()
        {
            Init();
        }

        public void Damage(float dmg)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            if(tween.active) tween.Kill();
            var distance = (transform.position - target.transform.position).magnitude;
            var time = distance / enemySO.moveSpeed;
            tween = transform.DOMove(target.transform.position,time).SetEase(Ease.Linear);
        }
    }
}

