using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AXD_Bullet : MonoBehaviour
{
    private GameObject target;

    [SerializeField]
    private float speed { get; set; }

    private void Update()
    {
        if (target != null)
        {
            transform.DOMove(target.transform.position,(target.transform.position - transform.position).magnitude/speed , false );
        }
    }

    public void Shoot(GameObject targetToSet, float speedToSet)
    {
        SetTarget(targetToSet);
        speed = speedToSet;
    }
    public void SetTarget(GameObject targetToSet)
    {
        target = targetToSet;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Pooler.instance.Depop("Bullet", gameObject);
            Destroy(this);
        }
    }
}
