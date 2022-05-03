using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildButton : MonoBehaviour
{
    public TMP_Text text;
    public BuildingSO buildingSO;
    public void Build()
    {
        if (GameManager.instance.selectedBlock.building is not null)
        {
            
            GameManager.instance.selectedBlock.DestroyBuilding();
            GameManager.instance.levelManager.OpenBlockUI();
        }
        else
        {
            
            GameManager.instance.selectedBlock.Build(buildingSO);
            GameManager.instance.levelManager.OpenBlockUI();
        }
    }

    private void Update()
    {
        if (GameManager.instance.selectedBlock is null) return;
        if (GameManager.instance.selectedBlock.building is not null)
            text.text = $"Destroy : {GameManager.instance.selectedBlock.building.buildingSO.goldRequired}";
        else
            text.text = $"Create {buildingSO.name} : {buildingSO.goldRequired}";

    }
}
