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
        /*Sound*/AudioManager.instance.Play(42, false);
    }
    
    private void OnDisable()
    {
        supportSo.RemoveEffects(affectedBlocks);
    }

    private void UpdateEffects()
    {
        supportSo.RemoveEffects(affectedBlocks);
        supportSo.AddEffects(affectedBlocks);
    }
    
    

    private void GetAffectedBlocks(int range,Block b)
    {
        if (range == 0) return;
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
