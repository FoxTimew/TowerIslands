using System;
using System.Collections.Generic;
using UnityEngine;


public class SupportTower : Building
{
    [SerializeField] private SupportSO supportSo;
    
    private Dictionary<GameObject, float> elementsAffected = new Dictionary<GameObject, float>();


    void Start()
    {
        foreach (var go in elementsAffected.Keys)
        {
            supportSo.AddEffect(go);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        supportSo.Enter(other, ref elementsAffected );
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        supportSo.Exit(other, ref elementsAffected );
    }
    
    private void OnDisable()
    {
        foreach (var go in elementsAffected.Keys)
        {
            supportSo.RemoveEffect(go);
        }
    }
    
    
    
}
