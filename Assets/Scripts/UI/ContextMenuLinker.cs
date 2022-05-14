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
    }

    private void Update()
    {
        UpdateUIPosition();
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

                    break;
                case (ContextMenuType.BlockBuilt):

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
 
        //now you can set the position of the ui element
        
        contextMenuRectTransform.anchoredPosition=WorldObject_ScreenPosition;
    }
}
