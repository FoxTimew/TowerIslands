using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCreator : MonoBehaviour
{
    public GameObject current;
    public string currentType;
    Vector3 origin;
    [SerializeField] private List<Drag> blocks;
    public Dictionary<Drag, int> blocksCount = new Dictionary<Drag, int>();


    void Start()
    {
        foreach(var drag in blocks)
            blocksCount.Add(drag,1);
    }

    public void PopBuild(string key)
    {
        if(current != null || currentType == key) Pooler.instance.Depop(currentType,current);
        currentType = key;
        current = Pooler.instance.Pop(currentType);
        origin = GameManager.instance.cam.ScreenToWorldPoint(Input.mousePosition);
        origin.z = 0;
        current.transform.position = origin;
    }

    public void Depop()
    {
        if(currentType!=null)
            Pooler.instance.Depop(currentType,current);
    }
}
