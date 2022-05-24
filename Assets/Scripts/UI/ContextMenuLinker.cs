using System;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject tmpChild;
    private Button tmpButton;

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
        if (block != null)
        {
            blockToHover = block;

            switch (type)
            {
                case (ContextMenuType.BlockEmpty):
                    Debug.Log("Block Empty");
                    foreach (var button in buttons) button.onClick.RemoveAllListeners();
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
                    if (GameManager.instance.mortarTowerSO.goldRequired <= EconomyManager.instance.GetGoldAmount())
                    {
                        buttons[1].GetComponent<Image>().sprite = UI_Manager.instance.towerButtonSprite;
                        buttons[1].onClick.AddListener(RapidTowerBuilder);
                        buttons[1].interactable = true;
                    }
                    else
                    {
                        buttons[1].GetComponent<Image>().sprite = UI_Manager.instance.lockedButtonSprite;
                        buttons[1].interactable = false;
                    }
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
                    
                    if (GameManager.instance.energySupportSO.goldRequired <= EconomyManager.instance.GetGoldAmount())
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
                    for (int i = 0; i < transform.childCount-1; i++)
                    {
                        buttons[i].onClick.AddListener(MenuCloserListener);
                    }
                    break;
                case (ContextMenuType.BlockBuilt):
                    Debug.Log("Block Built");
                    //Upgrade button
                    if(GameManager.instance.selectedBlock.building)
                    tmpButton = transform.GetChild(2).GetComponent<Button>();
                    tmpButton.onClick.RemoveAllListeners();
                    tmpButton.onClick.AddListener(SellBuildingListener);
                    //Repair button
                    tmpButton = transform.GetChild(3).GetComponent<Button>();
                    tmpButton.onClick.RemoveAllListeners();
                    tmpButton.onClick.AddListener(RepairBuildingListener);
                    for (int i = 1; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).GetComponent<Button>().onClick.AddListener(MenuCloserListener);
                    }
                    break;
            }
        }
    }

    private void RapidTowerBuilder()
    {
        Debug.Log("Build Rapid Tower");
        GameManager.instance.selectedBlock.Build(GameManager.instance.rapidTowerSO);
    }
    
    private void MortarTowerBuilder()
    {
        Debug.Log("Build Mortar Tower");
        GameManager.instance.selectedBlock.Build(GameManager.instance.mortarTowerSO);
    }

    private void EnergySupportTowerBuilder()
    {
        Debug.Log("Build Energy Support");
        GameManager.instance.selectedBlock.Build(GameManager.instance.energySupportSO);
    }
    private void StunTrapTowerBuilder()
    {
        Debug.Log("Build Stun Trap");
        GameManager.instance.selectedBlock.Build(GameManager.instance.stunTrapSO);
        
    }

    private void RepairBuildingListener()
    {
        Debug.Log("Build Stun Trap");
        
        
    }
    private void SellBuildingListener()
    {
        GameManager.instance.selectedBlock.SellBuilding();
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
