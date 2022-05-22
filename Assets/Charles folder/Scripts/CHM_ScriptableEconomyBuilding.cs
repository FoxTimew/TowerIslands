using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Economy Building", menuName = "EconomyBuilding")]
public class CHM_ScriptableEconomyBuilding : ScriptableObject
{
    public string buildingName;

    public float goldMultiplier, towerCostDivider;

    public int upgradePrice;
    public GameObject buildingObject;

    public CHM_ScriptableEconomyBuilding nextBuilding;

    public bool maxTier;
}
