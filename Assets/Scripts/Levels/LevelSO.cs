using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelSO", fileName = "LevelSO", order = 5)]
public class LevelSO : ScriptableObject
{
    public List<Wave> waves;
}
