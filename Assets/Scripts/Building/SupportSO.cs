using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "SupportSO", menuName = "ScriptableObjects/SupportSO", order = 1)]
public class SupportSO : ScriptableObject
{
    public int range;
    

    public virtual void AddEffect(GameObject go)
    {
        
    }

    public virtual void RemoveEffect(GameObject go)
    {
        
    }

    public virtual bool Enter(Collider2D other, ref Dictionary<GameObject,float> dic)
    {
        return default;
    }
    
    public virtual bool Exit(Collider2D other, ref Dictionary<GameObject,float> dic)
    {
        return default;
    }
}
