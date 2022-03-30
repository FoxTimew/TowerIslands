using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Barge",menuName = "ScriptableObjects/BargeSO", order = 3)]
public class BargeSO : ScriptableObject
{
    public List<Enemy> troops;
    public float bargeSpeed = 10f;
    public int bargeHP = 100;
    public float rewardModifier = 1f;
    public float waitingTime = 20f;
}
