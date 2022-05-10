using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class Block : MonoBehaviour
{

    public Index index;
    public SpriteRenderer spriteRenderer;
    public Dictionary<Block,int> adjacentBlocks = new Dictionary<Block, int>();
    public bool selectable = true;
    [SerializeField] private List<Sprite> sprites;

    private Collider2D collider;
    public bool placed;
    
    
    public Building building;
    
    public int energy = 2;
    
    
    #region Unity Methods

    private void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0,2)];
    }

    #endregion
    
    #region Energy

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
        if (value < energy)
        {
            energy -= value;
            return true;
        }
        else
        {
            energy = value - energy;
            SpentAdjacentEnergy(value - energy);
            return true;
        }
        return false;
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


    #endregion
    
    #region Building

    public void DestroyBuilding()
    {
        int buildingValue = building.buildingSO.energyRequired;
        foreach (var block in adjacentBlocks.Keys)
        {
            if (adjacentBlocks[block] == 0) continue;
            buildingValue -= adjacentBlocks[block];
            block.energy += adjacentBlocks[block];
            adjacentBlocks[block] = 0;
        }

        if (buildingValue <= 0) return;
        energy += buildingValue;
        Pooler.instance.Depop(building.buildingSO.bName,
            building.buildingSO.type == BuildingType.Trap ? building.gameObject : building.transform.parent.gameObject);
        EconomyManager.instance.GainGold(building.buildingSO.goldRequired);
        building = null;
    }

    private GameObject go;
    public void Build(BuildingSO building)
    {
        SpentEnergy(building.energyRequired);
        go = Pooler.instance.Pop(building.bName);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        EconomyManager.instance.RemoveGold(building.goldRequired);
        this.building = building.type == BuildingType.Trap ? go.GetComponent<Building>() : go.transform.parent.GetComponent<Building>();
    }

    #endregion
    public void UpdateAdjacents()
    {
        
        var temp = Utils.GetAdjacentsIndex(index);
        for (int i = 0; i < temp.Count; i++)
        {
            if (adjacentBlocks.ContainsKey(GameManager.instance.grid.GridElements[temp[i].x, temp[i].y].block))
                continue;
            adjacentBlocks.Add(GameManager.instance.grid.GridElements[temp[i].x,temp[i].y].block,0);
            //Debug.Log($"{name} close to {GameManager.instance.grid.GridElements[temp[i].x,temp[i].y].block.name} : {temp[i].x}, {temp[i].y}");
        }
    }
    public void Select()
    {
        GameManager.instance.selectedBlock = this;
        spriteRenderer.color = Color.green;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.transform.CompareTag("GridElement")) return;
        index = other.GetComponent<GridIndex>().index;
        collider = other;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.transform.CompareTag("GridElement")) return;
        if (other != collider) return;
        if (placed) return;
        index = new Index(5,5);
    }
}
