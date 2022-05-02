using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class CityCenter : MonoBehaviour
{
    [SerializeField] private int maxHealth = 200;
    [HideInInspector] public int health;

    void Start()
    {
        health = maxHealth;
    }
    
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }
}
