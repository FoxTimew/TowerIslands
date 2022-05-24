using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEffectSO : ScriptableObject
{
    public static Sprite sprite;
    
    public virtual IEnumerator ApplyEffect(Enemy enemy,ParticleSystem ps)
    {
        yield return null;
    }
}
