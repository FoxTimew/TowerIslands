using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    [SerializeField] public TowerSO towerSO;

    private bool shooting;
    public Enemy target;
    private List<Enemy> inRange;
    

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
        //Shoot
        GameObject go = Pooler.instance.Pop("Bullet");
        go.transform.position = transform.position + Vector3.up;
        go.GetComponent<AXD_Bullet>().Shoot(this, target, towerSO.bulletSpeed);
        Pooler.instance.DelayedDepop(2,"Bullet",go);
        Instantiate(towerSO.bulletPrefab, transform.position, Quaternion.identity).Shoot(this, target, towerSO.bulletSpeed);
        yield return new WaitForSeconds(1/towerSO.attackSpeed);
        shooting = false;
    }
    
    
}
