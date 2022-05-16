using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ContextMenuType
{
    BlockEmpty,
    BlockEmptyTowerChoice,
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
        buttons[0].gameObject.SetActive(GameManager.instance.rapidTowerSO.goldRequired <= EconomyManager.instance.GetGoldAmount());
        buttons[1].gameObject.SetActive(GameManager.instance.energySupportSO.goldRequired <= EconomyManager.instance.GetGoldAmount());
        buttons[2].gameObject.SetActive(GameManager.instance.stunTrapSO.goldRequired <= EconomyManager.instance.GetGoldAmount());

    }

    public void LinkListeners(Block block)
    {
        if (block != null)
        {
            blockToHover = block;

            switch (type)
            {
                case (ContextMenuType.BlockEmpty):
                    
                    foreach (var button in buttons) button.onClick.RemoveAllListeners();
                    buttons[0].onClick.AddListener(RapidTowerBuilder);
                    buttons[1].onClick.AddListener(EnergySupportTowerBuilder);
                    buttons[2].onClick.AddListener(StunTrapTowerBuilder);
                    for (int i = 0; i < transform.childCount-1; i++)
                    {
                        buttons[i].onClick.AddListener(MenuCloserListener);
                    }

                    break;
                case (ContextMenuType.BlockEmptyTowerChoice):
                    for (int i = 1; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).GetComponent<Button>().onClick.AddListener(MenuCloserListener);
                    }
                    break;
                case (ContextMenuType.BlockBuilt):
                    Debug.Log("Block Built");
                    transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                    transform.GetChild(2).GetComponent<Button>().onClick.AddListener(SellBuildingLinstener);
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
        GameManager.instance.selectedBlock.Build(GameManager.instance.rapidTowerSO);
    }

    private void EnergySupportTowerBuilder()
    {
        GameManager.instance.selectedBlock.Build(GameManager.instance.energySupportSO);
    }
    private void StunTrapTowerBuilder()
    {
        GameManager.instance.selectedBlock.Build(GameManager.instance.stunTrapSO);
        
    }

    private void SellBuildingLinstener()
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
