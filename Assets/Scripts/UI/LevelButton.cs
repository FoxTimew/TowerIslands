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
        if(GameManager.instance.selectedLevelButton is not null )GameManager.instance.selectedLevelButton.image.sprite = sprites[0];
        image.sprite = sprites[1];
        GameManager.instance.selectedLevelButton = this;
        GameManager.instance.levelManager.selectedLevel = levelContained;
        /*Sound*/ AudioManager.instance.Play(21);
    }
}
