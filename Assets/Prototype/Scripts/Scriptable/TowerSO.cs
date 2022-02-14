using UnityEngine;

namespace Prototype.Scripts
{
    [CreateAssetMenu(menuName = "Create Tower")]
    public class TowerSO : ScriptableObject
    {
        public int maxHp;
        public int level;
        public float range;
        public float damage;
        public float energyCost;
        public float upgradeCost;
    }
}
