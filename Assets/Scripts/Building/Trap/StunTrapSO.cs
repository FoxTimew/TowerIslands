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
            speedTemp = enemy.speed;
            enemy.speed = 0;
            yield return new WaitForSeconds(attackSpeed);
            enemy.speed = speedTemp;
            yield return null;
        }
    }
}
