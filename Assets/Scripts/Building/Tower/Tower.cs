using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PolygonCollider2D))]
public class Tower : Building
{
    public TowerSO towerSO;


    private TowerSO level1SO;

    private bool shooting;
    public Enemy target;
    private List<Enemy> inRange = new List<Enemy>();
    [SerializeField] private PolygonCollider2D pc;

    [SerializeField] private GameObject level1;
    [SerializeField] private GameObject level2;
    [SerializeField] private GameObject[] ruins;

    void Awake()
    {
        level1SO = towerSO;
        pc.points = Utils.UpdatePoints(towerSO.range);
        towerSO = (TowerSO) buildingSO;
        attackSpeed = new WaitForSeconds(1/towerSO.attackSpeed);
    }

    private void OnDisable()
    {
        Debug.Log("Reset");
        Reset();
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

    public override void Ruins()
    {
        ruins[Random.Range(0,2)].SetActive(true);
        level1.SetActive(false);
        level2.SetActive(false);
        destroyed = true;
    }

    public override void Repair()
    {
        destroyed = false;
        hp = buildingSO.healthPoints;
        foreach (var go in ruins) go.SetActive(false);
        switch (towerSO.level)
        {
            case 1 :
                level1.SetActive(true);
                level2.SetActive(false);
                
                break;
            case 2 :
                level2.SetActive(true);
                level1.SetActive(false);
                break;
        }
    }

    public override void Reset()
    {
        towerSO = level1SO;
        Debug.Log(towerSO.level);
        Repair();
    }

    public void Upgrade()
    {
        level1.SetActive(false);
        level2.SetActive(true);
        EconomyManager.instance.RemoveGold(towerSO.upgradeCost);
        towerSO = towerSO.nextLevel;
    }
    
    private WaitForSeconds attackSpeed;
    IEnumerator ShootCoroutine()
    {
        
        shooting = true;
        GameObject go = Pooler.instance.Pop(towerSO.bulletPrefab.gameObject.name);
        go.transform.position = transform.position + Vector3.up;
        Pooler.instance.DelayedDepop(3,towerSO.bulletPrefab.name,go);
        go.GetComponent<Bullet>().Shoot(this, target, towerSO.bulletSpeed);
        yield return attackSpeed;
        shooting = false;
    }
    
    
}
