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

        if (energy != null)
        {
            energy.text = $"Energy {GameManager.instance.selectedBlock.energy}";
        }

        if (blockEnergy != null)
        {
            blockEnergy.text = $"Max Energy {GameManager.instance.selectedBlock.GetMaxEnergy()}";
        }

        if (GameManager.instance.selectedBlock.building is not null)
        {
            if (destroyGroup !!= null)
            {
                destroyGroup.SetActive(true);
            }

            if( buildGroup != null){
                buildGroup.SetActive(false);
            }
        }
        else
        {
            if (destroyGroup !!= null)
            {
                destroyGroup.SetActive(false);
            }

            if( buildGroup != null){
                buildGroup.SetActive(true);
            }
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


