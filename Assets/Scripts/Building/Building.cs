using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Building : MonoBehaviour
{
    public Index index;
    public BuildingSO buildingSO;
    [SerializeField] private ParticleSystem destruction;
    public int hp { get; protected set; }

    public delegate void TakeDamage(int dmg);

    public TakeDamage takeDamage;

    protected bool destroyed = false;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] protected SpriteRenderer sr;
    
    private void OnEnable()
    {
        hp = buildingSO.healthPoints;
        takeDamage += BaseTakeDamage;
        Repair();
    }
    
    public void BaseTakeDamage(int dmg)
    {
        if (destroyed) return;
        hp -= dmg;
        sr.DOComplete();
        sr.DOColor(Color.red, .1f).SetLoops(6,LoopType.Yoyo);
        if (hp > 0) return;
        if (GameManager.instance.HDV == this)
        {
            UI_Manager.instance.OpenMenuWithoutTransition(11); 
            UI_Manager.instance.CloseMenuWithoutTransition(8);
        }
        else
        {
            Ruins();
        }
    }

    public virtual void Ruins()
    {
        sr.sprite = sprites[Random.Range(1, 3)];
        destroyed = true;
        destruction.Play();
        sr.sortingLayerName = "Shadows";
        sr.sortingOrder = 0;
    }

    public virtual void Repair()
    {
        destroyed = false;
        hp = buildingSO.healthPoints;
        sr.sortingLayerName = "Characters";
        sr.sprite = sprites[0];
    }
}