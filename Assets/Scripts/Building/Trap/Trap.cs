using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Building
{
    [SerializeField] private TrapEffectSO effect;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem ps;
    private Dictionary<Collider2D, Coroutine> enemies = new Dictionary<Collider2D, Coroutine>();

    
    private Coroutine routine;
    void Start()
    {
        //spriteRenderer.sprite = TrapEffectSO.sprite;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        routine = StartCoroutine(effect.ApplyEffect(other.GetComponent<Enemy>(),ps));
        enemies.Add(other,routine);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!enemies.ContainsKey(other)) return;
        StopCoroutine(enemies[other]);
        enemies.Remove(other);
    }
}

