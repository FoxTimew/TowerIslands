using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Physical = 0,
    Magical = 1
}

public enum TargetPriority
{
    FirstToCome = 0
}
public abstract class AXD_BuildingSO : ScriptableObject
{
    public  int healthPoints;
    public int level;
    public int energyRequired;
    public int goldToUpgrade;
}
