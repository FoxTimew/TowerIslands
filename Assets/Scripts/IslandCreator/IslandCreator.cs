using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandCreator : MonoBehaviour
{
    public GameObject current;
    public string currentType;
    Vector3 origin;
    [SerializeField] private List<Drag> blocks;
    public Dictionary<int, int> blocksCount = new Dictionary<int, int>();


    void Start()
    {
        foreach(var drag in blocks)
            blocksCount.Add(drag.index,0);
    }
    
    public void PopBuild(string key,RectTransform rTransform)
    {
        if(current != null || currentType == key) Pooler.instance.Depop(currentType,current);
        currentType = key;
        current = Pooler.instance.Pop(currentType);
        // origin = GameManager.instance.cam.ScreenToWorldPoint(Input.mousePosition);
        // origin.z = 0;
        // origin.y += 2 * 2.67f;
        
        origin = new Vector3(0,GameManager.instance.grid.size * 1.335f,0);
        //origin.x -= 150;
        origin.z = 0;
        current.transform.position = origin;

    }
    
    
    public void Depop()
    {
        if (currentType is null) return;
        if (!blocksCount.ContainsKey(int.Parse(currentType[^1..]))) return;
        Pooler.instance.Depop(currentType,current);
    }
    
}
