using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StunTrapSO", menuName = "ScriptableObjects/Trap/StunTrapSO", order = 1)]
public class StunTrapSO : TrapEffectSO
{
    public float attackSpeed;
    public float stunDuration;


    public override IEnumerator ApplyEffect(Enemy enemy)
    {
        while (true)
        {
            enemy.StopMovement();
            yield return new WaitForSeconds(attackSpeed);
            enemy.StartMovement();
            yield return null;
        }
    }
}
