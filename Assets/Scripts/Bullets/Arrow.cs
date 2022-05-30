using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Arrow : Bullet
{
    private Vector3 pos;
    public override void Shoot(Tower origin, Enemy targetToSet, float speedToSet)
    {
        //transform.GetChild(0).transform.localRotation = Quaternion.Euler(0,90,Random.Range(0f,360f));
        transform.position += Vector3.up;
        originTower = origin;
        SetTarget(targetToSet);
        pos = target.transform.position;
        transform.right = (pos - transform.position);
        pos -= (pos - transform.position) * 0.2f;
        
        speed = speedToSet;
        if (target != null)
        {
            transform.DOMove(pos,
                (pos - transform.position).magnitude / speed, false).SetEase(Ease.Linear)
                .OnComplete(Damaged);
        }
    }
    

    private void Damaged()
    {
        Pooler.instance.Depop("Arrow",gameObject);
        if (!target.isActiveAndEnabled) return;
        target.TakeDamage(originTower.towerSO.damageType,originTower.towerSO.damage,originTower);
    }
}
