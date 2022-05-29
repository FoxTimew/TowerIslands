using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    public LevelSO selectedLevel;
    public List<LevelSO> levels;

    void Awake()
    {
        foreach (var levelSO in levels)
        {
            levelSO.isCompleted = false;
        }
    }

}


