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
    public int hp { get; set; }

    public delegate void TakeDamage(int dmg);

    public TakeDamage takeDamage;

    protected bool destroyed = false;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] protected SpriteRenderer sr;
    
    private void OnEnable()
    {
        hp = buildingSO.healthPoints;
        takeDamage += BaseTakeDamage;
        SetBuilding();
    }
    
    public void BaseTakeDamage(int dmg)
    {
        if (destroyed) return;
        hp -= dmg;
        sr.DOComplete();
        sr.DOColor(Color.HSVToRGB(1,0.5f,1), .1f).SetLoops(2,LoopType.Yoyo);
        if (hp > 0)
        {
            /*Sound*/AudioManager.instance.Play(40, false);
            return;
        }
        if (GameManager.instance.HDV == this)
        {
            UI_Manager.instance.OpenMenuWithoutTransition(11); 
            UI_Manager.instance.CloseMenuWithoutTransition(8);
            GameManager.instance.ClearBuildings();
            GameManager.instance.StopAllCoroutines();
        }
        else
        {
            Ruins();
            /*Sound*/ AudioManager.instance.Play(12);
        }
    }

    public virtual void Reset()
    {
        return;
    }
    public virtual void Ruins()
    {
        sr.sprite = sprites[Random.Range(1, 3)];
        destroyed = true;
        GameObject go = Pooler.instance.Pop("DestructionFx");
        go.transform.position = transform.position;
        Pooler.instance.DelayedDepop(1f,"DestructionFx",go);
        sr.sortingLayerName = "Shadows";
        sr.sortingOrder = 0;
    }

    public virtual void Repair()
    {
        destroyed = false;
        hp = buildingSO.healthPoints;
        if (GameManager.instance.HDV == this) return;
        sr.sortingLayerName = "Characters";
        sr.sprite = sprites[0];
        EconomyManager.instance.RemoveGold(buildingSO.goldRequired * (buildingSO.healthPoints-hp)*100/buildingSO.healthPoints);
    }

    public virtual void SetBuilding()
    {
        destroyed = false;
        hp = buildingSO.healthPoints;
        sr.sortingLayerName = "Characters";
        sr.sprite = sprites[0];
    }
    public bool IsBuildingDestroyed()
    {
        return destroyed;
    }

    public virtual void ResetTarget()
    {
        return;
    }
}