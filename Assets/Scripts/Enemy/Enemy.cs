using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public static event Action<int> EnemyDeathGoldEvent;
    public static event Action<int> EnemyDeathCristalEvent;
    [SerializeField] private AXD_EnemySO enemyStats;

    [SerializeField] private Animator animator;
    private Block initPos;
    private Block destination;
    private BargeSO bargeItComesFrom;
    private int cristalStored;


    private float speed;

    private AXD_TowerShoot target;

    private int currentHP { get; set; }
    private Coroutine movement;
    private void Start()
    {
        currentHP = enemyStats.maxHealthPoints;
        speed = enemyStats.speed;
        StartMovement();
    }

    public void StartMovement()
    {
        movement = StartCoroutine(MoveEnemy());
    }

    public void StopMovement()
    {
        StopCoroutine(movement);
    }

    public bool TakeDamage(DamageType damageType, int damageToTake)
    {
        currentHP -= damageToTake;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Death(); 
            return true;
        }
        //Debug.Log(currentHP);
        return false;
    }

    IEnumerator MoveEnemy()
    {
        animator.SetInteger("Speed", 1);
        destination = GameManager.instance.blocks[Utils.Round(new Vector2(1.78f, 0))];
        var des = Vector2.one * int.MaxValue;
        foreach (var pos in GameManager.instance.blocks.Values)
        {
            if ((transform.position - (Vector3) des).magnitude > (transform.position - pos.transform.position).magnitude)
            {
                des = pos.transform.position;
            }
        }

        CheckDirection(transform.position, des);
        transform.DOMove(des, ((Vector3) des - transform.position).magnitude / speed).SetEase(Ease.Linear);
        yield return new WaitForSeconds(((Vector3) des - transform.position).magnitude / speed);

        initPos = GameManager.instance.blocks[Utils.Round(transform.position)];
        Pathfinding pf = new Pathfinding();
        List<Vector2> map = new List<Vector2>();
        foreach (var block in GameManager.instance.blocks.Values)
        {
            map.Add(block.transform.position);
        }

        List<Pathfinding.Node> path = pf.FindPath(initPos.transform.position, destination.transform.position, map);
        Debug.Log(path);
        for (int i = 0; i < path.Count; i++)
        {
            CheckDirection(transform.position, path[i].pos);
            if (GameManager.instance.blocks[Utils.Round(path[i].pos)].building is not null && GameManager.instance.blocks[Utils.Round(path[i].pos)].building.buildingSO.type != BuildingType.Trap)
                yield return StartCoroutine(Attack(GameManager.instance.blocks[Utils.Round(path[i].pos)].building));
            transform.DOMove(path[i].pos, ((Vector3) path[i].pos - transform.position).magnitude / speed)
                .SetEase(Ease.Linear);
            yield return new WaitForSeconds(((Vector3) path[i].pos - transform.position).magnitude / speed);
        }
        
        animator.SetInteger("Speed", 0);
        GameManager.instance.enemies.Remove(this);
    }

    void CheckDirection(Vector2 pos, Vector2 des)
    {
        // -x / +y = N
        // +x / +y = E
        // +x / -y = S
        // -x / -y = W
        int signX = Math.Sign(des.x - pos.x);
        int signY = Math.Sign(des.y - pos.y);

        int dir = 0;
        if (signX < 0 && signY > 0) dir = 0;
        if (signX > 0 && signY > 0) dir = 1;
        if (signX > 0 && signY < 0) dir = 2;
        if (signX < 0 && signY < 0) dir = 3;

        //animator.SetInteger("Direction", dir);
        animator.SetFloat("Direction", dir);
    }

    IEnumerator Attack(Building target){
        
    
        var pos = target.transform.position - (target.transform.position - transform.position).normalized * 0.25f;
        transform.DOMove(pos, (pos - transform.position).magnitude / speed)
            .SetEase(Ease.Linear);
        yield return new WaitForSeconds((pos - transform.position).magnitude / speed);
        animator.SetInteger("Speed", 0);
        while (target.hp > 0)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(enemyStats.attackSpeed);
            target.takeDamage(enemyStats.damage);
        }
        animator.SetTrigger("AttackEnd");
        animator.SetInteger("Speed", 1);
    }
    
    IEnumerator Attack(GameObject target)
    {
        var pos = target.transform.position - (target.transform.position - transform.position).normalized * 0.25f;
        transform.DOMove(pos, (pos - transform.position).magnitude / speed)
            .SetEase(Ease.Linear);
        yield return new WaitForSeconds((pos - transform.position).magnitude / speed);
        animator.SetInteger("Speed", 0);
        while (GameManager.instance.cityCenter.health > 0)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(enemyStats.attackSpeed);
            GameManager.instance.cityCenter.TakeDamage(enemyStats.damage);
        }
        animator.SetTrigger("AttackEnd");
        animator.SetInteger("Speed", 1);
    }

    public void OnSpawn(BargeSO _barge, int troopListIndex)
    {
        bargeItComesFrom = _barge;
        cristalStored = _barge.troops[troopListIndex].cristalToEarn;
        GameManager.instance.enemies.Add(this);
    }

    public void Death()
    {
        if (EnemyDeathGoldEvent != null)
        {
            EnemyDeathGoldEvent((int) (enemyStats.goldToAddOnDeath * bargeItComesFrom.rewardModifier));
        }

        if (EnemyDeathCristalEvent != null && cristalStored > 0)
        {
            EnemyDeathCristalEvent((int)(cristalStored * bargeItComesFrom.rewardModifier));
        }
        GameManager.instance.enemies.Remove(this);
        Pooler.instance.Depop("Enemy", this.gameObject);
    }
    
}