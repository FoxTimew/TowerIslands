using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private Material initMat;
    [SerializeField] private Material cantPlaceMat;
    [SerializeField] private Material canPlaceMat;
    [SerializeField] private Block[] blocks;

    private void Update()
    {
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            
            if(Physics.Raycast(ray,out RaycastHit hit))
            {
                float posX  = Mathf.Round(hit.point.x);
                float posZ = Mathf.Round(hit.point.z);
                transform.position = new Vector3(posX, 0, posZ);
            }
            IsPlaceable();
            if (touch.phase == TouchPhase.Ended)
            {
                if (IsPlaceable() == 2)
                {
                    foreach (var block in blocks)
                        GameManager.instance.blocks.Add(block.transform.position,block);
                    foreach (var block in blocks)
                        block.AddToAdjacents();
                    enabled = false;
                }
                else
                {
                    Pooler.instance.Depop("Tetris",gameObject);
                }
            }

           
        }
    }

    private int state = 0;
    int IsPlaceable()
    {
        state = 0;
        foreach (var block in blocks)
        {
            if(GameManager.instance.blocks.ContainsKey(block.transform.position))
            {
                return 1;
            }
            foreach (var vec in block.InitAdjacents())
            {
                if (GameManager.instance.blocks.ContainsKey(vec))
                {
                    state = 2;
                }
            }
        }
        return state;
    }
    
}
