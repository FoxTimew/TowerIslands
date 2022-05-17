
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    public int index;
    [SerializeField] private GameObject blocksGo;
    [SerializeField] private List<Block> blocks;
    
    private Vector3 origin;
    
    [SerializeField] private DragPointer dragPointer;
    
    void Start()
    {
        transform.position = blocksGo.transform.position;
    }
    void Update()
    {
        dragPointer.enabled = GameManager.instance.editorActivated;
        enabled= GameManager.instance.editorActivated;;
        blocksGo.transform.position = dragPointer.isSnapped ? dragPointer.snapPosition : transform.position;
    }

    private void OnMouseDown()
    {
        GameManager.instance.cameraZoom.enabled = false;
        blocksGo.transform.parent = null;
    }
    

    private void OnMouseDrag()
    {
        origin = GameManager.instance.cam.ScreenToWorldPoint(Input.mousePosition);
        origin.z = 0;
        origin.y += 2 * 2.67f;
        transform.parent.position = origin;
        ChangeSprite(IsPlaceable());
    }
    
    private void OnMouseUp()
    {
        transform.position = blocksGo.transform.position;
        blocksGo.transform.parent = transform.parent;
        GameManager.instance.cameraZoom.enabled = true;
        ChangeSprite(IsPlaceable());
        if(IsPlaceable()) PlaceBlock();   
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
        blocksGo.transform.parent = transform.parent;
        GameManager.instance.islandCreator.current = null;
        GameManager.instance.islandCreator.currentType = null;
        GameManager.instance.islandCreator.blocksCount[index]--;
        foreach (var block in blocks)
            GameManager.instance.grid.AddBlock(block);
        GameManager.instance.UpdateBlocks();
        blocksGo.transform.parent = GameManager.instance.blockGroup.transform;
        transform.parent.gameObject.SetActive(false);
    }
    
    
}
