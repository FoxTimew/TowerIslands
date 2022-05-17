using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text goldUI;
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
        UpdateUI();
    }

    public void GainGold(int goldToGain)
    {
        goldAmount += goldToGain;
        UpdateUI();
    }

    public void RemoveGold(int goldToRemove)
    {
        goldAmount -= goldToRemove;
        UpdateUI();
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }
    
    
    public void UpdateUI()
    {
        if (goldUI != null)
        {
            goldUI.text = $"{goldAmount}";
        }
    }

    
    public void Enemy_EnemyDeathGoldEvent(int goldToAdd)
    {
        GainGold(goldToAdd);
    }

}
