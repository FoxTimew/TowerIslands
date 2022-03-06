using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerStatsSO", menuName = "ScriptableObjects/TowerStatsSO", order = 1)]

public class AXD_TowerStatsSO : AXD_BuildingSO
{

    [field: SerializeField] public int damage;
    [field: SerializeField] public float attackSpeed;

    [field: SerializeField] public DamageType damageType;

    [field: SerializeField] public float range;

    [field: SerializeField] public int maxTargets;

    [field: SerializeField] public TargetPriority priority;

    [field: SerializeField] public float bulletSpeed;

}
