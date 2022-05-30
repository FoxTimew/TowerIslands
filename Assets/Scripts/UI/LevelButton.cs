using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelButton : MonoBehaviour
{
    public LevelSO levelContained;
    public Sprite[] sprites = new Sprite[2];
    public Image image;
    public void SetLevel()
    {
        
        GameManager.instance.selectedLevelButton = this;
        GameManager.instance.levelManager.selectedLevel = levelContained;
        /*Sound*/ AudioManager.instance.Play(21);
    }

    void Update()
    {
        image.sprite = GameManager.instance.selectedLevelButton == this ? sprites[1] : sprites[0];
    }
}
