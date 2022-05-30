using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MortarBullet : Bullet
{
    
    [SerializeField] private GameObject bullet;
    [SerializeField] private ParticleSystem shoot;
    [SerializeField] private ParticleSystem impact;
    [SerializeField] private List<Collider2D> targets;
    private Vector3 pos;
    private float duration;
    public override void Shoot(Tower origin, Enemy targetToSet, float speedToSet)
    {
        transform.position += Vector3.up;
        originTower = origin;
        SetTarget(targetToSet);
        speed = speedToSet;
        
        if (target == null) return;
        pos = target.transform.position;
        duration = (pos - transform.position).magnitude / speed;
        shoot.Play();
        impact.transform.position = pos;
        bullet.transform.DOJump(
                 pos,5, 1 , duration).SetEase(Ease.Linear)
             .OnComplete(Impact);
         
    }

    void Impact()
    {
        impact.Play();
        DoDamage();
        Pooler.instance.DelayedDepop(1.5f, "MortarBullet", gameObject);
    }
    private void DoDamage()
    {
        Debug.Log("target : " + targets.Count);
        /*Sound*/AudioManager.instance.Play(15, false);
        foreach (var enemy in targets)
        {
            enemy.GetComponentInParent<Enemy>().TakeDamage(originTower.towerSO.damageType,originTower.towerSO.damage,originTower);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (targets.Contains(other)) targets.Remove(other);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.transform.parent.CompareTag("Enemy")) return;
        targets.Add(other);
    }

    private void OnDisable()
    {
        impact.transform.localPosition = Vector3.zero;
        bullet.transform.position = Vector3.zero;
    }
}
