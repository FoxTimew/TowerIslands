using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MortarBullet : Bullet
{

    [SerializeField] private ParticleSystem shoot;
    [SerializeField] private ParticleSystem impact;
    public override void Shoot(Tower origin, Enemy targetToSet, float speedToSet)
    {
        originTower = origin;
        SetTarget(targetToSet);
        speed = speedToSet;
        if (target != null)
        {
            shoot.Play();
            transform.DOMove(target.transform.position,
                (target.transform.position - transform.position).magnitude / speed, false).SetEase(Ease.Linear).OnComplete(impact.Play);
        }
    }
}
