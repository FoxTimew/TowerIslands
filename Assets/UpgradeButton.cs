using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    private void OnEnable()
    {
        if (GameManager.instance.selectedBlock.building.buildingSO.type != BuildingType.Tower)
        {
            gameObject.SetActive(false);
            return;
        }
        Tower to = GameManager.instance.selectedBlock.building.gameObject.GetComponent<Tower>();
        if(to.towerSO.level != 1)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
    }
    
}
