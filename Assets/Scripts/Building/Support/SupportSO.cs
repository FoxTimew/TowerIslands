using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;


public class SupportSO : ScriptableObject
{
    public int range;


    public virtual void AddEffect(Block block)
    {
        
    }

    public virtual void RemoveEffect(Block blocko)
    {
        
    }

    public virtual void AddEffects(List<Block> blocks) 
    {
    }
    public virtual void RemoveEffects(List<Block> blocks)
    {
    }
}
