using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingSO buildingSO;
    public int hp { get; protected set; }

    public delegate void TakeDamage(int dmg);

    public TakeDamage takeDamage;


    private void Start()
    {
        takeDamage += BaseTakeDamage;
    }

    public void BaseTakeDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log(hp);
        if (hp <= 0) Pooler.instance.Depop(buildingSO.name,gameObject);
    }
}
