using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Pathfinding : MonoBehaviour
{
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;
    
    private Queue<Block> finalPath = new Queue<Block>();

    [SerializeField] private Block initPos;
    [SerializeField] private Block destination;

    [SerializeField] private Block[] initBlocks;

    private Dictionary<Vector2, Block> blocks = new Dictionary<Vector2, Block>();

    [SerializeField] private GameObject enemy;

    private List<Path> openList;
    private List<Path> closedList = new List<Path>();
    
    public class Path
    {
        public Vector2 pos;

        public int gCost;
        public int hCost;
        public int fCost;

        public Path cameFrom;

        public Path(Vector2 pos)
        {
            this.pos = pos;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
        
        
    }
    void Start()
    {
        foreach (var block in initBlocks)
            blocks.Add(block.transform.position,block);
        foreach (var block in blocks.Values)
            FindAdjacents(block);

        

        /*initPos.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.transform.position = initPos.transform.position;
        FindNearestBlock((Vector2)initPos.transform.position);
        destination.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        StartCoroutine(MoveEnemy(finalPath.Count));*/
    }

    public List<Path> FindPath()
    {
        Path start = new Path(initPos.transform.position);
        Path end = new Path(destination.transform.position);
        openList = new List<Path>() { start };

        foreach (var block in blocks)
        {
            Path path = new Path(block.Key);
            path.gCost = int.MaxValue;
            path.CalculateFCost();
            path.cameFrom = null;
        }

        start.gCost = 0;
        start.hCost = CalculateDistance(start, end);
        start.CalculateFCost();

        while (openList.Count > 0)
        {
            Path current = GetLowestFCostNode(openList);
            if (current == end)
            {
                return CalculatePath(end);
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (var neighbour in GetNeighbourList(current))
            {
                if (closedList.Contains(neighbour)) continue;
                int tentativeGCost = current.gCost + CalculateDistance(current, neighbour);
                if (tentativeGCost < neighbour.gCost)
                {
                    neighbour.cameFrom = current;
                    neighbour.gCost = tentativeGCost;
                    neighbour.hCost = CalculateDistance(neighbour, end);
                    neighbour.CalculateFCost();
                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    private List<Path> GetNeighbourList(Path current)
    {
        List<Path> neighbourList = new List<Path>();
        foreach (var block in blocks[current.pos].adjacentBlocks)
        {
            neighbourList.Add(new Path(block.transform.position));
        }

        return neighbourList;
    }

    private List<Path> CalculatePath(Path end)
    {
        List<Path> path = new List<Path>();
        path.Add(end);
        Path current = end;
        while (current.cameFrom != null)
        {
            path.Add(current.cameFrom);
            current = current.cameFrom;
            
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistance(Path a, Path b)
    {
        int xDistance = (int)Mathf.Abs(a.pos.x - b.pos.x);
        int yDistance = (int)Mathf.Abs(a.pos.y - b.pos.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * remaining;
    }

    private Path GetLowestFCostNode(List<Path> pathList)
    {
        Path lowestFCostNode = pathList[0];
        for (int i = 1; i < pathList.Count; i++)
        {
            if (pathList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathList[i];
            }
        }

        return lowestFCostNode;
    }
    
    void FindNearestBlock(Vector2 pos)
    {
        if (pos == (Vector2)destination.transform.position) return;
        Vector2 nextPos = pos;
        foreach (var block in blocks[pos].adjacentBlocks)
        {
            if (((Vector2)block.transform.position - (Vector2)destination.transform.position).magnitude < (nextPos - (Vector2)destination.transform.position).magnitude)
            {
                if(CheckItslef(block))  
                    nextPos = block.transform.position;
                
            }
        }

        if (nextPos == pos) return;
        blocks[nextPos].gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        finalPath.Enqueue(blocks[nextPos]);
        FindNearestBlock(nextPos);
        
    }

    bool CheckItslef(Block block)
    {
        Vector2 nextPos = block.transform.position;
        foreach (var b in block.adjacentBlocks)
        {
            if (((Vector2)b.transform.position - (Vector2)destination.transform.position).magnitude < (nextPos - (Vector2)destination.transform.position).magnitude)
            {
                nextPos = b.transform.position;
            }
        }
        if (nextPos == (Vector2)block.transform.position)
            return false;
        return true;
    }
    public Vector2[] InitAdjacents(Vector2 pos)
    {
        return new[]
        {
            new Vector2(pos.x+1,pos.y),
            new Vector2(pos.x-1,pos.y),
            new Vector2(pos.x,pos.y+1),
            new Vector2(pos.x,pos.y-1),
            new Vector2(pos.x+1,pos.y+1),
            new Vector2(pos.x-1,pos.y-1),
            new Vector2(pos.x-1,pos.y+1),
            new Vector2(pos.x+1,pos.y-1),
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
        enemy.transform.DOMove(destination.transform.position, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.5f);
    }
}
