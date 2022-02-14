using System;
using System.Collections;
using System.Collections.Generic;
using Prototype.Scripts;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private int enemyNumber;
    [SerializeField] private float spawnRate;
    [SerializeField] private GameObject[] spawners;
    private int enemyCount = 0;
    private WaitForSeconds waitSpawn;
    private GameObject enemy;

    private void Awake()
    {
        waitSpawn = new WaitForSeconds(spawnRate);
    }

    public IEnumerator StartWave()
    {
        while (enemyCount < enemyNumber)
        {
            foreach (var spawner in spawners)
            {
                enemy = Pooler.instance.Pop("Enemy");
                enemy.transform.parent = null;
                enemy.transform.position = spawner.transform.position;
            }
            yield return waitSpawn;
            enemyCount++;
        }
        enemyCount = 0;
    }
}
