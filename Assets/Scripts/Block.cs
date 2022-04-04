using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour
{

    public enum Effect
    {
        Energy,Defense
    }

    public List<Effect> effects;
    public SpriteRenderer spriteRenderer;
    public List<Block> adjacentBlocks;


    private Color baseColor;
    
    
    public AXD_TowerShoot tower;
    
    public int energy = 2;

    public bool selected;

    public delegate void ApplyEffect();
    public ApplyEffect applyEffect;
    
    
    #region Unity Methods

    private void Start()
    {
        UpdateAdjacents();
        baseColor = spriteRenderer.color;
        baseColor.a = 1;
    }

    #endregion
    public Vector2[] InitAdjacents()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;
        return new[]
        {
            new Vector2(posX+0.5f,posY+0.25f),
            new Vector2(posX+0.5f,posY-0.25f),
            new Vector2(posX-0.5f,posY+0.25f),
            new Vector2(posX-0.5f,posY-0.25f),
        };
    }
    
    public void UpdateAdjacents()
    {
        adjacentBlocks.Clear();
        foreach (Vector2 adj in InitAdjacents())
        {
            if (!GameManager.instance.blocks.ContainsKey(adj)) continue;
            adjacentBlocks.Add(GameManager.instance.blocks[adj]);
        }
    }

    public int GetMaxEnergy()
    {
        int result = energy;
        foreach (var block in adjacentBlocks)
        {
            result += block.energy;
        }
        return result;
    }

    public void Select()
    {
        spriteRenderer.color = Color.green;
    }

    public void Deselect()
    {
        spriteRenderer.color = baseColor;
    }

    void UpdateEffect()
    {
        applyEffect = null;
        if (effects.Contains(Effect.Defense)) applyEffect += DefenseEffect;
        if (effects.Contains(Effect.Energy)) applyEffect += EnergyEffect;
    }

    void EnergyEffect()
    {
        
    }

    void DefenseEffect()
    {
        
    }
}
