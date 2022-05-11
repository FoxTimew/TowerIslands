using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    
    public Index index;
    public BuildingSO buildingSO;
    public int hp { get; protected set; }

    public delegate void TakeDamage(int dmg);

    public TakeDamage takeDamage;


    private void Start()
    {
        hp = buildingSO.healthPoints;
        takeDamage += BaseTakeDamage;
    }

    public void BaseTakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0) Pooler.instance.Depop(buildingSO.bName,gameObject);
    }
}
