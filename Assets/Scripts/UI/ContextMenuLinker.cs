using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ContextMenuType
{
    BlockEmpty,
    BlockBuilt
}
public class ContextMenuLinker : MonoBehaviour
{
    public ContextMenuType type;
    public CircleMenuAnimation cma;
    public Block blockToHover;
    private RectTransform UIManagerCanvasRect;
    private RectTransform contextMenuRectTransform;
    public Button[] buttons;
    private GameObject tmpChild;
    private Button tmpButton;
    private TowerSO tmpTowerSO;
    private Tower tmpTower;
    public TMP_Text towerCostText;
    public TMP_Text mortarCostText;
    public TMP_Text supportCostText;
    public TMP_Text trapCostText;
    public TMP_Text upgradeCostText;
    public TMP_Text sellCostText;
    public TMP_Text repairCostText;
    

    private void Awake()
    {
        cma = GetComponent<CircleMenuAnimation>();
        UIManagerCanvasRect = UI_Manager.instance.GetComponent<RectTransform>();
        contextMenuRectTransform = GetComponent<RectTransform>();
        
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;
        if (GameManager.instance.selectedBlock is null) return;
        UpdateUIPosition();
        if (type != ContextMenuType.BlockEmpty) return;
    }

    public void LinkListeners(Block block)
    {
        Debug.Log("LinkListeners");
        if (block != null)
        {
            Debug.Log("BlockNotNull");
            blockToHover = block;

            switch (type)
            {
                case (ContextMenuType.BlockEmpty):
                    Debug.Log("Block Empty");
                    foreach (var button in buttons) button.onClick.RemoveAllListeners();
                    Debug.Log("Tower prix update");
                    towerCostText.text = GameManager.instance.rapidTowerSO.goldRequired.ToString();
                    if (GameManager.instance.rapidTowerSO.goldRequired <= EconomyManager.instance.GetGoldAmount())
                    {
                        buttons[0].GetComponent<Image>().sprite = UI_Manager.instance.towerButtonSprite;
                        buttons[0].onClick.AddListener(RapidTowerBuilder);
                        buttons[0].interactable = true;
                    }
                    else
                    {
                        buttons[0].GetComponent<Image>().sprite = UI_Manager.instance.lockedButtonSprite;
                        buttons[0].interactable = false;
                    }
                    Debug.Log("Mortier prix update");
                    mortarCostText.text = GameManager.instance.mortarTowerSO.goldRequired.ToString();
                    if (GameManager.instance.mortarTowerSO.goldRequired <= EconomyManager.instance.GetGoldAmount())
                    {
                        buttons[1].GetComponent<Image>().sprite = UI_Manager.instance.mortarButtonSprite;
                        buttons[1].onClick.AddListener(MortarTowerBuilder);
                        buttons[1].interactable = true;
                    }
                    else
                    {
                        buttons[1].GetComponent<Image>().sprite = UI_Manager.instance.lockedButtonSprite;
                        buttons[1].interactable = false;
                    }
                    Debug.Log("Support prix update");
                    supportCostText.text = GameManager.instance.energySupportSO.goldRequired.ToString();
                    if (GameManager.instance.energySupportSO.goldRequired <= EconomyManager.instance.GetGoldAmount())
                    {
                        buttons[2].GetComponent<Image>().sprite = UI_Manager.instance.supportButtonSprite;
                        buttons[2].onClick.AddListener(EnergySupportTowerBuilder);
                        buttons[2].interactable = true;
                    }
                    else
                    {
                        buttons[2].GetComponent<Image>().sprite = UI_Manager.instance.lockedButtonSprite;
                        buttons[2].interactable = false;
                    }
                    Debug.Log("Trap prix update");
                    trapCostText.text = GameManager.instance.stunTrapSO.goldRequired.ToString();
                    if (GameManager.instance.stunTrapSO.goldRequired <= EconomyManager.instance.GetGoldAmount())
                    {
                        buttons[3].GetComponent<Image>().sprite = UI_Manager.instance.trapButtonSprite;
                        buttons[3].onClick.AddListener(StunTrapTowerBuilder);
                        buttons[3].interactable = true;
                    }
                    else
                    {
                        buttons[3].GetComponent<Image>().sprite = UI_Manager.instance.lockedButtonSprite;
                        buttons[3].interactable = false;
                    }

                    for (int i = 0; i < transform.childCount - 1; i++)
                    {
                        buttons[i].onClick.AddListener(MenuCloserListener);
                    }

                    break;
                case (ContextMenuType.BlockBuilt):
                    //Upgrade button
                    //Si l'upgrade est dispo + on a assez d'argent + la tour n'est pas morte
                    if (GameManager.instance.selectedBlock.building.GetType() == typeof(Tower))
                    {
                        tmpTowerSO = (TowerSO) GameManager.instance.selectedBlock.building.buildingSO;
                        
                        if (tmpTowerSO.level != 2 && tmpTowerSO.nextLevel is not null &&
                            EconomyManager.instance.GetGoldAmount() >= tmpTowerSO.upgradeCost && 
                            GameManager.instance.selectedBlock.building.hp > 0)
                        {
                            buttons[0].GetComponent<Image>().sprite = UI_Manager.instance.upgradeSprite;
                            buttons[0].onClick.RemoveAllListeners();
                            buttons[0].interactable = true;
                            buttons[0].onClick.AddListener(UpgradeBuildingListener);
                        }
                        else if( tmpTowerSO.nextLevel != null && (EconomyManager.instance.GetGoldAmount() < tmpTowerSO.upgradeCost || GameManager.instance.selectedBlock.building.hp <= 0))
                        {
                            buttons[0].GetComponent<Image>().sprite = UI_Manager.instance.lockedButtonSprite;
                            buttons[0].interactable = false;
                            upgradeCostText.text = tmpTowerSO.upgradeCost.ToString();
                        }
                        else
                        {
                            buttons[0].GetComponent<Image>().sprite = UI_Manager.instance.lockedButtonSprite;
                            buttons[0].interactable = false;
                            upgradeCostText.text = "0";
                        }

                        upgradeCostText.text = tmpTowerSO.upgradeCost.ToString();
                    }
                    else
                    {
                        buttons[0].GetComponent<Image>().sprite = UI_Manager.instance.lockedButtonSprite;
                        buttons[0].interactable = false;
                        upgradeCostText.text = "0";
                    }

                    //Sell button
                    buttons[1].onClick.RemoveAllListeners();
                    buttons[1].onClick.AddListener(SellBuildingListener);
                    sellCostText.text = GameManager.instance.selectedBlock.building.IsBuildingDestroyed() ? "3" : GameManager.instance.selectedBlock.building.buildingSO.goldRequired.ToString();
                    //Repair button
                    //Si le repair est dispo + on a assez d'argent 
                    int tmpGoldRequired = GameManager.instance.selectedBlock.building.buildingSO.goldRequired;
                    int tmpMissingHealth = GameManager.instance.selectedBlock.building.buildingSO.healthPoints -
                                           GameManager.instance.selectedBlock.building.hp;
                    int tmpMaxHealth = GameManager.instance.selectedBlock.building.buildingSO.healthPoints;
                    Debug.Log($"Gold required : {tmpGoldRequired}, Missing Health : {tmpMissingHealth}, Max Health : {tmpMaxHealth}");
                    Debug.Log($"Calcul : {tmpGoldRequired*(tmpMissingHealth)/tmpMaxHealth}");
                    if (GameManager.instance.selectedBlock.building.IsBuildingDestroyed() &&
                        EconomyManager.instance.GetGoldAmount() >
                        GameManager.instance.selectedBlock.building.buildingSO.goldRequired *
                        ((GameManager.instance.selectedBlock.building.buildingSO.healthPoints-GameManager.instance.selectedBlock.building.hp)/GameManager.instance.selectedBlock.building.buildingSO.healthPoints) )
                    {
                        Debug.Log($"Gold required : {GameManager.instance.selectedBlock.building.buildingSO.goldRequired}, " +
                                  $"Missing health : {(GameManager.instance.selectedBlock.building.buildingSO.healthPoints-GameManager.instance.selectedBlock.building.hp)}");
                        buttons[2].GetComponent<Image>().sprite = UI_Manager.instance.repairSprite;
                        buttons[2].onClick.RemoveAllListeners();
                        buttons[2].onClick.AddListener(RepairBuildingListener);
                        
                    }
                    else
                    {
                        buttons[2].GetComponent<Image>().sprite = UI_Manager.instance.lockedButtonSprite;
                        buttons[2].interactable = false;
                    }

                    if (GameManager.instance.selectedBlock.building.IsBuildingDestroyed())
                    {
                        repairCostText.text = (GameManager.instance.selectedBlock.building.buildingSO.goldRequired *
                                               ((GameManager.instance.selectedBlock.building.buildingSO.healthPoints-GameManager.instance.selectedBlock.building.hp)/GameManager.instance.selectedBlock.building.buildingSO.healthPoints))
                            .ToString();
                    }
                    else
                    {
                        repairCostText.text = "0";
                    }

                    foreach (Button button in buttons)
                    {
                        button.onClick.AddListener(MenuCloserListener);
                    }

                    break;
            }
        }
    }

