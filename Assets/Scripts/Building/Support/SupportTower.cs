using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SupportTower : Building
{
    
    [SerializeField] private SupportSO supportSo;

    public List<Block> affectedBlocks;
    

    void Start()
    {
        GetAffectedBlocks(supportSo.range,GameManager.instance.grid.GridElements[index.x,index.y].block);
        supportSo.AddEffects(affectedBlocks);
    }
    
    private void OnDisable()
    {
        supportSo.RemoveEffects(affectedBlocks);
    }

    
    
    private void GetAffectedBlocks(int range,Block b)
    {
        Debug.Log(range);
        if (range == 0) return;
        Debug.Log(b.adjacentBlocks.Keys.Count);
        foreach (var block in b.adjacentBlocks.Keys)
        {
            if (!affectedBlocks.Contains(block))
            {
                affectedBlocks.Add(block);
            }
            GetAffectedBlocks(range-1,block);
        }
    }
}
