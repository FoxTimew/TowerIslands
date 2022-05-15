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
    private CircleMenuAnimation cma;
    private Block blockToHover;
    private RectTransform UIManagerCanvasRect;
    private RectTransform contextMenuRectTransform;
    private void Start()
    {
        cma = GetComponent<CircleMenuAnimation>();
        UIManagerCanvasRect = UI_Manager.instance.GetComponent<RectTransform>();
        contextMenuRectTransform = GetComponent<RectTransform>();
        blockToHover = GameManager.instance.selectedBlock;
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            UpdateUIPosition();
        }
    }

    public void LinkListeners(Block block)
    {
        if (block != null)
        {
            blockToHover = block;

            switch (type)
            {
                case (ContextMenuType.BlockEmpty):

                    transform.GetChild(1).GetComponent<Button>().onClick.AddListener(RapidTowerBuilder);
                    transform.GetChild(2).GetComponent<Button>().onClick.AddListener(DefenseSupportTowerBuilder);
                    transform.GetChild(3).GetComponent<Button>().onClick.AddListener(StunTrapTowerBuilder);
                    for (int i = 1; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).GetComponent<Button>().onClick.AddListener(MenuCloserListener);
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

    private void DefenseSupportTowerBuilder()
    {
        GameManager.instance.selectedBlock.Build(GameManager.instance.defenseSupportSO);
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
        Vector2 ViewportPosition=GameManager.instance.cam.WorldToViewportPoint(blockToHover.transform.position);
        var sizeDelta = UIManagerCanvasRect.sizeDelta;
        Vector2 WorldObject_ScreenPosition=new Vector2(
            ((ViewportPosition.x*sizeDelta.x)-(sizeDelta.x*0.5f)),
            ((ViewportPosition.y*sizeDelta.y)-(sizeDelta.y*0.5f)));

        contextMenuRectTransform.anchoredPosition=WorldObject_ScreenPosition;
    }
}
