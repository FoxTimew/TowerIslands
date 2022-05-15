using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public struct Index
{
    public int x;
    public int y;

    public Index(float _x, float _y)
    {
        x = (int)_x;
        y = (int)_y;
    }

    public bool Equals(Index index)
    {
        if (index.x == x && index.y == y) return true;
        return false;
    }
}
public class Grid
{
    
    
    public class GridElement
    {
        public bool walkable;
        public Vector2 position;
        public Block block = null;
        public GridIndex gridIndex;

        public GridElement(bool value, Vector2 pos)
        {
            position = pos;
            walkable = value;
        }
    }
    
    public int size;
    public GridElement[,] GridElements;
    
    private Vector2 zeroPos;

    public Index[] hdvIndex;

    public Index[] baseBlocks;
    
    public Grid(int s)
    {
        size = s;
        GridElements = new GridElement[size, size];
        zeroPos = new Vector2( -0.06f + (-0.12f * (Mathf.Round(size*0.5f)-1)),  -1.335f + (-2.67f * (Mathf.Round(size*0.5f)-1)));
        for (int i = 0; i < size; i++)
        {
            Vector2 tempZero = zeroPos + new Vector2(-1.72f *i, 1.335f*i);
            for (int j = 0; j < size; j++)
            {
                Vector2 temp = tempZero + new Vector2(1.84f * j, 1.335f * j);
                GridElements[i, j] = new GridElement(false,temp);
            }
        }

        hdvIndex = new[]
        {
            new Index(Mathf.Round(size * 0.5f), Mathf.Round(size * 0.5f)),
            new Index(Mathf.Round(size * 0.5f)-1, Mathf.Round(size * 0.5f)),
            new Index(Mathf.Round(size * 0.5f)-1, Mathf.Round(size * 0.5f)-1),
            new Index(Mathf.Round(size * 0.5f), Mathf.Round(size * 0.5f)-1)
        };

        baseBlocks = new[]
        {
            new Index(hdvIndex[0].x+1, hdvIndex[0].y),
            new Index(hdvIndex[0].x, hdvIndex[0].y+1),
            
            new Index(hdvIndex[1].x, hdvIndex[1].y+1),
            new Index(hdvIndex[1].x-1, hdvIndex[1].y),
            
            new Index(hdvIndex[2].x-1, hdvIndex[2].y),
            new Index(hdvIndex[2].x, hdvIndex[2].y-1),
            
            new Index(hdvIndex[3].x, hdvIndex[3].y-1),
            new Index(hdvIndex[3].x+1, hdvIndex[3].y),
        };
    }

    public Grid(Grid _grid)
    {
        GridElements = _grid.GridElements;
        hdvIndex = _grid.hdvIndex;
    }

    public void AddBlock(Block block)
    {
        
        GridElements[block.index.x, block.index.y].block = block;
        GridElements[block.index.x, block.index.y].block.placed = true;
        GridElements[block.index.x, block.index.y].block.selectable = true;
        GridElements[block.index.x, block.index.y].gridIndex.Disable();
        GridElements[block.index.x, block.index.y].walkable = true;
        
    }

    public Block GetNearestBlock(Vector3 pos)
    {
        Block result = GridElements[hdvIndex[0].x,hdvIndex[0].y].block;
        var dist = float.MaxValue;
        foreach (var element in GridElements)
        {
            if (!element.walkable) continue;
            foreach (var index in hdvIndex) if (element.block.index.Equals(index)) continue;
            if (!(((Vector3) element.position - pos).magnitude < dist)) continue;
            dist = ((Vector3) element.position - pos).magnitude;
            result = element.block;
        }
        return result;
    }


}