    private void RapidTowerBuilder()
    {
        /*Sound*/ AudioManager.instance.Play(11, false);
        Debug.Log("Build Rapid Tower");
        GameManager.instance.selectedBlock.Build(GameManager.instance.rapidTowerSO);
    }

    private void OnEnable()
    {
        cma.PlayAnimation();
    }

    private void MortarTowerBuilder()
    {
        /*Sound*/ AudioManager.instance.Play(11, false);
        Debug.Log("Build Mortar Tower");
        GameManager.instance.selectedBlock.Build(GameManager.instance.mortarTowerSO);
    }

    private void EnergySupportTowerBuilder()
    {
        /*Sound*/ AudioManager.instance.Play(11, false);
        Debug.Log("Build Energy Support");
        GameManager.instance.selectedBlock.Build(GameManager.instance.energySupportSO);
    }
    private void StunTrapTowerBuilder()
    {
        /*Sound*/ AudioManager.instance.Play(11, false);
        Debug.Log("Build Stun Trap");
        GameManager.instance.selectedBlock.Build(GameManager.instance.stunTrapSO);
        
    }

    private void UpgradeBuildingListener()
    {
        Debug.Log("Upgrade tower");
        tmpTower = (Tower)GameManager.instance.selectedBlock.building;
        tmpTower.Upgrade();
    }

    private void RepairBuildingListener()
    {
        Debug.Log("Repair tower");
        /*Sound*/AudioManager.instance.Play(24);
        GameManager.instance.selectedBlock.building.Repair();
        
    }
    private void SellBuildingListener()
    {
        GameManager.instance.selectedBlock.SellBuilding();
        /*Sound*/AudioManager.instance.Play(22);
    }
    private void MenuCloserListener()
    {
        cma.CloseContextMenu();
    }
    

    public void UpdateUIPosition()
    {
        Vector2 ViewportPosition=GameManager.instance.cam.WorldToViewportPoint(GameManager.instance.selectedBlock.transform.position);
        var sizeDelta = UIManagerCanvasRect.sizeDelta;
        Vector2 WorldObject_ScreenPosition=new Vector2(
            ((ViewportPosition.x*sizeDelta.x)-(sizeDelta.x*0.5f)),
            ((ViewportPosition.y*sizeDelta.y)-(sizeDelta.y*0.5f)));

        contextMenuRectTransform.anchoredPosition=WorldObject_ScreenPosition;
    }
}
