using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageTrapSO", menuName = "ScriptableObjects/Trap/DamageTrapSO", order = 1)]
public class DamageTrapSO : TrapEffectSO
{
    public float attackSpeed;
    public int damage;
    public DamageType damageType;
        
    
    public override IEnumerator ApplyEffect(Enemy enemy,ParticleSystem ps)
    {
        while (true)
        {
            enemy.TakeDamage(damageType,damage,null);
            yield return new WaitForSeconds(attackSpeed);
        }
    }
}
