using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefenseSupportSO", menuName = "ScriptableObjects/Support/DefenseSupportSO", order = 1)]
public class DefenseSupportSO : SupportSO
{

    public int healthValue;

    private Building tower;
    
    public override void AddEffect(Block block)
    {
        if (block.building is null) return;
        block.building.hp += healthValue;
    }

    public override void RemoveEffect(Block block)
    {
        block.building.hp -= healthValue;
        if(block.building.hp <= 0) block.building.takeDamage(1);
    }
    
    public override void AddEffects(List<Block> blocks)
    {
        foreach (var block in blocks)
        {
            AddEffect(block);
        }
    }
}
