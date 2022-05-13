using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    [SerializeField] public TowerSO towerSO;

    private bool shooting;
    public Enemy target;
    private List<Enemy> inRange = new List<Enemy>();


    void Update()
    {
        if (destroyed) return;
        if (target is null)
        {
            if (inRange.Count > 0)
                target = inRange[0];
        }
        else return;
        if (inRange.Count > 0 && !shooting)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            inRange.Add(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        if (inRange.Contains(other.GetComponent<Enemy>()))
        {
            inRange.Remove(other.GetComponent<Enemy>());
            if (inRange.Count <= 0) return;
            target = inRange[0];
        }
    }
    
    IEnumerator ShootCoroutine()
    {
        shooting = true;
        GameObject go = Pooler.instance.Pop(towerSO.bulletPrefab.name);
        go.transform.position = transform.position + Vector3.up;
        Pooler.instance.DelayedDepop(3,towerSO.bulletPrefab.name,go);
        go.GetComponent<Bullet>().Shoot(this, target, towerSO.bulletSpeed);
        yield return new WaitForSeconds(1/towerSO.attackSpeed);
        shooting = false;
    }
    
    
}
