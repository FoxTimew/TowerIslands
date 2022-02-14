using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Scripts
{
    public class Enemies : MonoBehaviour
    {
        public static Enemies instance;

        public List<Enemy> enemies = new List<Enemy>();

        private void Awake()
        {
            instance = this;
        }

        public void SetTarget(Enemy enemy, GameObject tower)
        {
            if (!enemies.Contains(enemy)) return;
            enemies.Find(e => e == enemy).target = tower;
            enemies.Find(e => e == enemy).MoveToTarget();
        }
    }
}