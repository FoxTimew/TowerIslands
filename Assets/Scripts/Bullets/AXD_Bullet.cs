using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AXD_Bullet : MonoBehaviour
{
    private GameObject target;
    private float speed;
    
    private void Update()
    {
        if (target != null)
        {
            transform.DOMove((target.transform.position - transform.position), speed*Time.deltaTime, false );
        }
    }

    public void SetTarget(GameObject targetToSet)
    {
        target = targetToSet;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            //Infliger des dégâts à l'ennemi
            //Remettre le projectile dans le pooler
            Destroy(this);
        }
    }
}
