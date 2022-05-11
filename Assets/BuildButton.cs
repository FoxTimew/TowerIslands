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
        GameManager.instance.selectedBlock.Build(buildingSO);
        GameManager.instance.levelManager.OpenBlockUI();
    }

    private void Update()
    {
        text.text = $"Create {buildingSO.name} : {buildingSO.goldRequired}";
    }
}
