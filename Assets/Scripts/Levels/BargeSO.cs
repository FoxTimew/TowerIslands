using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Troop
{
    public Enemy enemy;
    public int cristalToEarn;
}
[Serializable]
[CreateAssetMenu(fileName = "Barge",menuName = "ScriptableObjects/BargeSO", order = 3)]
public class BargeSO : ScriptableObject
{
    public List<Troop> troops = new List<Troop>();
    public float bargeSpeed = 10f;
    public int bargeHP = 100;
    public float rewardModifier = 1f;
    public float waitingTime = 20f;
}
