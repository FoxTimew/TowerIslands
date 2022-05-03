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
    
    public class Node
    {
        public Vector2 pos;

        public int x;
        public int y;
        public float gCost;
        public float hCost;
        public float fCost;

        public Node cameFrom;

        public Node(Vector2 pos, int x, int y)
        {
            this.pos = pos;
            this.x = x;
            this.y = y;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }

    public List<Node> FindPath(Vector2 start, Vector2 end, Vector2[,] map, int[,] walkables)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        for (int j = 0; j < map.GetLength(1); j++)
        {
            Node node = new Node(map[i,j],i,j);
            node.gCost = int.MaxValue;
            node.CalculateFCost();
            node.cameFrom = null;
            grid.Add(new Vector2(i,j),node);
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
            foreach (var neighbour in NeighbourList(currentNode, map.GetLength(0)))
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

    private List<Vector2> NeighbourList(Node current,int lenght)
    {
        List<Vector2> result = new List<Vector2>();
        Vector2[] neigbourPos = new[]
        {
            new Vector2(current.x + 1, current.y - 1),
            new Vector2(current.x + 1, current.y + 1),
            new Vector2(current.x - 1, current.y - 1),
            new Vector2(current.x - 1, current.y + 1),
        };
        foreach (var pos in neigbourPos)
        {
            if (pos.x < 0 || pos.x > lenght || pos.y < 0 || pos.y > lenght) continue;
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
