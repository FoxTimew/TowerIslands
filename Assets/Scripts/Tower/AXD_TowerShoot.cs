using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_TowerShoot : MonoBehaviour
{
    [SerializeField]
    private AXD_TowerStatsSO stats;
    [SerializeField]
    private List<GameObject> targets;
    [SerializeField]
    private List<GameObject> enemiesWithinRange;

    [SerializeField] private AXD_Bullet bulletPrefab;

    private bool shooting;
    
    // Start is called before the first frame update
    void Start()
    {
        targets = new List<GameObject>();
        enemiesWithinRange = new List<GameObject>();
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
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " has entered the radius");
        if (other.CompareTag("Enemy"))
        {
            enemiesWithinRange.Add(other.gameObject);
            if (targets.Count == 0)
            {
                ChangeTargetToFirst();
            }
            else
            {
                SortEnemiesByPriority();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + " has left the radius");
        if (enemiesWithinRange.Contains(other.gameObject))
        {
            enemiesWithinRange.Remove(other.gameObject);
            targets.Clear();
            SortEnemiesByPriority();
            
        }
    }
    
    public void SortEnemiesByPriority()
    {
        ChangeTargetToFirst();
        // TODO : sort by priority defined in the scriptable object
    }

    IEnumerator ShootCoroutine()
    {
        shooting = true;
        //Shoot
        GameObject bulletTemp = Pooler.instance.Pop("Bullet");
        Pooler.instance.DelayedDepop( 1f,"Bullet", bulletTemp);
        Instantiate(bulletPrefab, transform.position, Quaternion.identity).Shoot(targets[0], stats.bulletSpeed);
        yield return new WaitForSeconds(1/stats.attackSpeed);
        
        shooting = false;
    }
}
