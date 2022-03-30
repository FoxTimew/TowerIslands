
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private Block[] blocks;
    private void Start()
    {
    
        StartCoroutine(WaitForRelease());
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
                pos.y = RoundTo(pos.y, 0.25f);
                pos.x = RoundTo(pos.x, 0.5f);
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
