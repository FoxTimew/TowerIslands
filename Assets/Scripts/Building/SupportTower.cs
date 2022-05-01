using System;
using System.Collections.Generic;
using UnityEngine;


public class SupportTower : Building
{
    [SerializeField] private SupportSO supportSo;
    
    private Dictionary<GameObject, float> elementsAffected = new Dictionary<GameObject, float>();

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        supportSo.Enter(other, ref elementsAffected );
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!elementsAffected.ContainsKey(other.gameObject)) return;
        supportSo.RemoveEffect(other.gameObject);
        elementsAffected.Remove(other.gameObject);
    }
    
    private void OnDisable()
    {
        foreach (var go in elementsAffected.Keys) supportSo.RemoveEffect(go);
    }
    
    
    
}
