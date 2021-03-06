using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    
    public static event Action<int> EnemyDeathGoldEvent;
    public AXD_EnemySO enemyStats;

    [SerializeField] public Animator animator;
    [SerializeField] private Collider2D col;
    private Block initPos;
    private Block destination;
    private BargeSO bargeItComesFrom;
    private int cristalStored;

    private bool stunned;

    private float speed;

    private AXD_TowerShoot target;

    public int currentDamage { get; set; }
    public int currentHP { get; private set; }
    private Coroutine movement;


    private Vector2 pos;
    private void Init()
    {
        isDying = false;
        pos.x = Random.Range(-0.5f, 0.5f);
        pos.y = Random.Range(-0.5f, 0.5f);
        sr.transform.localPosition = pos;
        col.offset = pos;
        sr.color = Color.white;
        currentHP = enemyStats.maxHealthPoints;
        currentDamage = enemyStats.damage;
        speed = enemyStats.speed;
        path.Clear();
        FindPath(GameManager.instance.grid.GetNearestBlock(transform.position));
        StartMovement();
    }


    public void StartMovement()
    {
        if(gameObject.activeSelf) movement = StartCoroutine(MoveEnemy());
    }

    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Block"))
        {
            if (other.GetComponent<Block>().building is null) return;
            if (other.GetComponent<Block>().building.buildingSO.type == BuildingType.Trap) return;
            StartCoroutine(Attack(other.GetComponent<Block>().building));
        }

        if (!other.GetComponent<Tower>()) return;
        if(other.GetComponent<Tower>().inRange.Contains(this)) return;
        other.GetComponent<Tower>().inRange.Add(this);


    }

    public bool rageAnim;
    public void StopMovementRageAnim()
    {
        rageAnim = true;
        StopMovement();
    }
    
    public void StopMovement()
    {
        if (movement is null) return;
        StopCoroutine(movement);
    }
    public void StopMovement(WaitForSeconds stunDuration)
    {
        if (movement is null) return;
        if (stunned) return;
        StartCoroutine(StopMovementDelayed(stunDuration));
    }
    

    private IEnumerator StopMovementDelayed(WaitForSeconds stunDuration)
    {
        StopCoroutine(movement);
        tween.Pause();
        animator.SetInteger("Speed", 0);
        stunned = true;
        yield return stunDuration;
        tween.Play().OnComplete(StartMovement);
        yield return stunDuration;
        stunned = false;
    }
    
    public bool TakeDamage(DamageType damageType, int damageToTake,Tower origin)
    {
        sr.DOColor(Color.HSVToRGB(1,0.5f,1), .1f).SetLoops(2,LoopType.Yoyo);
        currentHP -= damageToTake;
        if (currentHP > 0)
        {
            /*Sound*/ AudioManager.instance.Play(enemyStats.dammageSoundIndex[UnityEngine.Random.Range(0, enemyStats.dammageSoundIndex.Length)], false, true);
            return false;
        }
        if (isDying) return true;
        isDying = true;
        currentHP = 0;
        if( origin is not null) origin.ResetTarget();
        tween.Kill();
        StopAllCoroutines();
        Death(); 
        return true;
    }


    private Queue<Block> path = new Queue<Block>();
    //private List<Vector3> points;
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
            //points.Add(GameManager.instance.grid.GridElements[pathfinding[i].index.x, pathfinding[i].index.y].position);
        }
    }

    private Block block;
    private Tween tween;
    public IEnumerator MoveEnemy()
    {
        animator.SetInteger("Speed", 1);
        
        while (path.Count > 0)
        {
            block = path.Dequeue();
            CheckDirection(transform.position,block.transform.position);
            tween = transform.DOMove(block.transform.position,
                    (block.transform.position - transform.position).magnitude / speed)
                .SetEase(Ease.Linear);
            yield return new WaitForSeconds((block.transform.position - transform.position).magnitude / speed);
        }
        
        animator.SetInteger("Speed", 0);
    }

    void CheckDirection(Vector2 pos, Vector2 des)
    {

        int signX = Math.Sign(des.x - pos.x);
        int signY = Math.Sign(des.y - pos.y);

        int dir = 0;
        if (signX < 0 && signY > 0) dir = 0;
        if (signX > 0 && signY > 0) dir = 1;
        if (signX > 0 && signY < 0) dir = 2;
        if (signX < 0 && signY < 0) dir = 3;
        
        animator.SetFloat("Direction", dir);
    }

    IEnumerator Attack(Building target)
    {
        StopMovement();
        tween.Pause();
        animator.SetInteger("Speed", 0);
        while (target.hp > 0)
        {
            if (rageAnim) yield return null;
            animator.SetTrigger("Attack");
            /*Sound*/ AudioManager.instance.Play(enemyStats.attackSoundIndex[UnityEngine.Random.Range(0, enemyStats.attackSoundIndex.Length)], false, true);
            yield return new WaitForSeconds(1/enemyStats.attackSpeed);
            target.takeDamage(currentDamage);
        }
        animator.SetTrigger("AttackEnd");
        animator.SetInteger("Speed", 1);
        tween.Play().OnComplete(StartMovement);
        
    }
    

    public void OnSpawn(BargeSO _barge, int troopListIndex)
    {
        Init();
        bargeItComesFrom = _barge;
        cristalStored = _barge.troops[troopListIndex].cristalToEarn;
        /*Sound*/ AudioManager.instance.Play(enemyStats.spawnSoundIndex, false, true);
        //GameManager.instance.enemies.Add(this);
    }


    public bool isDying;
    [SerializeField] private SpriteRenderer sr;
    public void Death()
    {
        int rand = Random.Range(0, 3);
        /*SOund*/ AudioManager.instance.Play(enemyStats.dieSoundIndex[rand], false, true);
        if (EnemyDeathGoldEvent != null)
        {
            EnemyDeathGoldEvent((int) (enemyStats.goldToAddOnDeath * bargeItComesFrom.rewardModifier));
        }
        
        animator.SetTrigger("AttackEnd");
        animator.SetInteger("Speed", 0);
        animator.SetTrigger("Death");
        StartCoroutine(DeathDepop());
        Pooler.instance.DelayedDepop(0.8f, enemyStats.eName, this.gameObject);
    }

    private WaitForSeconds waitDepop = new WaitForSeconds(0.5f);

    IEnumerator DeathDepop()
    {
        yield return waitDepop;
        sr.DOFade(0, 0.3f);
        
        var go = Pooler.instance.Pop("GoldFx");
        go.transform.position = transform.position;
        Pooler.instance.DelayedDepop(0.8f,"GoldFx",go);
    }
}