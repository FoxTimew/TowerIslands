using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StunTrapSO", menuName = "ScriptableObjects/Trap/StunTrapSO", order = 1)]
public class StunTrapSO : TrapEffectSO
{
    public float stunDuration;

    private WaitForSeconds stun;
    public void OnEnable()
    {
        stun = new WaitForSeconds(stunDuration);
    }

    public override IEnumerator ApplyEffect(Enemy enemy)
    {
        while (true)
        {
            enemy.StopMovement(stun);
            yield return stun;
        }
    }
}
