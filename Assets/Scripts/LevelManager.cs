using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    [SerializeField] private GameObject blockInfogroup;
    [SerializeField] private TMP_Text energy;
    [SerializeField] private TMP_Text blockEnergy;
    [SerializeField] private TMP_Text towerText;
    
    public void OpenBlockUI()
    {
        blockInfogroup.SetActive(true);
        energy.text = $"Energy {GameManager.instance.selectedBlock.GetEnergy()}";
        blockEnergy.text = $"Max Energy {GameManager.instance.selectedBlock.GetMaxEnergy()}";
        if (GameManager.instance.selectedBlock.tower is not null)
            towerText.text = "Destroy Tower";
        else
            towerText.text = "Build Tower";
    }
      
    public void CloseBlockUI()
    { 
        blockInfogroup.SetActive(false);
    }  

}


