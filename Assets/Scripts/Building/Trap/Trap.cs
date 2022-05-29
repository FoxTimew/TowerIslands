using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Building
{
    [SerializeField] private TrapEffectSO effect;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem ps;
    private List<Collider2D> enemies = new List<Collider2D>();
    private bool reloading;
    private WaitForSeconds psTime = new WaitForSeconds(0.5f);
    private WaitForSeconds reloadingTime = new WaitForSeconds(1.5f);
    
    private Coroutine routine;
    void Start()
    {
        StartCoroutine(StartTrap());
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.transform.parent.CompareTag("Enemy")) return;
        enemies.Add(other);
        
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!enemies.Contains(other)) return;
        enemies.Remove(other);
    }

    
    private IEnumerator StartTrap()
    {
        while (true)
        {
            while (enemies.Count <= 0) yield return null;
            while(reloading) yield return null;
            reloading = true;
            ps.Play();
            /*Sound*/AudioManager.instance.Play(41, false);
            yield return psTime;
            foreach (var enemy in enemies)
            {
                StartCoroutine(effect.ApplyEffect(enemy.GetComponentInParent<Enemy>(), ps));
            }
            yield return reloadingTime;
            reloading = false;
        }
    }
}

