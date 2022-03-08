using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Manager")] 
    [SerializeField] private UIManager uiManager;
    
    
    
    
    
    
    [SerializeField] private Camera cam;
    [SerializeField] private  Block[] baseBlock;
    

    public Block selectedBlock;
    public Dictionary<Vector3, Block> blocks = new Dictionary<Vector3, Block>();


    #region Unity Methods
    void Awake()
    {
        if (instance != null) 
        { 
            Destroy(gameObject);
            return;
        }
                
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    void Start()
    {
        foreach (var block in baseBlock)
            blocks.Add(block.transform.position,block);
        foreach (var block in blocks.Values)
            block.FindAdjacents();
        StartCoroutine(GameLoop());
    }
    
    
    #endregion


    IEnumerator GameLoop()
    {
        while (true)
        {
            SelectTower();
            yield return null;
        }
    }

    void SelectTower()
    {
        if (Input.touchCount > 0)
        {
            
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = cam.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit))
                {
                    if(selectedBlock is not null) selectedBlock.Deselect();
                    uiManager.CloseBlockUI();
                    if (hit.transform.GetComponent<Block>())
                    {
                        selectedBlock = blocks[hit.transform.position];
                        selectedBlock.Select();
                        uiManager.OpenBlockUI();
                    }
                }
            }
        }
    }


    
}
