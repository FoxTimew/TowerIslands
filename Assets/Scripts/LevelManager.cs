using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    [SerializeField] private GameObject blockInfogroup;
    [SerializeField] private TMP_Text energy;
    [SerializeField] private TMP_Text blockEnergy;

    public void OpenBlockUI()
      {
          blockInfogroup.SetActive(true);
          energy.text = $"Energy {GameManager.instance.selectedBlock.energy}";
          blockEnergy.text = $"Max Energy {GameManager.instance.selectedBlock.GetMaxEnergy()}";
      }
      
      public void CloseBlockUI()
      {
          blockInfogroup.SetActive(false);
      }  

}


