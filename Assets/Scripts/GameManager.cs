using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject blockGroup;
    public Camera cam;

    [Header("Manager")] 
    public IslandCreator islandCreator;
    
    [SerializeField] private  Block[] baseBlock;

    public Block selectedBlock;
    public Dictionary<Vector2, Block> blocks = new Dictionary<Vector2, Block>();

    public static bool IsPointerOverUI() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return true;
        } 
            
        var pe = new PointerEventData(EventSystem.current)
        {
            position = Input.GetTouch(0).position
        };
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);
        return hits.Count > 0;
    }
    
    #region Unity Methods
    void Awake()
    {
        if (instance != null) 
        { 
            Destroy(gameObject);
            return;
        }
                
        DontDestroyOnLoad(gameObject);
        instance = this;
        cam.transparencySortMode = TransparencySortMode.CustomAxis;
        cam.transparencySortAxis = Vector3.up;

        foreach (var block in baseBlock)
            blocks.Add(block.transform.position,block);
    }

    #endregion


    private bool building;
    public void StartLevel()
    {
        //building = true;
        StartCoroutine(LevelCoroutine());
    }

    public void StartWave()
    {
        building = false;
    }
    
    private IEnumerator LevelCoroutine()
    {
        while (building)
        {
            yield return null;
        }

        GameObject go = Pooler.instance.Pop("enemy");
        go.transform.parent = null;
        go.transform.position = new Vector3(-4.5f, -2.5f, 0);
        yield return null;

    }
}
