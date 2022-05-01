
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private Block[] blocks;
    [SerializeField] private LayerMask layerMask;
    private void Start()
    {
    
        
    }

    private Touch touch;
    private Ray2D ray;
    private Vector3 origin;
    private Vector3 pos;
    private RaycastHit2D hit2D;
    private Color color;
    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended) return;
            origin = GameManager.instance.cam.ScreenToWorldPoint(touch.position);
            origin.z = 0;
            origin.y += 2.67f*2f;
            if (touch.phase != TouchPhase.Moved) return;
            hit2D = Physics2D.Raycast(origin, Vector2.zero,layerMask);
            if (hit2D.collider != null)
            {
                if (hit2D.transform.CompareTag("GridElement"))
                {
                    pos = hit2D.transform.position;
                    transform.position = pos;
                    if (IsPlaceable())
                    {
                        foreach (var block in blocks)
                        {
                            color = block.spriteRenderer.color;
                            color.a = 1f;
                            block.spriteRenderer.color = color;
                        }
                    }
                    else
                    {
                        foreach (var block in blocks)
                        {
                            color = block.spriteRenderer.color;
                            color.a = 0.5f;
                            block.spriteRenderer.color = color;
                        }
                    }

                }
            }
            else
            {
                transform.position = origin;
            }
            

        }
        
    }
    
    private IEnumerator WaitForRelease()
    {
        var place = false;
        Color color;
        yield return new WaitForSeconds(0.1f);
        while (Input.touchCount == 0) yield return null;
        while (gameObject.activeSelf)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 pos = GameManager.instance.cam.ScreenToWorldPoint(touch.position);
                pos.z = 0;
                transform.position = pos;
                place = IsPlaceable();
                switch (touch.phase)
                {
                    case TouchPhase.Moved when place:
                    {
                        foreach (var block in blocks)
                        {
                            color = block.spriteRenderer.color;
                            color.a = 1f;
                            block.spriteRenderer.color = color;
                        }
                        break;
                    }
                    case TouchPhase.Moved:
                    {
                        foreach (var block in blocks)
                        { 
                            color = block.spriteRenderer.color; 
                            color.a = 0.5f; 
                            block.spriteRenderer.color = color;
                        }
                        break;
                    }
                    case TouchPhase.Ended when place:
                    {
                        PlaceBlock();
                        yield break;
                    }
                }
            }
            yield return null;
        }

        yield return null;

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
            if (GameManager.instance.blocks.ContainsKey(block.transform.position))
                return false;
            foreach (var vec in block.InitAdjacents())
            {
                if (GameManager.instance.blocks.ContainsKey(vec))
                    result = true;
            }
        }
        return result;
    }



    void PlaceBlock()
    {
        GameManager.instance.islandCreator.current = null;
        foreach (var block in blocks)
         GameManager.instance.blocks.Add(block.transform.position,block);
        foreach (var block in  GameManager.instance.blocks.Values)
            block.UpdateAdjacents();
        transform.parent = GameManager.instance.blockGroup.transform;
        enabled = false;
    }
    
}
