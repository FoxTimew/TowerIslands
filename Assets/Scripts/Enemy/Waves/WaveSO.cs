using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSO", menuName = "ScriptableObjects/WaveSO", order = 4)]
public class WaveSO : ScriptableObject
{
    public List<BargeSO> barges;
    public int arcanumToGain;
}
