using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class IslandCreator : MonoBehaviour
{
    public GameObject current;
    public string currentType;
    Vector3 origin;
    [SerializeField] private List<Drag> blocks;
    public Dictionary<int, int> blocksCount = new Dictionary<int, int>();
    [SerializeField] private GameObject canvas;
    Drag drag;
    public Sprite[] blockPreviewSprites;

    void Start()
    {
        foreach(var drag in blocks)
            blocksCount.Add(drag.index,0);
    }

    void Update()
    {
        if (currentType is null) return;
        if (currentType.Length < 3 ) return;
        canvas.SetActive(drag.IsPlaceable());  
        
    }
    public void PopBuild(string key,RectTransform rTransform)
    {
        if(current != null || currentType == key) Pooler.instance.Depop(currentType,current);
        currentType = key;
        current = Pooler.instance.Pop(currentType);
        origin = new Vector3(0,1.335f +GameManager.instance.grid.size * 1.335f,0);
        origin.z = 0;
        current.transform.position = origin;
        drag = current.GetComponent<Drag>();

    }

    private int test;
    public void Depop()
    {
        if (currentType is null) return;
        if (currentType.Length < 3 ) return;
        if(!int.TryParse(currentType[^1..], out int test)) return;
        if (!blocksCount.ContainsKey(test)) return;
        Pooler.instance.Depop(currentType,current);
    }
    
    public void PlaceBlock()
    {
        AudioManager.instance.Play(4, false);
        drag.blocksGo.transform.parent = GameManager.instance.blockGroup.transform;
        GameManager.instance.islandCreator.current = null;
        GameManager.instance.islandCreator.currentType = null;
        GameManager.instance.islandCreator.blocksCount[drag.index]--;
        foreach (var block in drag.blocks)
        {
            block.PlaceBlock();
            GameManager.instance.grid.AddBlock(block);
        }
        GameManager.instance.UpdateBlocks();
        drag.transform.parent = GameManager.instance.blockGroup.transform;
        drag.gameObject.SetActive(false);
        drag.enabled = false;
        drag.dragPointer.enabled = false;

    }
    
}
