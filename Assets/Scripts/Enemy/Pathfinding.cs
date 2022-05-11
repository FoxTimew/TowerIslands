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

    private Dictionary<Index, Node> grid = new Dictionary<Index, Node>();
    
    public class Node
    {
        public Vector2 pos;

        public Index index;
        public float gCost;
        public float hCost;
        public float fCost;

        public Node cameFrom;

        public Node(Vector2 pos, int x, int y)
        {
            this.pos = pos;
            index.x = x;
            index.y = y;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }

    public List<Node> FindPath(Block startPos, Block endPos, Grid _grid)
    {
        
        Index start = startPos.index;
        Node s = new Node(_grid.GridElements[startPos.index.x, startPos.index.y].position, startPos.index.x,
            startPos.index.y);
        s.gCost = int.MaxValue;
        s.CalculateFCost();
        s.cameFrom = null;
        grid.Add(start,s);
        
        Index end = endPos.index;
        Node e = new Node(_grid.GridElements[endPos.index.x, endPos.index.y].position, endPos.index.x,
            endPos.index.y);
        e.gCost = int.MaxValue;
        e.CalculateFCost();
        e.cameFrom = null;
        grid.Add(end,e);
        
        //Debug.Log(start.x + " " + start.y);
        //Debug.Log(end.x + " " + end.y);
        
        for (int i = 0; i < _grid.size; i++)
        for (int j = 0; j < _grid.size; j++)
        {
            if (!_grid.GridElements[i, j].walkable) continue;
            if (i == start.x && j == start.y) continue;
            if (i == end.x && j == end.y) continue;
            Node node = new Node(_grid.GridElements[i, j].position,i,j);
            node.gCost = int.MaxValue;
            node.CalculateFCost();
            node.cameFrom = null;
            grid.Add(new Index(i, j),node);
            
        }
        toCheckList.Add(grid[start]);
        grid[start].gCost = 0;
        grid[start].hCost = CalculateDistance(grid[start], grid[end]);
        grid[start].CalculateFCost();
        while (toCheckList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(toCheckList);
            //Debug.Log("currentNode "+ currentNode.index.x + " " + currentNode.index.y);
            if (currentNode == grid[end])
                return CalculatePath(grid[end]);
            toCheckList.Remove(currentNode);
            checkedList.Add(currentNode);
            foreach (var neighbour in NeighbourList(currentNode, _grid.size))
            {
                if (checkedList.Contains(grid[neighbour])) continue;
                //Debug.Log("neigbour "+ neighbour.x + " " + neighbour.y);
                float tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, grid[neighbour]);
                if (tentativeGCost < grid[neighbour].gCost)
                {
                    grid[neighbour].cameFrom = grid[new Index(currentNode.index.x,currentNode.index.y)];
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

    private List<Index> NeighbourList(Node current,int lenght)
    {
        List<Index> results = Utils.GetAdjacentsIndex(current.index);
        foreach (var index in results)
        {
            if (!grid.ContainsKey(index))
                results.Remove(index);
        }
        //Debug.Log(results.Count);
        return results;
    }

    private float CalculateDistance(Node a, Node b)
    {
        float xDistance = Mathf.Abs(a.index.x - b.index.x);
        float yDistance = Mathf.Abs(a.index.y - b.index.y);
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
