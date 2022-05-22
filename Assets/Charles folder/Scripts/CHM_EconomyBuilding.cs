using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CHM_EconomyBuilding : MonoBehaviour
{
    public CHM_ScriptableEconomyBuilding sOEconomyBuilding;

    public CHM_Resources_CityCenter resources;

    public TMP_Text buildingNameText, goldMultiplierText, towerCostDividerText, upgradePriceText;
    private float goldMultiplier, towerCostDivider;
    private int upgradePrice;
    private bool maxTier;

    public GameObject upgradeButton;
    
    public void Awake()
    {
        ReloadStats();
    }

    public void ReloadStats()
    {
        if(sOEconomyBuilding.nextBuilding == null)
        {
            maxTier = true;
        }
        if (maxTier == false)
        {
            buildingNameText.text = sOEconomyBuilding.buildingName;
            goldMultiplierText.text = "Gold Multiplier : " + sOEconomyBuilding.goldMultiplier.ToString();
            towerCostDividerText.text = "Tower cost divider : " + sOEconomyBuilding.towerCostDivider.ToString();
            upgradePriceText.text = "Upgrade Price : " + sOEconomyBuilding.nextBuilding.upgradePrice.ToString();

            goldMultiplier = sOEconomyBuilding.goldMultiplier;
            towerCostDivider = sOEconomyBuilding.towerCostDivider;
            upgradePrice = sOEconomyBuilding.nextBuilding.upgradePrice;
        }

        if(maxTier == true)
        {
            buildingNameText.text = sOEconomyBuilding.buildingName;
            goldMultiplierText.text = "Gold Multiplier : " + sOEconomyBuilding.goldMultiplier.ToString();
            towerCostDividerText.text = "Tower cost divider : " + sOEconomyBuilding.towerCostDivider.ToString();
            upgradePriceText.text = "Max upgrade";

            goldMultiplier = sOEconomyBuilding.goldMultiplier;
            towerCostDivider = sOEconomyBuilding.towerCostDivider;
            upgradeButton.SetActive(false);
        }
    }


    public void Upgrade()
    {
        if(resources.gold > upgradePrice)
        {
            resources.gold -= upgradePrice;
            sOEconomyBuilding = sOEconomyBuilding.nextBuilding;
            ReloadStats();
            resources.ReloadFunds();
        }
    }
}
