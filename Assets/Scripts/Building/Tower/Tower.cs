using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    public TowerSO towerSO;

    private bool shooting;
    public Enemy target;
    private List<Enemy> inRange = new List<Enemy>();

    void Start()
    {
        towerSO = (TowerSO) buildingSO;
        attackSpeed = new WaitForSeconds(1/towerSO.attackSpeed);
    }
    void Update()
    {
        if (destroyed) return;
        if (inRange.Count > 0 && target is null)
            target = inRange[0];
        if (shooting) return;
        if (target is null) return;
        if (!target.gameObject.activeSelf) return;
        StartCoroutine(ShootCoroutine());
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

    private WaitForSeconds attackSpeed;
    IEnumerator ShootCoroutine()
    {
        shooting = true;
        GameObject go = Pooler.instance.Pop(towerSO.bulletPrefab.name);
        go.transform.position = transform.position + Vector3.up;
        Pooler.instance.DelayedDepop(3,towerSO.bulletPrefab.name,go);
        go.GetComponent<Bullet>().Shoot(this, target, towerSO.bulletSpeed);
        yield return attackSpeed;
        shooting = false;
    }
    
    
}
