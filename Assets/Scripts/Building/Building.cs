using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Building : MonoBehaviour
{
    
    public Index index;
    public BuildingSO buildingSO;
    public int hp { get; protected set; }

    public delegate void TakeDamage(int dmg);

    public TakeDamage takeDamage;

    protected bool destroyed;
    [SerializeField] private Sprite[] ruins;
    [SerializeField] private SpriteRenderer sr;

    private void Start()
    {
        hp = buildingSO.healthPoints;
        takeDamage += BaseTakeDamage;
    }

    public void BaseTakeDamage(int dmg)
    {
        if(destroyed) return;
        hp -= dmg;
        if (hp <= 0) Ruins();
    }

    public void Ruins()
    {
        sr.sprite = ruins[Random.Range(0, 2)];
        destroyed = true;
        sr.sortingLayerName = "Shadows";
        sr.sortingOrder = 0;
    }
}
