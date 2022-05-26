using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public LevelSO levelContained;

    public void SetLevel()
    {
        GameManager.instance.levelManager.selectedLevel = levelContained;
        /*Sound*/ AudioManager.instance.Play(21);
    }
}
