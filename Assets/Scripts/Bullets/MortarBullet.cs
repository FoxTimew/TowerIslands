using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MortarBullet : Bullet
{

    [SerializeField] private ParticleSystem shoot;
    [SerializeField] private ParticleSystem impact;
    private Vector3 pos;
    private float duration;
    public override void Shoot(Tower origin, Enemy targetToSet, float speedToSet)
    {
        originTower = origin;
        SetTarget(targetToSet);
        speed = speedToSet;
        
        if (target == null) return;
        pos = target.transform.position;
        duration = (pos - transform.position).magnitude / speed;
        shoot.Play();
         transform.DOJump(
                 pos,5, 1 , duration).SetEase(Ease.Linear)
             .OnComplete(impact.Play);
         

    }
}
