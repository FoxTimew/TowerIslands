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
        if (instance == null)
        {
            instance = this;
        }

        Enemy.EnemyDeathGoldEvent += Enemy_EnemyDeathGoldEvent;
        UpdateEconomyUI();
    }

    public void GainGold(int goldToGain)
    {
        goldAmount += goldToGain;
        UpdateEconomyUI();
    }

    public void SetGold(int gold)
    {
        goldAmount = gold;
        UpdateEconomyUI();
    }

    public void RemoveGold(int goldToRemove)
    {
        goldAmount -= goldToRemove;
        UpdateEconomyUI();
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }

    private void UpdateEconomyUI()
    {
        UI_Manager.instance.UpdateGoldUI(goldAmount);
        UI_Manager.instance.UpdateWaveUI();
    }
    

    
    public void Enemy_EnemyDeathGoldEvent(int goldToAdd)
    {
        GainGold(goldToAdd);
    }

}
