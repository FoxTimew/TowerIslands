using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    
    #region Selected Block parameters
    
    [SerializeField] private GameObject buildingGroup;
    [SerializeField] private GameObject blockInfogroup;
    [SerializeField] private TMP_Text energy;
    [SerializeField] private TMP_Text blockEnergy;
    
    #endregion
    
    
    #region Selected Block Methods
    
    public void OpenBlockUI()
    {
        buildingGroup.SetActive(true);
        blockInfogroup.SetActive(true);
        energy.text = GameManager.instance.selectedBlock.energy.ToString();
        blockEnergy.text = GameManager.instance.selectedBlock.GetMaxEnergy().ToString();
    }
    
    public void CloseBlockUI()
    {
        buildingGroup.SetActive(false);
        blockInfogroup.SetActive(false);
    }
    
    #endregion
}


