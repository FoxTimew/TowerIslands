using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum MenuEnum
{
    MainMenu = 1,
    CreditsMenu = 2,
    SettingsMenu = 3,
    IslandMenu = 4,
    IslandEditorMenu = 5,
    LevelSelectionMenu = 6,
    LevelPreparationMenu = 7,
    PlayingLevelMenu = 8,
    FeedbackUI = 9,
    TowerInfoUI = 10,
    DefeatMenu =11,
    VictoryMenu = 12,
    BlockInfo = 13
}

public class UI_Manager : MonoBehaviour
{
    
    public static UI_Manager instance;

    [Header("Menu References")] 
    [SerializeField] private GameObject  mainMenu;

    [SerializeField] private GameObject
        creditsMenu,
        settingsMenu,
        islandMenu,
        islandEditorMenu,
        levelSelectionMenu,
        playingLevelMenu,
        levelPreparationMenu,
        feedbackUI,
        towerInfoUI,
        defeatMenu,
        victoryMenu,
        blockInfo,
        islandEditorScroller,
        levelSelectionScroller;
        

    [Header("Transition Reference")]
    [SerializeField] private RectTransform transitionClouds;
    [SerializeField] private float transitionDuration;
    private Vector3 initialCloudPosition;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        initialCloudPosition = transitionClouds.position;
    }
    
    public void CloudTransition()
    {
        transitionClouds.DOMove(-transitionClouds.position/3, 
                transitionDuration).OnComplete(() => { transitionClouds.position = initialCloudPosition;
            });
        
    }
    public void OpenMenu(int menuEnumValue)
    {
        if (menuEnumValue != (int)MenuEnum.BlockInfo && menuEnumValue != (int)MenuEnum.FeedbackUI)
        {
            StartCoroutine(OpenMenuWithTransition(menuEnumValue));
        }
        else
        {
            if (menuEnumValue == (int) MenuEnum.BlockInfo)
            {
                blockInfo.SetActive(true);
            }else
            {
                feedbackUI.SetActive(true);
            }
        }
    }

    public void CloseMenu(int menuEnumValue)
    {
        if (menuEnumValue != (int) MenuEnum.BlockInfo && menuEnumValue != (int) MenuEnum.FeedbackUI)
        {
            StartCoroutine(CloseMenuWithTransition(menuEnumValue));
        }
        else
        {
            if (menuEnumValue == (int) MenuEnum.BlockInfo)
            {
                blockInfo.SetActive(false);
            }else
            {
                feedbackUI.SetActive(false);
            }
        }
    }

    IEnumerator CloseMenuWithTransition(int menuID)
    {
        CloudTransition();
        yield return new WaitForSeconds(transitionDuration / 2);
        switch ((MenuEnum)menuID)
        {
            case (MenuEnum.MainMenu):
                mainMenu.SetActive(false);
                break;
            case (MenuEnum.CreditsMenu):
                creditsMenu.SetActive(false);
                break;
            case (MenuEnum.SettingsMenu):
                settingsMenu.SetActive(false);
                break;
            case (MenuEnum.IslandMenu):
                islandMenu.SetActive(false);
                break;
            case (MenuEnum.IslandEditorMenu):
                islandEditorMenu.SetActive(false);
                break;
            case (MenuEnum.LevelSelectionMenu):
                levelSelectionMenu.SetActive(false);
                break;
            case (MenuEnum.LevelPreparationMenu):
                levelPreparationMenu.SetActive(false);
                break;
            case (MenuEnum.PlayingLevelMenu):
                playingLevelMenu.SetActive(false);
                break;
            case (MenuEnum.FeedbackUI):
                feedbackUI.SetActive(false);
                break;
            case (MenuEnum.TowerInfoUI):
                towerInfoUI.SetActive(false);
                break;
            case (MenuEnum.DefeatMenu):
                defeatMenu.SetActive(false);
                break;
            case (MenuEnum.VictoryMenu):
                victoryMenu.SetActive(false);
                break;
            case (MenuEnum.BlockInfo):
                for (int i = 0; i < blockInfo.transform.childCount; i++)
                {
                    blockInfo.transform.GetChild(i).GetComponent<CircleMenuAnimation>().CloseContextMenu();
                }
                blockInfo.SetActive(false);
                break;
        }
    }
    IEnumerator OpenMenuWithTransition(int menuID)
    {
        yield return new WaitForSeconds(transitionDuration / 2);
        switch ((MenuEnum)menuID)
        {
            case (MenuEnum.MainMenu):
                mainMenu.SetActive(true);
                break;
            case (MenuEnum.CreditsMenu):
                creditsMenu.SetActive(true);
                break;
            case (MenuEnum.SettingsMenu):
                settingsMenu.SetActive(true);
                break;
            case (MenuEnum.IslandMenu):
                islandMenu.SetActive(true);
                break;
            case (MenuEnum.IslandEditorMenu):
                islandEditorMenu.SetActive(true);
                break;
            case (MenuEnum.LevelSelectionMenu):
                levelSelectionMenu.SetActive(true);
                break;
            case (MenuEnum.LevelPreparationMenu):
                levelPreparationMenu.SetActive(true);
                break;
            case (MenuEnum.PlayingLevelMenu):
                playingLevelMenu.SetActive(true);
                break;
            case (MenuEnum.FeedbackUI):
                feedbackUI.SetActive(true);
                break;
            case (MenuEnum.TowerInfoUI):
                towerInfoUI.SetActive(true);
                break;
            case (MenuEnum.DefeatMenu):
                defeatMenu.SetActive(true);
                break;
            case (MenuEnum.VictoryMenu):
                victoryMenu.SetActive(true);
                break;
            case (MenuEnum.BlockInfo):
                blockInfo.SetActive(true);
                if (GameManager.instance.selectedBlock.building == null)
                {
                    if (!transform.GetChild(0).gameObject.activeSelf)
                    {
                        transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
                else
                {
                    transform.GetChild(2).gameObject.SetActive(true);
                }
                break;
        }
    }


}
