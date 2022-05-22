using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWaveButton : MonoBehaviour
{
    void Update()
    {
        gameObject.SetActive(GameManager.instance.currentWave<GameManager.instance.levelManager.selectedLevel.waves.Count);
    }
}
