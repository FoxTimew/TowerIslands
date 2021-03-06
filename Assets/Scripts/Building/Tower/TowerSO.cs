using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TowerSO", menuName = "ScriptableObjects/TowerSO", order = 1)]
public class TowerSO : BuildingSO
{

    public int level;
    public TowerSO nextLevel;
    public int upgradeCost;
        
    public int damage;
    public float attackSpeed;

    public DamageType damageType;
    
    public int range;

    public int maxTargets;

    public TargetPriority priority;

    public float bulletSpeed;

    public Bullet bulletPrefab;

    public int soundIndex;
}
