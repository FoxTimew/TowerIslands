using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AXD_Bullet : MonoBehaviour
{
    private Enemy target;
    private Tower originTower;

    [SerializeField]
    private float speed { get; set; }

    private void Update()
    {
        if (target != null)
        {
            transform.DOMove(target.transform.position,(target.transform.position - transform.position).magnitude/speed , false );
        }
    }

    public void Shoot(Tower origin, Enemy targetToSet, float speedToSet)
    {
        originTower = origin;
        SetTarget(targetToSet);
        speed = speedToSet;
    }
    public void SetTarget(Enemy targetToSet)
    {
        target = targetToSet;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Pooler.instance.Depop("Bullet", gameObject);
            Enemy tmpEnemy = collider.gameObject.GetComponent<Enemy>();
            if (tmpEnemy.TakeDamage(originTower.towerSO.damageType,originTower.towerSO.damage,originTower))
            {
                originTower.target = null;
            }
        }
    }
}
