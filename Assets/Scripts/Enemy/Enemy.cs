using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private AXD_EnemySO enemyStats;

    [SerializeField] private Animator animator;
    private Block initPos;
    private Block destination;
    private int currentHP { get; set; }

    private void Start()
    {
        currentHP = enemyStats.maxHealthPoints;
        StartCoroutine(MoveEnemy());
    }

    public bool TakeDamage(DamageType damageType,int damageToTake)
    {
        if (damageToTake >= currentHP)
        {
            currentHP = 0;
            Pooler.instance.Depop("Enemy", this.gameObject);
            return true;
        }
        currentHP -= damageToTake;
        return false;
    }
    
    IEnumerator MoveEnemy()
    {
        animator.SetInteger("Speed",1);
        destination = GameManager.instance.blocks[new Vector2(0, -0.25f)];
        var des = Vector2.one * int.MaxValue; 
        foreach (var pos in GameManager.instance.blocks.Keys)
        {
            if ((transform.position - (Vector3)des).magnitude > (transform.position - (Vector3)pos).magnitude)
            {
                des = pos;
            }
        }
        CheckDirection(transform.position,des);
        transform.DOMove(des, 2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2f);
        
        initPos = GameManager.instance.blocks[transform.position];
        Pathfinding pf = new Pathfinding();
        List<Vector2> map = new List<Vector2>();
        foreach (var block in GameManager.instance.blocks.Values)
        {
            map.Add(block.transform.position);
        }
        List<Pathfinding.Node> path = pf.FindPath(initPos.transform.position, destination.transform.position, map);

        
        for (int i = 0; i < path.Count; i++)
        {
            CheckDirection(transform.position,path[i].pos);
            transform.DOMove(path[i].pos, 0.5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);
        }
        animator.SetInteger("Speed",0);
        //transform.DOMove(destination.transform.position, 0.5f).SetEase(Ease.Linear);
        //yield return new WaitForSeconds(0.5f);
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
        
        animator.SetInteger("Direction", dir);

        /*if ((des-pos).normalized == Vector2.left + Vector2.up/2)
        {
            animator.SetInteger("Direction", 0);
            return;
        }
        if((des-pos).normalized == Vector2.right + Vector2.up/2)
        {
            animator.SetInteger("Direction", 1);
            return;
        }
        if((des-pos).normalized == Vector2.right + Vector2.down/2)
        {
            animator.SetInteger("Direction", 2);
            return;
        }
        if((des-pos).normalized == Vector2.left + Vector2.down/2)
        {
            animator.SetInteger("Direction", 3);
        }*/
    }
}
