using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    [SerializeField] private RectTransform 
        mainMenu,
        creditsMenu,
        settingsMenu,
        islandMenu,
        islandEditorMenu,
        levelSelectionMenu,
        playingLevelMenu,
        levelPreparationMenu,
        feedbackUI,
        defeatMenu,
        victoryMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void OpenMainMenu()
    {
        
    }

    public void CloseMainMenu()
    {
        
    }
    public void OpenCreditsMenu()
    {
        
    }

    public void CloseCreditsMenu()
    {
        
    }
    public void OpenSettingsMenu()
    {
        
    }

    public void CloseSettingsMenu()
    {
        
    }
    public void OpenIslandMenu()
    {
        
    }

    public void CloseIslandMenu()
    {
        
    }
    public void OpenIslandEditorMenu()
    {
        
    }

    public void CloseIslandEditorMenu()
    {
        
    }
    public void OpenLevelSelectionMenu()
    {
        
    }

    public void CloseLevelSelectionMenu()
    {
        
    }
    public void OpenFeedbackUIMenu()
    {
        
    }

    public void CloseFeedbackUIMenu()
    {
        
    }
    public void OpenPlayLevelMenu()
    {
        
    }

    public void ClosePlayLevelMenu()
    {
        
    }
    public void OpenDefeatMenu()
    {
        
    }

    public void CloseDefeatMenu()
    {
        
    }
    public void OpenVictoryMenu()
    {
        
    }

    public void CloseVictoryMenu()
    {
        
    }
    public void OpenLevelPreparationMenu()
    {
        
    }

    public void CloseLevelPreparationMenu()
    {
        
    }
}
