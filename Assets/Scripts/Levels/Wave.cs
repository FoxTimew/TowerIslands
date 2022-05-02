using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WaveSO", menuName = "ScriptableObjects/WaveSO", order = 3)]
public class Wave
{
    public List<BargeSO> bargesInWave;
}
