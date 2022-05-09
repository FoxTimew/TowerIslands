
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private Block[] blocks;
    private Vector3 touchStart;
    private Touch touch;
    private Ray2D ray;
    private Vector3 origin;
    private Vector3 pos;
    private RaycastHit2D hit2D;
    private Color color;
    private Vector3 direction;
    private bool isSnapped;
    private Vector3 lastPosition;
    
    public void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            GameManager.instance.cameraZoom.enabled = false;
            touch = Input.GetTouch(0);
        
            origin = GameManager.instance.cam.ScreenToWorldPoint(touch.position);
            origin.y += 2.67f * 2f;
            origin.z = 0;
            lastPosition = transform.GetChild(0).position;
            /*foreach (var block in blocks)
            {
                color = block.spriteRenderer.color;
                color.a = IsPlaceable()? 1 : 0.5f;
                block.spriteRenderer.color = color;
            }
            transform.position = origin;
            if (isSnapped) transform.GetChild(0).position = lastPosition;
            if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("place");
                if (IsPlaceable())
                {
                    PlaceBlock();
                }
                //else GameManager.instance.islandCreator.Depop();
            }*/
            
        }
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.transform.CompareTag("GridElement")) return;
        isSnapped = true;
        transform.GetChild(0).position = other.transform.position;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.transform.CompareTag("GridElement")) return;
        isSnapped = false;
        transform.GetChild(0).localPosition = Vector3.zero;
    }

    
    private float RoundTo(float value, float step)
    {
        return Mathf.Round(value/step) * step;
    }
    
    
    /*bool IsPlaceable()
    {
        bool result = false;
        foreach (var block in blocks)
        {
            if (GameManager.instance.blocks.ContainsKey(Utils.Round(block.transform.position)))
                return false;
            foreach (var vec in block.InitAdjacents())
            {
                if (GameManager.instance.blocks.ContainsKey(Utils.Round(vec)))
                    result = true;
            }
        }
        return result;
    }



    void PlaceBlock()
    {
        GameManager.instance.islandCreator.current = null;
        foreach (var block in blocks)
         GameManager.instance.blocks.Add(Utils.Round(block.transform.position),block);
        foreach (var block in  GameManager.instance.blocks.Values)
            block.UpdateAdjacents();
        transform.parent = GameManager.instance.blockGroup.transform;
        enabled = false;
    }*/
    
}
