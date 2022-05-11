using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AXD_TowerShoot : Building
{
    [SerializeField] 
    public AXD_TowerStatsSO stats;
    [SerializeField]
    private List<Enemy> targets;
    [SerializeField]
    private List<Enemy> enemiesWithinRange;
    
    public delegate void ShootBullet();
    public ShootBullet shootBullet;
        
    
   
    private bool shooting;
    
    // Start is called before the first frame update
    void Start()
    {
        targets = new List<Enemy>();
        enemiesWithinRange = new List<Enemy>();
        hp = stats.healthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Count > 0 && !shooting)
        {
            StartCoroutine(ShootCoroutine());
        }
    }
    public void ChangeTargetToFirst()
    {
        if (enemiesWithinRange.Count != 0)
        {
            targets.Add(enemiesWithinRange[0]);
        }
    }

    
    
    public int GetDamage()
    {
        return stats.damage;
    }

    public DamageType GetDamageType()
    {
        return stats.damageType;
    }

    public void RemoveTargetFromTargets(Enemy enemy)
    {
        if (targets.Contains(enemy))
        {
            targets.Remove(enemy);
        }
    }
    public void RemoveTargetFromEnemiesWithinRange(Enemy enemy)
    {
        if (enemiesWithinRange.Contains(enemy))
        {
            enemiesWithinRange.Remove(enemy);
        }
    }
    
    public void SortEnemiesByPriority()
    {
        ChangeTargetToFirst();
        // TODO : sort by priority defined in the scriptable object
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesWithinRange.Add(other.GetComponent<Enemy>());
            if (targets.Count == 0)
                ChangeTargetToFirst();
            else
                SortEnemiesByPriority();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemiesWithinRange.Contains(other.GetComponent<Enemy>()))
        {
            enemiesWithinRange.Remove(other.GetComponent<Enemy>());
            targets.Clear();
            SortEnemiesByPriority();
        }
    }
    
    

    IEnumerator ShootCoroutine()
    {
        // shooting = true;
        // //Shoot
        // GameObject go = Pooler.instance.Pop("Bullet");
        // go.transform.position = transform.position + Vector3.up;
        // go.GetComponent<AXD_Bullet>().Shoot(this, targets[0], stats.bulletSpeed);
        // Pooler.instance.DelayedDepop(2,"Bullet",go);
        // //Instantiate(bulletPrefab, transform.position, Quaternion.identity).Shoot(this, targets[0], stats.bulletSpeed);
        // yield return new WaitForSeconds(1/stats.attackSpeed);
        //
        // shooting = false;
        yield return null;
    }
}
