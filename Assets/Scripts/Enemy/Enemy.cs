using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public static event Action<int> EnemyDeathGoldEvent;
    public static event Action<int> EnemyDeathCristalEvent;
    public AXD_EnemySO enemyStats;

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
        FindPath(GameManager.instance.grid.GetNearestBlock(transform.position));
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

        return false;
    }


    private Queue<Block> path = new Queue<Block>();
    void FindPath(Block initPos)
    {
        Pathfinding pf = new Pathfinding();
        var dist =float.MaxValue; 
        foreach (var i in GameManager.instance.grid.hdvIndex) 
            if (((Vector3)GameManager.instance.grid.GridElements[i.x,i.y].position - transform.position).magnitude <= dist)
                destination = GameManager.instance.grid.GridElements[i.x, i.y].block;
        
        List<Pathfinding.Node> pathfinding = pf.FindPath(initPos, destination, GameManager.instance.grid);
        for (int i = 0; i < pathfinding.Count; i++)
        {
            path.Enqueue(GameManager.instance.grid.GridElements[pathfinding[i].index.x, pathfinding[i].index.y].block);
        }
    }

    private Block block;
    public IEnumerator MoveEnemy()
    {
        Pathfinding pf = new Pathfinding();
        
        animator.SetInteger("Speed", 1);
        
        while (path.Count > 0)
        {
            
            block = path.Dequeue();
            CheckDirection(transform.position,block.transform.position);
            if (block.building is not null)
            {
                if( block.building.buildingSO.type != BuildingType.Trap)
                {
                    yield return StartCoroutine(Attack(block.building));
                }
            }
            transform.DOMove(block.transform.position, (block.transform.position - transform.position).magnitude / speed)
                .SetEase(Ease.Linear);
            yield return new WaitForSeconds((block.transform.position - transform.position).magnitude / speed);
        }
        
        animator.SetInteger("Speed", 0);
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

    IEnumerator Attack(Building target)
    {
        var pos = target.transform.position - (target.transform.position - transform.position).normalized * 2f;
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
    

    public void OnSpawn(BargeSO _barge, int troopListIndex)
    {
        bargeItComesFrom = _barge;
        cristalStored = _barge.troops[troopListIndex].cristalToEarn;
        //GameManager.instance.enemies.Add(this);
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
        //GameManager.instance.enemies.Remove(this);
        Pooler.instance.Depop(enemyStats.eName, this.gameObject);
    }
    
}