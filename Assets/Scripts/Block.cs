using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class Block : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public List<Block> adjacentBlocks;
    public bool selectable = true;
    [SerializeField] private List<Sprite> sprites;
    public delegate void ApplyEffect();
    public ApplyEffect applyEffect;

    private int bonusEnergy = 1;
    
    private Color baseColor;
    
    
    public Building building;
    
    private int energy = 2;

    public bool selected;
    
    #region Unity Methods

    private void Start()
    {
        UpdateAdjacents();
        baseColor = spriteRenderer.color;
        baseColor.a = 1;
        spriteRenderer.sprite = sprites[Random.Range(0, 1)];
    }

    #endregion
    public Vector2[] InitAdjacents()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;
        return new[]
        {
            new Vector2(posX+1.84f,posY+1.335f),
            new Vector2(posX+1.72f,posY-1.335f),
            new Vector2(posX-1.84f,posY-1.335f),
            new Vector2(posX-1.72f,posY+1.335f),
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
    }

    public void Deselect()
    {
        spriteRenderer.color = baseColor;
    }


    

    public void DefenseSupportEffect()
    {
        
    }
}
