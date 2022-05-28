using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWaveButton : MonoBehaviour
{
    void Update()
    {
        gameObject.SetActive(GameManager.instance.waveCount>0);
    }
}
