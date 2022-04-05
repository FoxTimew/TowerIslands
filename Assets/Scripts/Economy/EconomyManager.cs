using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text goldUI;
    [SerializeField] private TMP_Text arcanumyUI;
    public EconomyManager instance;
    [SerializeField] private int goldAmount = 0;
    [SerializeField] private int arcanumAmount = 0;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        Enemy.EnemyDeathGoldEvent += Enemy_EnemyDeathGoldEvent;
        Enemy.EnemyDeathCristalEvent += Enemy_EnemyDeathCristalEvent;
        UpdateUI();
    }

    public void GainGold(int goldToGain)
    {
        goldAmount += goldToGain;
        UpdateUI();
    }

    public void RemoveGold(int goldToRemove)
    {
        if (goldAmount > goldToRemove)
        {
            goldAmount -= goldToRemove;
        }
        else
        {
            goldAmount = 0;
        }
        UpdateUI();
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }

    public void GainCristal(int arcanumToAdd)
    {
        arcanumAmount += arcanumToAdd;
        UpdateUI();
    }
    
    public void RemoveArcanum(int goldToRemove)
    {
        if (arcanumAmount > goldToRemove)
        {
            arcanumAmount -= goldToRemove;
        }
        else
        {
            arcanumAmount = 0;
        }
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        goldUI.text = goldAmount.ToString();
        arcanumyUI.text = arcanumAmount.ToString();
    }

    
    public void Enemy_EnemyDeathGoldEvent(int goldToAdd)
    {
        GainGold(goldToAdd);
    }
    public void Enemy_EnemyDeathCristalEvent(int cristalToAdd)
    {
        GainCristal(cristalToAdd);
    }
    
}
