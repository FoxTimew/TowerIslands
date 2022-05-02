using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class Block : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Dictionary<Block,int> adjacentBlocks = new Dictionary<Block, int>();
    public bool selectable = true;
    [SerializeField] private List<Sprite> sprites;
    
    
    private Color baseColor;
    
    
    public Building building;
    
    public int energy = 2;

    public bool selected;
    
    #region Unity Methods

    private void Start()
    {
        UpdateAdjacents();
        baseColor = spriteRenderer.color;
        baseColor.a = 1;
        spriteRenderer.sprite = sprites[Random.Range(0, 2)];
    }

    #endregion
    public Vector2[] InitAdjacents()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;
        return new[]
        {
            new Vector2(posX+1.84f,posY+1.335f),
            new Vector2(posX+1.72f,posY-1.335f),
            new Vector2(posX-1.84f,posY-1.335f),
            new Vector2(posX-1.72f,posY+1.335f),
        };
    }
    
    public void UpdateAdjacents()
    {
        adjacentBlocks.Clear();
        foreach (Vector2 adj in InitAdjacents())
        {
            if (!GameManager.instance.blocks.ContainsKey(adj)) continue;
            adjacentBlocks.Add(GameManager.instance.blocks[adj],0);
        }
    }


    
    public int GetMaxEnergy()
    {
        int result = energy;
        foreach (var block in adjacentBlocks.Keys)
        {
            result += block.energy;
        }
        return result;
    }

    public bool SpentEnergy(int value)
    {
        if (value > GetMaxEnergy()) return false;
        if (value < energy) energy -= value;
        else
        {
            energy = value - energy;
            SpentAdjacentEnergy(value - energy);
        }
        return true;
    }
    private void SpentAdjacentEnergy(int value)
    {
        int temp = value;
        foreach (var block in adjacentBlocks.Keys)
        {
            if (block.energy <= value) continue;
            adjacentBlocks[block] = value;
            block.energy -= value;
            temp -= value;
        }

        if (temp <= 0) return;
        while (temp > 0)
        {
            foreach (var block in adjacentBlocks.Keys)
            {
                if (temp - block.energy < 0)
                {
                    block.energy -= temp;
                    temp = 0;
                    adjacentBlocks[block] = temp;
                }
                else
                {
                    temp -= block.energy;
                    adjacentBlocks[block] = block.energy;
                    block.energy = 0;
                }
            }  
        }
    }

    public void Select()
    {
        spriteRenderer.color = Color.green;
    }
    
    public void DestroyBuilding()
    {
        int buildingValue = building.buildingSO.energyRequired;
        foreach (var block in adjacentBlocks.Keys)
        {
            buildingValue -= adjacentBlocks[block];
            block.energy += adjacentBlocks[block];
            adjacentBlocks[block] = 0;
        }

        if (buildingValue <= 0) return;
        energy += buildingValue;
    }
    public void Deselect()
    {
        spriteRenderer.color = baseColor;
    }

    
}
