using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Scripts
{
    [CreateAssetMenu(menuName = "Create Enemy")]
    public class EnemySO : ScriptableObject
    {
        public int maxHp;
        public float damage;
        public float speed;
        public float attackSpeed;
        public float moveSpeed;
        public float range;
    }
}
