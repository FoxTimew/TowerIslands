using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Queue<Block> finalPath = new Queue<Block>();

    [SerializeField] private Block initPos;
    [SerializeField] private Block destination;

    [SerializeField] private Block[] initBlocks;

    private Dictionary<Vector2, Block> blocks = new Dictionary<Vector2, Block>();

    [SerializeField] private GameObject enemy;

    void Start()
    {
        foreach (var block in initBlocks)
            blocks.Add(block.transform.position,block);
        foreach (var block in blocks.Values)
            FindAdjacents(block);
        
        initPos.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.transform.position = initPos.transform.position;
        FindNearestBlock((Vector2)initPos.transform.position);
        destination.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        StartCoroutine(MoveEnemy(finalPath.Count));
    }

    void FindNearestBlock(Vector2 pos)
    {
        if (pos == (Vector2)destination.transform.position) return;
        Vector2 nextPos = pos;
        foreach (var block in blocks[pos].adjacentBlocks)
        {
            if (((Vector2)block.transform.position - (Vector2)destination.transform.position).magnitude < (nextPos - (Vector2)destination.transform.position).magnitude)
            {
                nextPos = block.transform.position;
            }
        }
        blocks[nextPos].gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        finalPath.Enqueue(blocks[nextPos]);
        FindNearestBlock(nextPos);
        
    }
    
    public Vector2[] InitAdjacents(Vector2 pos)
    {
        return new[]
        {
            new Vector2(pos.x+1,pos.y),
            new Vector2(pos.x-1,pos.y),
            new Vector2(pos.x,pos.y+1),
            new Vector2(pos.x,pos.y-1),
        };
    }
    public void FindAdjacents(Block block)
    {
        foreach (Vector3 adj in InitAdjacents(block.transform.position))
        {
            if (!blocks.ContainsKey(adj)) continue;
            block.adjacentBlocks.Add(blocks[adj]);
        }
    }


    IEnumerator MoveEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Debug.Log(finalPath.Peek().transform.position);
            enemy.transform.DOMove(finalPath.Dequeue().transform.position, 0.5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
