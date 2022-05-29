using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergySupportSO", menuName = "ScriptableObjects/Support/EnergySupportSO", order = 1)]
public class EnergySupportSO : SupportSO
{
    public int energyValue;
    
    public override void AddEffect(Block block)
    {
        block.effect = true;
    }

    public override void RemoveEffect(Block block)
    {
        block.effect = false;
    }
    
    public override void AddEffects(List<Block> blocks)
    {
        foreach (var block in blocks)
        {
            AddEffect(block);
        }
    }
    
    public override void RemoveEffects(List<Block> blocks)
    {
        foreach (var block in blocks)
        {
            RemoveEffect(block);
        }
    }
    
}
