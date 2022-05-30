
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    public int index;
    [SerializeField] public GameObject blocksGo;
    [SerializeField] public List<Block> blocks;
    [SerializeField] public GameObject canvas;
    
    private Vector3 origin;
    
    
    [SerializeField] public DragPointer dragPointer;
    
    void Start()
    {
        transform.position = blocksGo.transform.position;
    }
    void Update()
    {
        //dragPointer.enabled = GameManager.instance.editorActivated;
        //enabled= GameManager.instance.editorActivated;;
        blocksGo.transform.position = dragPointer.isSnapped ? dragPointer.snapPosition : transform.position;
    }

    // private void OnMouseDown()
    // {
    //     if (Utils.IsPointerOverUI()) return;
    //     GameManager.instance.cameraZoom.enabled = false;
    // }
    

    private void OnMouseDrag()
    {
        //if (Utils.IsPointerOverUI()) return;
        origin = GameManager.instance.cam.ScreenToWorldPoint(Input.mousePosition);
        origin.z = 0;
        origin.y += 2 * 2.67f;
        /*Sound*/ if (transform.position != origin) AudioManager.instance.Play(26, false, true);
        transform.position = origin;
        ChangeSprite(IsPlaceable());
    }
    
    private void OnMouseUp()
    {
        //GameManager.instance.cameraZoom.enabled = true;
        transform.position = blocksGo.transform.position;
        //blocksGo.transform.parent = transform.parent;
        ChangeSprite(IsPlaceable());
    }


    public bool IsPlaceable()
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


    
    
    
}
