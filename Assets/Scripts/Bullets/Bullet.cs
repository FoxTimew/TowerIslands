using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Enemy target;
    protected Tower originTower;

    protected float speed { get; set; }
    
    
    public virtual void Shoot(Tower origin, Enemy targetToSet, float speedToSet)
    {
    }
    public void SetTarget(Enemy targetToSet)
    {
        target = targetToSet;
    }

}
