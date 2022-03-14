using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text goldUI;
    public EconomyManager instance;
    [SerializeField] private int goldAmount = 0;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        UpdateGoldUI();
    }

    public void GainGold(int goldToGain)
    {
        goldAmount += goldToGain;
        UpdateGoldUI();
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
        UpdateGoldUI();
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }

    public void UpdateGoldUI()
    {
        goldUI.text = goldAmount.ToString();
    }
}
