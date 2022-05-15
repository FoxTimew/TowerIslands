
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public CameraZoom cameraZoom;
    public static GameManager instance;
    public LevelManager levelManager;
    public GameObject blockGroup;
    public Camera cam;

    [SerializeField] public Building HDV;
    
    public GridIndex gridElement;
    [SerializeField] private int gridSize = 10;
    [SerializeField] private Block blockPrefab;
    
    public IslandCreator islandCreator;
    
    public Grid grid;

    public Block selectedBlock;
    private PolygonCollider2D pc;

    [Header("TestUI")] [SerializeField] private LevelSO level1Test;


    [Header("Base Building Prefabs")] 
    public BuildingSO rapidTowerSO;
    public BuildingSO mortarTowerSO;
    public BuildingSO stunTrapSO;
    public BuildingSO damageTrapSO;
    public BuildingSO defenseSupportSO;
    public BuildingSO energySupportSO;
    

    #region Unity Methods

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        cam.transparencySortMode = TransparencySortMode.CustomAxis;
        cam.transparencySortAxis = Vector3.up;
        
        InitGrid();
        preparationTime = new WaitForSeconds(timeBetweenWaves);
    }

    
    private RaycastHit2D hit2D;
    private void Update()
    {
        if (!selectableBlock) return;
        UnSelectBlock();
    }
    
    #endregion
    
    
    #region Grid

    public GameObject gridGroup;
    private Grid.GridElement element;
    private Block block;
    private void InitGrid()
    {
        grid = new Grid(gridSize);
        foreach (var index in grid.hdvIndex)
        {
            element = grid.GridElements[index.x, index.y];
            block = Instantiate(blockPrefab,element.position,Quaternion.identity,blockGroup.transform);
            element.block = block;
            element.walkable = true;
            block.index = index;
            block.selectable = false;
            block.building = HDV;
            grid.GridElements[index.x, index.y] = element;
        }
        foreach (var index in grid.baseBlocks)
        {
            element = grid.GridElements[index.x, index.y];
            block = Instantiate(blockPrefab,element.position,Quaternion.identity,blockGroup.transform);
            element.block = block;
            element.walkable = true;
            block.index = index;
            block.selectable = true;
            grid.GridElements[index.x, index.y] = element;
        }
        for(int i = 0;i<gridSize;i++)
        for(int j = 0;j<gridSize;j++)
        {
            if (grid.GridElements[i,j].block is not null) continue;
            GridIndex index = Instantiate(gridElement,grid.GridElements[i,j].position,Quaternion.identity,gridGroup.transform);
            grid.GridElements[i, j].gridIndex = index;
            index.index = new Index(i, j);
        }
        
        pc = gridGroup.GetComponent<PolygonCollider2D>();
        
        pc.points=new []
        {
            new Vector2(-3.56f + (-3.56f * (Mathf.Round(grid.size*0.5f)-1)) , 0),
            new Vector2( -0.12f + (-0.12f * (Mathf.Round(grid.size*0.5f)-1)),  -2.67f + (-2.67f * (Mathf.Round(grid.size*0.5f)-1))),
            new Vector2(3.56f + (3.56f * (Mathf.Round(grid.size*0.5f)-1)) , 0),
            new Vector2( 0.12f + (0.12f * (Mathf.Round(grid.size*0.5f)-1)),  2.67f + (2.67f * (Mathf.Round(grid.size*0.5f)-1))),
        };
    }

    public void UpdateBlocks()
    {
        foreach (var element in grid.GridElements)
            if(element.walkable)
                element.block.UpdateAdjacents();
    }


    #endregion

    
    #region Level

    public Coroutine levelRoutine;
    [SerializeField] private float timeBetweenWaves = 10;
    private WaitForSeconds preparationTime;
    public bool selectableBlock = false;

    private int currentWave;
    private int waveCount;
    [SerializeField] private Vector3[] bargeSpawn;

    private Vector3 spawnPoint;
    [SerializeField] private Transform enemyGroup;
    private GameObject bargeGO;
    private GameObject enemyGO;


    public void StartLevel()
    {

        if (levelManager.selectedLevel != null)
        {
            levelRoutine = StartCoroutine(LevelCoroutine(levelManager.selectedLevel));
        }
    }
    public void SetBlockSelectable(bool value)
    {
        selectableBlock = value;
    }
    public void StartLevelTest()
    {
        Debug.Log("Test initiated");
    }
    public void StartWave()
    {
        waveCount--;
        currentWave++;
    }

    public void Retry()
    {
        StopCoroutine(GameManager.instance.levelRoutine);
        for(int i = enemyGroup.childCount-1;i>-1;i--)
            Pooler.instance.Depop(enemyGroup.GetChild(0).name,enemyGroup.GetChild(0).gameObject);
        HDV.Repair();
        ResetLevel();
        selectableBlock = true;

    }

    private void ResetLevel()
    {
        waveCount = 0;
        currentWave = 0;
    }
    
    public IEnumerator LevelCoroutine(LevelSO level)
    {
        waveCount = level.waves.Count;
        currentWave = -1;
        Debug.Log(waveCount);
        while (waveCount > 0)
        {
            StartWave();
            Debug.Log(waveCount);
            selectableBlock = false;
            selectedBlock = null;
            Debug.Log("StartWave");
            foreach (var bargeSo in level.waves[currentWave].bargesInWave)
            {
                spawnPoint = bargeSpawn[Random.Range(0,4)];
                bargeGO = Pooler.instance.Pop("barge");
                bargeGO.transform.position = spawnPoint;
                bargeGO.transform.DOMove(grid.GetNearestBlock(spawnPoint).transform.position, (grid.GetNearestBlock(spawnPoint).transform.position - spawnPoint).magnitude / bargeSo.bargeSpeed);
                yield return new WaitForSeconds(
                    (grid.GetNearestBlock(spawnPoint).transform.position - spawnPoint).magnitude / bargeSo.bargeSpeed);
                foreach (var troop in bargeSo.troops)
                {
                    enemyGO = Pooler.instance.Pop(troop.enemy.enemyStats.eName);
                    enemyGO.transform.position = bargeGO.transform.position;
                    enemyGO.transform.parent = enemyGroup;
                    enemyGO.GetComponent<Enemy>().OnSpawn(bargeSo,troop.cristalToEarn);
                    yield return new WaitForSeconds(0.5f);
                }
                Pooler.instance.Depop("barge",bargeGO);
            }
            while (enemyGroup.childCount > 0) yield return null;
            
            if (waveCount > 0)
            {
                selectableBlock = true;
                yield return preparationTime;
                UI_Manager.instance.CloseMenu(13);
            }
            yield return null;
        }

        levelManager.selectedLevel.isCompleted = true;
        HDV.Repair();
        UI_Manager.instance.CloseMenu(8);
        UI_Manager.instance.OpenMenu(12);
        ResetLevel();

    }

    #endregion
    
    
    [SerializeField] private LayerMask layerMask;
    private void UnSelectBlock()
    {
        
        if (!Input.GetMouseButtonDown(0)) return;
        hit2D = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,layerMask);
        if (hit2D)
        {
            
            if (hit2D.transform.CompareTag("Block")) return;
            if (Utils.IsPointerOverUI()) return;
            selectedBlock = null;
            UI_Manager.instance.CloseMenu((int) MenuEnum.BlockInfo);
        }
        else
        {
            if (Utils.IsPointerOverUI()) return;
            selectedBlock = null;
            UI_Manager.instance.CloseMenu((int) MenuEnum.BlockInfo);
        }
    }
    public void SellBuilding()
    {
        selectedBlock.SellBuilding();
    }


    


}