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
    VictoryMenu = 12
}

public class UI_Manager : MonoBehaviour
{
    
    public static UI_Manager instance;

    [Header("Menu References")] [SerializeField]
    private RectTransform  mainMenu;
    [SerializeField] private RectTransform 
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
        victoryMenu;

    [Header("Transition Reference")]
    [SerializeField] private Transform transitionClouds;
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

    public void OpenMenu(int menuEnumValue)
    {
        switch ((MenuEnum)menuEnumValue)
        {
            case (MenuEnum.MainMenu):
                break;
            case (MenuEnum.CreditsMenu):
                break;
            case (MenuEnum.SettingsMenu):
                break;
            case (MenuEnum.IslandMenu):
                break;
            case (MenuEnum.IslandEditorMenu):
                break;
            case (MenuEnum.LevelSelectionMenu):
                break;
            case (MenuEnum.LevelPreparationMenu):
                break;
            case (MenuEnum.PlayingLevelMenu):
                break;
            case (MenuEnum.FeedbackUI):
                break;
            case (MenuEnum.TowerInfoUI):
                break;
            case (MenuEnum.DefeatMenu):
                break;
            case (MenuEnum.VictoryMenu):
                break;
        }
    }

    public void CloseMenu(int menuEnumValue)
    {
        switch ((MenuEnum)menuEnumValue)
        {
            case (MenuEnum.MainMenu):
                break;
            case (MenuEnum.CreditsMenu):
                break;
            case (MenuEnum.SettingsMenu):
                break;
            case (MenuEnum.IslandMenu):
                break;
            case (MenuEnum.IslandEditorMenu):
                break;
            case (MenuEnum.LevelSelectionMenu):
                break;
            case (MenuEnum.LevelPreparationMenu):
                break;
            case (MenuEnum.PlayingLevelMenu):
                break;
            case (MenuEnum.FeedbackUI):
                break;
            case (MenuEnum.TowerInfoUI):
                break;
            case (MenuEnum.DefeatMenu):
                break;
            case (MenuEnum.VictoryMenu):
                break;
        }
    }

    public void TransitionMenu(int menuIDToClose, int menuIDToOpen)
    {
        
    }

    public void CloudTransition()
    {
        transitionClouds.DOMove(Vector3.zero, 
                transitionDuration).OnComplete(() => { transitionClouds.position = -transitionClouds.position;
            });
        
    }
}
