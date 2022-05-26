using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berzerk : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    public bool rage = false;

    void Update()
    {
        if (rage) return;
        if (enemy.currentHP > enemy.enemyStats.maxHealthPoints * 0.2f) return;
        Rage();
    }

    private void Rage()
    {
        enemy.animator.SetFloat("Speed",0);
        enemy.animator.SetTrigger("AttackEnd");
        enemy.animator.SetTrigger("Rage");
        enemy.currentDamage = (int) (enemy.enemyStats.damage * 1.2f);
        rage = true;
    }
}
