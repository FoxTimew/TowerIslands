
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private GameObject blocksGo;
    [SerializeField] private List<Block> blocks;
    
    private Vector3 origin;
    
    [SerializeField] private DragPointer dragPointer;




    void Update()
    {
        blocksGo.transform.position = dragPointer.isSnapped ? dragPointer.snapPosition : transform.position;
    }

    private void OnMouseDown()
    {
        blocksGo.transform.parent = null;
    }
    

    private void OnMouseDrag()
    {
        
        GameManager.instance.cameraZoom.enabled = false;
        origin = GameManager.instance.cam.ScreenToWorldPoint(Input.mousePosition);
        origin.z = 0;
        origin.y += 2 * 2.67f;
        transform.parent.position = origin;
        ChangeSprite(IsPlaceable());
        Debug.Log(IsPlaceable());
    }
    
    private void OnMouseUp()
    {
        blocksGo.transform.parent = transform.parent;
        GameManager.instance.cameraZoom.enabled = true;
        ChangeSprite(IsPlaceable());
        if(IsPlaceable()) PlaceBlock();   
    }

    
    private float RoundTo(float value, float step)
    {
        return Mathf.Round(value/step) * step;
    }
    
    
    bool IsPlaceable()
    {
        bool result = false;
        foreach (var block in blocks)
        {
            if (GameManager.instance.grid.hdvIndex.Contains(block.index)) return false;
            if (Utils.GetAdjacentsIndex(block.index).Count>0) 
                result = true;
        }
        return result;
    }


    void ChangeSprite(bool value)
    {
        var alpha = value ? 1f : 0.5f;
        var color = blocks[0].spriteRenderer.color;
        color.a = alpha;
        foreach (var b in blocks) b.spriteRenderer.color = color;
    }


    void PlaceBlock()
    {
        GameManager.instance.islandCreator.current = null;
        GameManager.instance.islandCreator.currentType = null;
        foreach (var block in blocks)
            GameManager.instance.grid.AddBlock(block);
        GameManager.instance.UpdateBlocks();
        blocksGo.transform.parent = GameManager.instance.blockGroup.transform;
        dragPointer.enabled = false;
        enabled = false;
        transform.parent.gameObject.SetActive(false);
    }
    
    
}
