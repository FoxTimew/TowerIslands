using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Pathfinding
{
    public static Pathfinding instance;

    //private Queue<Block> finalPath = new Queue<Block>();

    private List<Node> toCheckList = new List<Node>();
    private List<Node> checkedList = new List<Node>();

    private Dictionary<Vector2, Node> grid = new Dictionary<Vector2, Node>();


    public Pathfinding()
    {
        instance = this;
    }
    public class Node
    {
        public Vector2 pos;

        public float gCost;
        public float hCost;
        public float fCost;

        public Node cameFrom;

        public Node(Vector2 pos)
        {
            this.pos = pos;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }

    public List<Node> FindPath(Vector2 start, Vector2 end, List<Vector2> map)
    {
        

        foreach (var pos in map)
        {
            Node node = new Node(pos);
            node.gCost = int.MaxValue;
            node.CalculateFCost();
            node.cameFrom = null;
            grid.Add(pos,node);
        }
        toCheckList.Add(grid[start]);
        grid[start].gCost = 0;
        grid[start].hCost = CalculateDistance(grid[start], grid[end]);
        grid[start].CalculateFCost();
        while (toCheckList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(toCheckList);
            if (currentNode == grid[end])
                return CalculatePath(grid[end]);
            toCheckList.Remove(currentNode);
            checkedList.Add(currentNode);
            foreach (var neighbour in NeighbourList(currentNode))
            {
                if (checkedList.Contains(grid[neighbour])) continue;
                float tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, grid[neighbour]);
                if (tentativeGCost < grid[neighbour].gCost)
                {
                    grid[neighbour].cameFrom = grid[currentNode.pos];
                    grid[neighbour].gCost = tentativeGCost;
                    grid[neighbour].hCost = CalculateDistance(grid[neighbour], grid[end]);
                    grid[neighbour].CalculateFCost();
                    if (!toCheckList.Contains(grid[neighbour]))
                         toCheckList.Add(grid[neighbour]);
                }
            }
        }
        return null;
    }

    private List<Vector2> NeighbourList(Node current)
    {
        List<Vector2> result = new List<Vector2>();
        Vector2[] neigbourPos = new[]
        {
            current.pos + new Vector2(0.5f, 0.25f),
            current.pos + new Vector2(0.5f, -0.25f),
            current.pos + new Vector2(-0.5f, -0.25f),
            current.pos + new Vector2(-0.5f, 0.25f),
            
            
        };
        foreach (var pos in neigbourPos)
        {
            if (!grid.ContainsKey(pos)) continue;
            result.Add(pos);
        }
        return result;
    }

    private float CalculateDistance(Node a, Node b)
    {
        float xDistance = Mathf.Abs(a.pos.x - b.pos.x);
        float yDistance = Mathf.Abs(a.pos.y - b.pos.y);
        float remaining = Mathf.Abs(xDistance - yDistance);
        return 14 * Mathf.Min(xDistance, yDistance) + 10 * remaining;
    }

    private Node GetLowestFCostNode(List<Node> pathList)
    {
        Node lowestFCostNode = pathList[0];
        for (int i = 1; i < pathList.Count; i++)
        {
            if (pathList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathList[i];
            }
        }

        return lowestFCostNode;
    }

    private List<Node> CalculatePath(Node end)
    {
        List<Node> nodes = new List<Node>();
        nodes.Add(end);
        Node current = end;
        while (current.cameFrom != null)
        {
            if (nodes.Contains(current.cameFrom)) continue;
            nodes.Add(current.cameFrom);
            current = current.cameFrom;
        }
        nodes.Reverse();
        return nodes;
    }
}


/*public List<Path> FindPath()
{
    Path start = new Path(initPos.transform.position);
    Path end = new Path(destination.transform.position);
    openList = new List<Path>();
    openList.Add(start);

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
        Debug.Log(openList.Count);
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
    List<Path> paths = new List<Path>();
    paths.Add(end);
    Path current = end;
    while (current.cameFrom != null)
    {
        paths.Add(current.cameFrom);
        current = current.cameFrom;
        
    }
    paths.Reverse();
    return paths;
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
}*/