using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WaveSO", menuName = "ScriptableObjects/WaveSO", order = 3)]
public class WaveSO : ScriptableObject
{
    public List<Enemy> enemiesInWave;
    public int arcanumToGain;
}
