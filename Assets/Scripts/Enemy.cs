using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void Start()
    {
        transform.DOMove(Vector3.up, 10).SetEase(Ease.Linear);
        Pooler.instance.DelayedDepop(10,"Enemy",gameObject);
    }
}
