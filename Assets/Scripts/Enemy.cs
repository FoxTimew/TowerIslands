using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private AXD_EnemySO enemyStats;

    private int currentHP { get; set; }

    private void Start()
    {
        currentHP = enemyStats.maxHealthPoints;
        transform.DOMove(Vector3.up, 10).SetEase(Ease.Linear);
        Pooler.instance.DelayedDepop(10,"Enemy",gameObject);
    }

    public bool TakeDamage(DamageType damageType,int damageToTake)
    {
        if (damageToTake >= currentHP)
        {
            currentHP = 0;
            Pooler.instance.Depop("Enemy", this.gameObject);
            return true;
        }
        else
        {
            currentHP -= damageToTake;
            return false;
        }
            
    }
}
