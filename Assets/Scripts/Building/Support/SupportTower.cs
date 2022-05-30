using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SupportTower : Building
{
    
    [SerializeField] private SupportSO supportSo;

    public List<Block> affectedBlocks;
    public List<Block> affectedBuildings;

    private void Start()
    {
        
        GetAffectedBlocks(supportSo.range,GameManager.instance.grid.GridElements[index.x,index.y].block);
        supportSo.AddEffects(affectedBlocks);
        affectedBlocks.Remove(GameManager.instance.grid.GridElements[index.x, index.y].block);
        AddBlocksFx();
        /*Sound*/AudioManager.instance.Play(42, false);
    }

    void Update()
    {
        foreach (var block in affectedBlocks)
        {
            if (block.building is null) continue;
            if (block.effect) continue;
            GameObject go = Pooler.instance.Pop("FoudreFx");
            go.transform.position = block.transform.position;
            fxs.Add(go);
            supportSo.AddEffect(block);
        }
    }
    private void OnDisable()
    {
        supportSo.RemoveEffects(affectedBlocks);
        RemoveBlocksFx();
    }
    
    

    private List<GameObject> fxs = new List<GameObject>();
    
    void AddBlocksFx()
    {
        foreach (var block in affectedBuildings)
        {
            GameObject go = Pooler.instance.Pop("FoudreFx");
            go.transform.position = block.transform.position;
            fxs.Add(go);
        }
    }
    
    void RemoveBlocksFx()
    {
        for (int i = fxs.Count - 1; i > -1; i--)
        {
            Pooler.instance.Depop("FoudreFx",fxs[i]);
            fxs.Remove(fxs[i]);
        }
    }

    private void GetAffectedBlocks(int range,Block b)
    {
        
        if (range == 0) return;
        foreach (var block in b.adjacentBlocks.Keys)
        {
            if (!affectedBlocks.Contains(block))
            {
                affectedBlocks.Add(block);
                if(block.building is not null) affectedBuildings.Add(block);
            }
            GetAffectedBlocks(range-1,block);
        }
    }
    
    
}
