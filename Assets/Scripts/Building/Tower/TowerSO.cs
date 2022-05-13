using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TowerSO", menuName = "ScriptableObjects/TowerSO", order = 1)]
public class TowerSO : ScriptableObject
{
    public int damage;
    public float attackSpeed;

    public DamageType damageType;
    
    public float range;

    public int maxTargets;

    public TargetPriority priority;

    public float bulletSpeed;

    public Bullet bulletPrefab;
}
