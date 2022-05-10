using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Block : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public List<Block> adjacentBlocks;
    public bool selectable = true;
    
    public Dictionary<SupportEffect,int> supportEffects= new Dictionary<SupportEffect,int>();
    public delegate void ApplyEffect();
    public ApplyEffect applyEffect;

    public UnityEvent OnSelect, OnDeselect;
    private int bonusEnergy = 1;
    
    private Color baseColor;
    
    
    public AXD_TowerShoot tower;
    
    private int energy = 2;

    public bool selected;
    
    #region Unity Methods

    private void Start()
    {
        UpdateAdjacents();
        baseColor = spriteRenderer.color;
        baseColor.a = 1;
        foreach (SupportEffect effect in Enum.GetValues(typeof(SupportEffect)))
        {
            supportEffects.Add(effect,0);
        }
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
    
    public int GetEnergy()
    {
        return energy + bonusEnergy;
    }
    
    public void SetEnergy(int value)
    {
        if (value <= bonusEnergy)
        {
            bonusEnergy -= 1;
        }

        if (value > bonusEnergy)
        {
            value -= bonusEnergy;
            bonusEnergy = 0;
            energy -= value;
        }
    }
    public int GetMaxEnergy()
    {
        int result = energy;
        foreach (var block in adjacentBlocks)
        {
            result += block.GetEnergy();
            result += bonusEnergy;
        }
        return result;
    }

    public void Select()
    {
        spriteRenderer.color = Color.green;
        OnSelect.Invoke();
        Debug.Log("Block selected : "+gameObject.name);
    }

    public void Deselect()
    {
        spriteRenderer.color = baseColor;
        OnDeselect.Invoke();
        Debug.Log("Block deselected");
    }



    public void UpdateSupportEffect()
    {
        bonusEnergy = supportEffects[SupportEffect.Energy] > 0 ? 2 : 0;
        if (supportEffects[SupportEffect.Defense] > 0)
            applyEffect += DefenseSupportEffect;
        else
            applyEffect -= DefenseSupportEffect;
    }
    
    public void EnergySupportEffect()
    {
        
    }

    public void DefenseSupportEffect()
    {
        
    }

    public void Test()
    {
        Debug.Log("test");
    }
}
