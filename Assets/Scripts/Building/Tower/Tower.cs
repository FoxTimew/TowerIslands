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
    [SerializeField] private List<Enemy> inRange = new List<Enemy>();
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

        if (target is null) return;
        if (target.currentHP <= 0)
        {
            ResetTarget();
        }
        if (target is null) return;
        if (!shooting)
        {
            StartCoroutine(ShootCoroutine());
        }

        
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.CompareTag("Enemy"))
        {
            inRange.Add(other.GetComponentInParent<Enemy>());
            if (target == null) target = other.GetComponentInParent<Enemy>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.transform.parent.CompareTag("Enemy")) return;
        if (!inRange.Contains(other.GetComponentInParent<Enemy>())) return;
        inRange.Remove(other.GetComponentInParent<Enemy>());
        if (inRange.Count <= 0) return;
        target = inRange[0];
    }

    public override void Ruins()
    {
        ruins[Random.Range(0,2)].SetActive(true);
        level1.SetActive(false);
        level2.SetActive(false);
        destroyed = true;
        /*Sound*/ AudioManager.instance.Play(12, false);
    }



    public override void SetBuilding()
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

    public override void ResetTarget()
    {
        inRange.Remove(target);
        target = null;
        if (inRange.Count > 0) target = inRange[0];
    }
    public override void Repair()
    {
        destroyed = false;
        hp = buildingSO.healthPoints;
        /*Sound*/
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
        buildingSO = level1SO;
        Debug.Log(towerSO.level);
        attackSpeed = new WaitForSeconds(1/towerSO.attackSpeed);
        Repair();
        target = null;
    }

    public void Upgrade()
    {
        level1.SetActive(false);
        level2.SetActive(true);
        EconomyManager.instance.RemoveGold(towerSO.upgradeCost);
        towerSO = level1SO.nextLevel;
        buildingSO = level1SO.nextLevel;
        attackSpeed = new WaitForSeconds(1 / towerSO.attackSpeed);

    }
    
    private WaitForSeconds attackSpeed;
    IEnumerator ShootCoroutine()
    {
        if(target is null) yield break;
        shooting = true;
        GameObject go = Pooler.instance.Pop(towerSO.bulletPrefab.gameObject.name);
        go.transform.position = transform.position + Vector3.up;
        Pooler.instance.DelayedDepop(3,towerSO.bulletPrefab.name,go);
        /*Sound*/ AudioManager.instance.Play(towerSO.soundIndex, false);
        go.GetComponent<Bullet>().Shoot(this, target, towerSO.bulletSpeed);
        yield return attackSpeed;
        shooting = false;
    }
    
    
}
