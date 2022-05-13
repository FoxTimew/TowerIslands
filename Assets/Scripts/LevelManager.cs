using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    [SerializeField] private GameObject blockInfogroup;
    [SerializeField] private GameObject buildGroup;
    [SerializeField] private GameObject destroyGroup;
    [SerializeField] private TMP_Text energy;
    [SerializeField] private TMP_Text blockEnergy;
    public LevelSO selectedLevel;
    public List<LevelSO> levels;
    
    public void OpenBlockUI()
    {
        if (blockInfogroup != null)
        {
            blockInfogroup.SetActive(true);
        }

        energy.text = $"Energy {GameManager.instance.selectedBlock.energy}";
        blockEnergy.text = $"Max Energy {GameManager.instance.selectedBlock.GetMaxEnergy()}";
        if (GameManager.instance.selectedBlock.building is not null)
        {
            destroyGroup.SetActive(true);
            buildGroup.SetActive(false);
        }
        else
        {
            destroyGroup.SetActive(false);
            buildGroup.SetActive(true);
        }
            
        
    }
    
    public void CloseBlockUI()
    {
        if (blockInfogroup != null)
        {
            blockInfogroup.SetActive(false);
        }

        
    }  

}


