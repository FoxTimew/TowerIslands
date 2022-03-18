using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private Block[] blocks;

    private void OnEnable()
    {
        Debug.Log("enable");
        StartCoroutine(WaitForRelease());
    }

    private IEnumerator WaitForRelease()
    {
        while (Input.touchCount == 0) yield return null;
        while (gameObject.activeSelf)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                pos.y = RoundTo(pos.y, 0.25f);
                pos.x = RoundTo(pos.x, 0.5f);
                pos.z = 0;
                transform.position = pos;
                yield return null;
            }
            yield return null;
        }
    }

    private float RoundTo(float value, float step)
    {
        return Mathf.Round(value/step) * step;
    }
    

    private int state = 0;
    int IsPlaceable()
    {
        state = 0;
        foreach (var block in blocks)
        {
            if(GameManager.instance.blocks.ContainsKey(block.transform.position))
            {
                return 1;
            }
            foreach (var vec in block.InitAdjacents())
            {
                if (GameManager.instance.blocks.ContainsKey(vec))
                {
                    state = 2;
                }
            }
        }
        return state;
    }
    
}
