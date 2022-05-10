using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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
            new Index(Mathf.Round(size * 0.5f), Mathf.Round(size * 0.5f)-1),
            new Index(Mathf.Round(size * 0.5f)-1, Mathf.Round(size * 0.5f)-1)
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
        GridElements[block.index.x, block.index.y].gridIndex.gameObject.SetActive(false);
        GridElements[block.index.x, block.index.y].walkable = true;
        
    }


}
