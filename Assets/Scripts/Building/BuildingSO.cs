using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum BuildingType
{
    Tower,Support,Trap
}
[CreateAssetMenu(fileName = "BuildingSO", menuName = "ScriptableObjects/BuildingSO", order = 1)]
public class BuildingSO : ScriptableObject
{
    public string bName;
    public BuildingType type;
    public  int healthPoints;
    public int energyRequired;
    public int goldRequired;
}
