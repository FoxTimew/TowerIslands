using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO",menuName = "ScriptableObjects/EnemySO", order = 2)]
public class AXD_EnemySO : ScriptableObject
{

    public string eName;
    
    public float speed;
    
    public int maxHealthPoints;

    public int damage;

    public float attackSpeed;

    public float range;
    
    public int goldToAddOnDeath;
}
