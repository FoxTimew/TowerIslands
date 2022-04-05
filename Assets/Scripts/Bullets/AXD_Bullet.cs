using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AXD_Bullet : MonoBehaviour
{
    private Enemy target;
    private AXD_TowerShoot originTower;

    [SerializeField]
    private float speed { get; set; }

    private void Update()
    {
        if (target != null)
        {
            transform.DOMove(target.transform.position,(target.transform.position - transform.position).magnitude/speed , false );
        }
    }

    public void Shoot(AXD_TowerShoot origin, Enemy targetToSet, float speedToSet)
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
        Debug.Log("Bonk");
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Pooler.instance.Depop("Bullet", gameObject);
            Enemy tmpEnemy = collider.gameObject.GetComponent<Enemy>();
            if (tmpEnemy.TakeDamage(originTower.GetDamageType(),originTower.GetDamage()))
            {
                //Si l'enemy est détruit par le coup
                originTower.RemoveTargetFromTargets(tmpEnemy);
                originTower.RemoveTargetFromEnemiesWithinRange(tmpEnemy);
                //Pooler.instance.Depop("Enemy", tmpEnemy.gameObject);
            }
        }
    }
}
