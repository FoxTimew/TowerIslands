using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCreator : MonoBehaviour
{
    public GameObject current;
    private string currentType;



    public void SpawnGrid()
    {
        
    }
    public void PopBuild(string key)
    {
        Debug.Log(key);
        if(current != null) Pooler.instance.Depop(currentType,current);
        currentType = key;
        current = Pooler.instance.Pop(currentType);
        current.transform.position = GameManager.instance.cam.ScreenToWorldPoint(Input.GetTouch(0).position);
    }
}
