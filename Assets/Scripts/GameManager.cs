using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private  Block[] baseBlock;
    
    
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
    }
    
    #endregion
    
}
