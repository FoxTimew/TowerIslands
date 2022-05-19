using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    
    void Update()
    {
        Tower to = (Tower)GameManager.instance.selectedBlock.building;
        gameObject.SetActive(to.towerSO.nextLevel is not null);
    }
}
