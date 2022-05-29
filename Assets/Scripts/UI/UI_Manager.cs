using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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

    [SerializeField] private TMP_Text goldUIText;
    [SerializeField] private TMP_Text waveUIText;
    [Header("Transition Reference")]
    [SerializeField] private RectTransform  LeftTransition;
    [SerializeField] private RectTransform  RightTransition;
    [SerializeField] private float transitionDuration;

    [Header("Wave Transition Reference")] 
    public WaveTransitionAnimation waveTransitionObject;

    public WaveTransitionAnimation buildYourDefenseTransitionObject;
    public float waveTransitionTime;
 
    [Header("DynamicButtonsReferences")] 
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private GameObject blockButtonPrefab;

    [SerializeField] private Sprite tickImage;
    [SerializeField] private Sprite redCrossImage;

    [Header("Button Sprites")] public Sprite lockedButtonSprite;
    public Sprite towerButtonSprite;
    public Sprite supportButtonSprite;
    public Sprite trapButtonSprite;
    public Sprite mortarButtonSprite;
    public Sprite upgradeSprite;
    public Sprite repairSprite;
    
    private GameObject tmpButton;
    private TMP_Text tmpButtonText;
    private Image tmpButtonImage;
    private Button tmpPrepareButton;
    private EventTrigger tmpEventTrigger;

    private WaitForSeconds closeMenuTransitionDuration;
    private GameObject tmpChild;
    private ContextMenuLinker linker;
    
    private void Awake()
    {
        Debug.Log("instance");
        instance = this;
    }

    private void Start()
    {
        closeMenuTransitionDuration = new WaitForSeconds(transitionDuration * .5f);
    }
    
    public void CloudTransition()
    {
        LeftTransition.DOLocalMoveX(-50, transitionDuration*0.48f).SetEase(Ease.Unset)
            .OnComplete(()=>LeftTransition.DOScale(Vector3.one, transitionDuration*0.02f)
                .OnComplete(() => LeftTransition.DOLocalMoveX(-800, transitionDuration*0.48f).SetEase(Ease.Unset)));
        RightTransition.DOLocalMoveX(50, transitionDuration*0.48f).SetEase(Ease.Unset)
            .OnComplete(()=>LeftTransition.DOScale(Vector3.one, transitionDuration*0.02f)
                .OnComplete(() => RightTransition.DOLocalMoveX(800, transitionDuration*0.48f).SetEase(Ease.Unset)));
    }
    public void OpenMenu(int menuEnumValue)
    {
        /*Sound*/ AudioManager.instance.Play(21, false);
        if (menuEnumValue != (int)MenuEnum.BlockInfo && menuEnumValue != (int)MenuEnum.FeedbackUI)
        {
            StartCoroutine(OpenMenuWithTransition(menuEnumValue));
        }
        else
        {
            if (menuEnumValue == (int) MenuEnum.BlockInfo)
            {
                blockInfo.SetActive(true);
                linker = blockInfo.transform.GetChild(1).GetComponent<ContextMenuLinker>();
                if (GameManager.instance.selectedBlock.building == null)
                {
                    tmpChild = blockInfo.transform.GetChild(0).gameObject;
                    linker = tmpChild.GetComponent<ContextMenuLinker>();
                    if (!tmpChild.activeSelf)
                    {
                        tmpChild.SetActive(true);
                        linker.LinkListeners(GameManager.instance.selectedBlock);
                    }
                }
                else
                {
                    linker.cma.PlayAnimation();
                    linker.gameObject.SetActive(true);
                    if (GameManager.instance.selectedBlock != null)
                    {
                        linker.LinkListeners(GameManager.instance.selectedBlock);
                    }
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
                foreach (Transform child in islandEditorScroller.transform.GetChild(0).transform)
                {
                    Destroy(child.gameObject);
                }
                islandEditorMenu.SetActive(false);
                break;
            case (MenuEnum.LevelSelectionMenu):
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
                /*Sound*/ AudioManager.instance.Play(17, true, true);
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
                /*Sound*/ AudioManager.instance.Play(2, true);

                break;
            case (MenuEnum.PlayingLevelMenu):
                playingLevelMenu.SetActive(true);
                playingLevelMenu.transform.GetChild(0).gameObject.SetActive(true);
                /*Sound*/ AudioManager.instance.Play(17, true, true);
                

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
                /*Sound*/ AudioManager.instance.StopMusic();
                AudioManager.instance.Play(19, false, true);
                break;
            case (MenuEnum.VictoryMenu):
                //Apeler menu de victoire
                victoryMenu.SetActive(true);
                /*Sound*/ AudioManager.instance.StopMusic();
                AudioManager.instance.Play(18, false, true);
                break;
        }
    }
    IEnumerator OpenMenuWithTransition(int menuID)
    {
        /*Sound*/ AudioManager.instance.Play(20, false);
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
                /*Sound*/ AudioManager.instance.Play(17, true, true);
                break;
            case (MenuEnum.IslandEditorMenu):
                islandEditorMenu.SetActive(true);
                DrawBlockButtons();
                break;
            case (MenuEnum.LevelSelectionMenu):
                levelSelectionMenu.SetActive(true);
                DrawLevelSelectionButton();
                /*Sound*/ AudioManager.instance.Play(17, true, true);
                break;
            case (MenuEnum.LevelPreparationMenu):
                levelPreparationMenu.SetActive(true);
                /*Sound*/AudioManager.instance.Play(2, true);
                Debug.Log("PreparationMenu");
                buildYourDefenseTransitionObject.gameObject.SetActive(true);
                buildYourDefenseTransitionObject.BuildYourDefenseTransition();
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
                /*Sound*/ AudioManager.instance.StopMusic();
                AudioManager.instance.Play(19, false, true);
                break;
            case (MenuEnum.VictoryMenu):
                //Apeler menu de victoire
                victoryMenu.SetActive(true);
                /*Sound*/ AudioManager.instance.StopMusic();
                AudioManager.instance.Play(18, false, true);
                break;
        }
    }

    public void DrawBlockButtons()
    {
        foreach (KeyValuePair<int, int> block in GameManager.instance.islandCreator.blocksCount)
        {
            tmpButton = Instantiate(blockButtonPrefab, islandEditorScroller.transform.GetChild(0));
            tmpButton.name = $"Blocks{block.Key}";
            //tmpButton.GetComponent<BlockButton>().index = block.Key;
            tmpEventTrigger = tmpButton.GetComponent<EventTrigger>();
            tmpButton.GetComponent<BlockButton>().index = block.Key;
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) =>
            {
                GameManager.instance.islandCreator.
                    PopBuild($"Blocks{block.Key}",tmpButton.GetComponent<RectTransform>());
            });
            tmpEventTrigger.triggers.Add(entry);
            tmpButton.transform.GetChild(0).GetComponent<TMP_Text>().text = block.Value.ToString();
            tmpButton.transform.GetChild(1).GetComponent<Image>().sprite =
                GameManager.instance.islandCreator.blockPreviewSprites[block.Key-1];
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

    public bool IsMenuOpen(MenuEnum menu)
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
        //GameManager.instance.levelManager.selectedLevel = tmpButton.GetComponent<LevelButton>().levelContained;
        EconomyManager.instance.SetGold(GameManager.instance.levelManager.selectedLevel.startGold);
    }

    private void EnablePrepareButtonListener()
    {
        tmpPrepareButton.onClick.RemoveAllListeners();
        tmpPrepareButton.onClick.AddListener(OnClickListener);
        tmpPrepareButton.interactable = true;

    }

    public void UpdateWaveUI()
    {

        if (GameManager.instance.levelManager.selectedLevel is null) return;
        waveUIText.text = $"{GameManager.instance.currentWave}/{GameManager.instance.levelManager.selectedLevel.waves.Count}";

    }
    public void UpdateGoldUI(int goldAmount)
    {
        goldUIText.text = $"{goldAmount}";
    }

    public void LaunchWaveClearedTransition()
    {
        Debug.Log("transition intitiated");
        waveTransitionObject.gameObject.SetActive(true);
        waveTransitionObject.WaveClearedTransition();
    }


}
