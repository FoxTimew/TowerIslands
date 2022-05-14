using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCreator : MonoBehaviour
{
    public GameObject current;
    public string currentType;
    Vector3 origin;
    [SerializeField] private List<GameObject> blocks;
    public Dictionary<string, int> blocksCount = new Dictionary<string, int>();


    void Start()
    {
        foreach(var drag in blocks)
            blocksCount.Add(drag.name,1);
    }
    
    public void PopBuild(string key)
    {
        if(current != null || currentType == key) Pooler.instance.Depop(currentType,current);
        currentType = key;
        current = Pooler.instance.Pop(currentType);
        origin = GameManager.instance.cam.ScreenToWorldPoint(Input.mousePosition);
        origin.z = 0;
        origin.y += 2 * 2.67f;
        current.transform.position = origin;

    }
    
    
    public void Depop()
    {
        if(currentType!=null)
            Pooler.instance.Depop(currentType,current);
    }
    
}
