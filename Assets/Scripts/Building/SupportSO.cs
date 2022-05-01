using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public enum AffectedType
{
    Block, Tower
}

[CreateAssetMenu(fileName = "SupportSO", menuName = "ScriptableObjects/SupportSO", order = 1)]
public class SupportSO : ScriptableObject
{
    public AffectedType affectedType;
    public int range;
    

    public virtual void AddEffect(GameObject go)
    {
        
    }

    public virtual void RemoveEffect(GameObject go)
    {
        
    }

    public void Enter(Collider2D other, ref Dictionary<GameObject,float> dic)
    {
        switch (affectedType)
        {
            case AffectedType.Block :
                if(other.transform.CompareTag("Block")) AddEffect(other.gameObject);
                break;
            case AffectedType.Tower :
                if(other.transform.CompareTag("Tower")) AddEffect(other.gameObject);
                break;
        }
    }
}
