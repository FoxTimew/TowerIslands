using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;



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
        levelPreparationMenu,
        playingLevelMenu,
        feedbackUI,
        towerInfoUI,
        defeatMenu,
        victoryMenu,
        blockInfo,
        islandEditorScroller,
        levelSelectionScroller;
        

    [Header("Transition Reference")]
    [SerializeField] private RectTransform  LeftTransition;
    [SerializeField] private RectTransform  RightTransition;
    [SerializeField] private RectTransform transitionClouds;
    [SerializeField] private float transitionDuration;
    private Vector3 initialCloudPosition;

    [Header("DynamicButtonsReferences")] 
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private GameObject blockButtonPrefab;

    [SerializeField] private Sprite tickImage;
    [SerializeField] private Sprite redCrossImage;
    private GameObject tmpButton;
    private TMP_Text tmpButtonText;
    private Image tmpButtonImage;
    private Button tmpPrepareButton;
    private EventTrigger tmpEventTrigger;

    private WaitForSeconds closeMenuTransitionDuration;
    
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
        closeMenuTransitionDuration = new WaitForSeconds(transitionDuration * .5f);
    }
    
    public void CloudTransition()
    {
        //transitionClouds.DOMove(-transitionClouds.position/3, 
        //        transitionDuration).OnComplete(() => { transitionClouds.position = initialCloudPosition;
        //    });

        LeftTransition.DOLocalMoveX(-50, transitionDuration*0.5f).SetEase(Ease.Unset)
            .OnComplete(() => LeftTransition.DOLocalMoveX(-800, transitionDuration*0.5f).SetEase(Ease.Unset));
        RightTransition.DOLocalMoveX(50, transitionDuration*0.5f).SetEase(Ease.Unset)
            .OnComplete(() => RightTransition.DOLocalMoveX(800, transitionDuration*0.5f).SetEase(Ease.Unset));
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
                if (GameManager.instance.selectedBlock.building == null)
                {
                    blockInfo.transform.GetChild(0).GetComponent<ContextMenuLinker>().cma.PlayAnimation();
                    if (!blockInfo.transform.GetChild(0).gameObject.activeSelf)
                    {
                        blockInfo.transform.GetChild(0).gameObject.SetActive(true);
                        blockInfo.transform.GetChild(0).GetComponent<ContextMenuLinker>().LinkListeners(GameManager.instance.selectedBlock);
                    }
                    /*else
                    {
                        // A Ajouter pour activer le menu sur 2 niveaux
                        blockInfo.transform.GetChild(1).gameObject.SetActive(true);
                    }*/
                }
                else
                {
                    ContextMenuLinker linker = blockInfo.transform.GetChild(2).GetComponent<ContextMenuLinker>();
                    linker.cma.PlayAnimation();
                    linker.gameObject.SetActive(true);
                    linker.LinkListeners(GameManager.instance.selectedBlock);
                }
            }else
            {
                feedbackUI.SetActive(true);
            }
        }
    }


    public void CloseMenuWithoutTransition(int menuEnumValue)
    {
        switch ((MenuEnum)menuEnumValue)
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
                //Détruire tous les boutons générés lors de l'ouverture du menu.
                foreach (Transform child in islandEditorScroller.transform.GetChild(0).transform)
                {
                    Destroy(child.gameObject);
                }
                islandEditorMenu.SetActive(false);
                break;
            case (MenuEnum.LevelSelectionMenu):
                //Détruire tous les boutons générés lors de l'ouverture du menu.
                foreach (Transform child in levelSelectionScroller.transform.GetChild(0).transform)
                {
                    Destroy(child.gameObject);
                }
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

        }
    }
    public void CloseMenu(int menuEnumValue)
    {

        if (menuEnumValue == (int) MenuEnum.DefeatMenu)
        {
            defeatMenu.GetComponent<DefeatMenu>().Close();
        }
        else if (menuEnumValue != (int) MenuEnum.BlockInfo && menuEnumValue != (int) MenuEnum.FeedbackUI)
        {
            StartCoroutine(CloseMenuWithTransition(menuEnumValue));
        }
        
        else
        {
            if (menuEnumValue == (int) MenuEnum.BlockInfo)
            {
                for (int i = 0; i < blockInfo.transform.childCount; i++)
                {
                    if (blockInfo.activeSelf)
                    {
                        blockInfo.transform.GetChild(i)?.GetComponent<CircleMenuAnimation>()?.CloseContextMenu();
                    }
                }
                //blockInfo.SetActive(false);
            }else
            {
                feedbackUI.SetActive(false);
            }
        }
    }

    IEnumerator CloseMenuWithTransition(int menuID)
    {
        CloudTransition();
        yield return closeMenuTransitionDuration;
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
                //Détruire tous les boutons générés lors de l'ouverture du menu.
                foreach (Transform child in islandEditorScroller.transform.GetChild(0).transform)
                {
                    Destroy(child.gameObject);
                }
                islandEditorMenu.SetActive(false);
                break;
            case (MenuEnum.LevelSelectionMenu):
                //Détruire tous les boutons générés lors de l'ouverture du menu.
                foreach (Transform child in levelSelectionScroller.transform.GetChild(0).transform)
                {
                    Destroy(child.gameObject);
                }
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

        }
    }

    public void OpenMenuWithoutTransition(int menuID)
    {
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
                DrawBlockButtons();
                break;
            case (MenuEnum.LevelSelectionMenu):
                levelSelectionMenu.SetActive(true);
                DrawLevelSelectionButton();
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
                //Appeler menu de défaite
                defeatMenu.SetActive(true);
                break;
            case (MenuEnum.VictoryMenu):
                //Apeler menu de victoire
                victoryMenu.SetActive(true);
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
                DrawBlockButtons();
                break;
            case (MenuEnum.LevelSelectionMenu):
                levelSelectionMenu.SetActive(true);
                DrawLevelSelectionButton();
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
                //Appeler menu de défaite
                defeatMenu.SetActive(true);
                break;
            case (MenuEnum.VictoryMenu):
                //Apeler menu de victoire
                victoryMenu.SetActive(true);
                break;
        }
    }

    public void DrawBlockButtons()
    {
        foreach (KeyValuePair<string, int> block in GameManager.instance.islandCreator.blocksCount)
        {
            tmpButton = Instantiate(blockButtonPrefab, islandEditorScroller.transform.GetChild(0));
            tmpButton.name = block.Key;
            tmpEventTrigger = tmpButton.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) =>
            {
                GameManager.instance.islandCreator.PopBuild(block.Key,tmpButton.GetComponent<RectTransform>());
            });
            tmpEventTrigger.triggers.Add(entry);
            tmpButton.transform.GetChild(0).GetComponent<TMP_Text>().text = block.Value.ToString();
        }
    }

    public void DrawLevelSelectionButton()
    {
        
        foreach (LevelSO level in GameManager.instance.levelManager.levels)
        {
            tmpButton = Instantiate(levelButtonPrefab, levelSelectionScroller.transform.GetChild(0));
            tmpPrepareButton = levelSelectionMenu.transform.GetChild(2).GetComponent<Button>();
            tmpPrepareButton.interactable = false;
            
            tmpButton.GetComponent<Button>().onClick.AddListener(EnablePrepareButtonListener);
            
            tmpButtonText = tmpButton.transform.GetChild(0).GetComponent<TMP_Text>();
            tmpButtonImage = tmpButton.transform.GetChild(1).GetComponent<Image>();
            tmpButton.GetComponent<LevelButton>().levelContained = level;
            if (tmpButtonText != null)
            {
                tmpButtonText.GetComponent<TMP_Text>().text += " " + level.levelNumber;
            }

            
            if (tmpButtonImage != null)
            {
                tmpButtonImage.sprite = level.isCompleted ? tickImage : redCrossImage;
            }
        }
    }

    public bool isMenuOpen(MenuEnum menu)
    {
        switch (menu)
        {
            case (MenuEnum.MainMenu):
                return mainMenu.gameObject.activeSelf;
            case (MenuEnum.CreditsMenu):
                return creditsMenu.gameObject.activeSelf;
            case (MenuEnum.SettingsMenu):
                return settingsMenu.gameObject.activeSelf;
            case (MenuEnum.IslandMenu):
                return islandMenu.gameObject.activeSelf;
            case (MenuEnum.IslandEditorMenu):
                return islandEditorMenu.gameObject.activeSelf;
            case (MenuEnum.LevelSelectionMenu):
                return levelSelectionMenu.gameObject.activeSelf;
            case (MenuEnum.LevelPreparationMenu):
                return levelPreparationMenu.gameObject.activeSelf;
            case (MenuEnum.PlayingLevelMenu):
                return playingLevelMenu.gameObject.activeSelf;
            case (MenuEnum.FeedbackUI):
                return feedbackUI.gameObject.activeSelf;
            case (MenuEnum.TowerInfoUI):
                return towerInfoUI.gameObject.activeSelf;
            case (MenuEnum.DefeatMenu):
                return defeatMenu.gameObject.activeSelf;
            case (MenuEnum.VictoryMenu):
                return victoryMenu.gameObject.activeSelf;
            case(MenuEnum.BlockInfo):
                return blockInfo.gameObject.activeSelf;
            default:
                return false;
        }
    }
    private void OnClickListener()
    {
        GameManager.instance.levelManager.selectedLevel = tmpButton.GetComponent<LevelButton>().levelContained;
        EconomyManager.instance.SetGold(GameManager.instance.levelManager.selectedLevel.startGold);
    }

    private void EnablePrepareButtonListener()
    {
        tmpPrepareButton.onClick.RemoveAllListeners();
        tmpPrepareButton.onClick.AddListener(OnClickListener);
        tmpPrepareButton.interactable = true;

    }
    

}
