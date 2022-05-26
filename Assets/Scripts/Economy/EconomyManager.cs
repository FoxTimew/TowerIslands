using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;
    [SerializeField] private int goldAmount = 200;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Enemy.EnemyDeathGoldEvent += Enemy_EnemyDeathGoldEvent;
    }

    public void GainGold(int goldToGain)
    {
        goldAmount += goldToGain;
        Debug.Log("Gain Gold UI Update");
        UpdateEconomyUI();
    }

    public void SetGold(int gold)
    {
        goldAmount = gold;
        Debug.Log("Set Gold UI Update");
        UpdateEconomyUI();
    }

    public void RemoveGold(int goldToRemove)
    {
        goldAmount -= goldToRemove;
        Debug.Log("Remove Gold UI Update");
        UpdateEconomyUI();
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }

    private void UpdateEconomyUI()
    {
        if (UI_Manager.instance != null)
        {
            UI_Manager.instance.UpdateGoldUI(goldAmount);
            UI_Manager.instance.UpdateWaveUI();
        }
        else
        {
            Debug.Log("l'instance existe pas");
        }

    }
    

    
    public void Enemy_EnemyDeathGoldEvent(int goldToAdd)
    {
        GainGold(goldToAdd);
    }

}
