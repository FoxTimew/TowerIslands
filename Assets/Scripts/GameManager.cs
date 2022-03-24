using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public LevelManager levelManager;
    
    [SerializeField] private Block[] baseBlock;

    public Block selectedBlock;
    public Dictionary<Vector2, Block> blocks = new Dictionary<Vector2, Block>();
    

    public static bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
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
            blocks.Add(block.transform.position, block);
    }

    #endregion


    private bool building;

    public void StartLevel()
    {
        building = true;
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
            if (Input.touchCount > 0)
            {
                if (!IsPointerOverUI())
                {
                    if (selectedBlock is not null)
                    {
                        selectedBlock.Deselect();
                        selectedBlock = null;
                        levelManager.CloseBlockUI();
                    }
                    Touch touch = Input.GetTouch(0);
                    RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(touch.position), Vector3.forward);
                    if(hit.collider != null)
                        if (blocks.ContainsKey(hit.transform.position))
                        {
                            selectedBlock = blocks[hit.transform.position];
                            selectedBlock.Select();
                            levelManager.OpenBlockUI();
                        }
                }
                
            }
            yield return null;
        }

        GameObject go = Pooler.instance.Pop("enemy");
        go.transform.parent = null;
        go.transform.position = new Vector3(-4.5f, -2.5f, 0);
        yield return null;
    }



    public void BuildTower()
    {
        if (selectedBlock.tower is not null) return;
        levelManager.CloseBlockUI();
        selectedBlock.energy -= 2;
        GameObject go = Pooler.instance.Pop("Tower");
        go.transform.parent = selectedBlock.transform;
        go.transform.position = selectedBlock.transform.position;
        selectedBlock.tower = go.GetComponent<AXD_TowerShoot>();
    }
}