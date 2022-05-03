using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildButton : MonoBehaviour
{
    public List<GameObject> gos;
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
        {
            foreach (var go in gos)
            {
                go.SetActive(false);
            }
            text.text = $"Destroy : {GameManager.instance.selectedBlock.building.buildingSO.goldRequired}";
        }

        else
        {
            foreach (var go in gos)
            {
                go.SetActive(true);
            }
            text.text = $"Create {buildingSO.name} : {buildingSO.goldRequired}";
        }
    }
}
