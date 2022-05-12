using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RapidBullet : Bullet
{
    public override void Shoot(Tower origin, Enemy targetToSet, float speedToSet)
    {
        originTower = origin;
        SetTarget(targetToSet);
        speed = speedToSet;
        if (target != null)
        {
            transform.DOMove(target.transform.position,(target.transform.position - transform.position).magnitude/speed , false );
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Pooler.instance.Depop("Bullet", gameObject);
            Enemy tmpEnemy = collider.gameObject.GetComponent<Enemy>();
            if (tmpEnemy.TakeDamage(originTower.towerSO.damageType,originTower.towerSO.damage))
            {
                originTower.target = null;
            }
        }
    }
}
