using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelSO", fileName = "LevelSO", order = 5)]
public class LevelSO : ScriptableObject
{
    public int levelNumber;
    public bool isCompleted = false;
    public List<Wave> waves;
    public int startGold;
    public Drag block;
    
}
