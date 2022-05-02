using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;


public class SupportSO : ScriptableObject
{
    public int range;


    public virtual void AddEffect(GameObject go)
    {
        
    }

    public virtual void RemoveEffect(GameObject go)
    {
        
    }

    public virtual void Enter(Collider2D other, ref Dictionary<GameObject,int> dic)
    {
    }
}
