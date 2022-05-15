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

    protected bool destroyed = false;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer sr;

    private void OnEnable()
    {
        hp = buildingSO.healthPoints;
        takeDamage += BaseTakeDamage;
    }
    
    public void BaseTakeDamage(int dmg)
    {
        if (destroyed) return;
        hp -= dmg;
        if (hp > 0) return;
        if (GameManager.instance.HDV == this)
        {
            UI_Manager.instance.OpenMenu(11); 
            UI_Manager.instance.CloseMenu(8);
        }
        else
        {
            Ruins();
        }
    }

    public void Ruins()
    {
        sr.sprite = sprites[Random.Range(1, 3)];
        destroyed = true;
        sr.sortingLayerName = "Shadows";
        sr.sortingOrder = 0;
    }

    public void Repair()
    {
        destroyed = false;
        hp = buildingSO.healthPoints;
        if (GameManager.instance.HDV == this) return;
        sr.sortingLayerName = "Characters";
        sr.sprite = sprites[0];
    }
}