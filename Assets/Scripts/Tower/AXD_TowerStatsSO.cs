using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerStatsSO", menuName = "ScriptableObjects/TowerStatsSO", order = 1)]

public class AXD_TowerStatsSO : AXD_BuildingSO
{

    public int damage;
    public float attackSpeed;

    public DamageType damageType;

    public float range;

    public int maxTargets;

    public TargetPriority priority;

    public float bulletSpeed;
    
}
