using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StunTrapSO", menuName = "ScriptableObjects/Trap/StunTrapSO", order = 1)]
public class StunTrapSO : TrapEffectSO
{
    public float attackSpeed;
    public float stunDuration;

    private float speedTemp;
    public override IEnumerator ApplyEffect(Enemy enemy)
    {
        while (true)
        {
            speedTemp = enemy.speedRatio;
            enemy.speedRatio = 0;
            yield return new WaitForSeconds(attackSpeed);
            enemy.speedRatio = speedTemp;
            yield return null;
        }
    }
}
