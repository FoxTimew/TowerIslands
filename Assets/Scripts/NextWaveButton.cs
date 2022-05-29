using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class NextWaveButton : MonoBehaviour
{
    void Update()
    {
        gameObject.SetActive(GameManager.instance.waveCount > 0);
        gameObject.SetActive(GameManager.instance.currentWave < GameManager.instance.levelManager.selectedLevel.waves.Count);
    }
}
