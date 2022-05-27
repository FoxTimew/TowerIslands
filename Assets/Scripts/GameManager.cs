
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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
            
            GridIndex index = Instantiate(gridElement,grid.GridElements[i,j].position,Quaternion.identity,gridGroup.transform);
            grid.GridElements[i, j].gridIndex = index;
            index.index = new Index(i, j);
            if (grid.GridElements[i,j].block is not null) grid.GridElements[i, j].gridIndex.Disable();
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

    public int currentWave;
    private int waveCount;
    [SerializeField] private Vector3[] bargeSpawn;

    private Vector3 spawnPoint;
    [SerializeField] private Transform enemyGroup;
    private GameObject bargeGO;
    private GameObject enemyGO;


    public void StartLevel()
    {
        ResetLevel();
        if (buildings.Count <= 0) return;
        if (levelManager.selectedLevel != null)
        {
            /*Sound*/ AudioManager.instance.Play(2, true);
            key = levelManager.selectedLevel.block.name;
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
        StartCoroutine(SpawnWave(levelManager.selectedLevel.waves[currentWave]));
    }

    public void Retry()
    {
        if(levelRoutine is not null) StopCoroutine(levelRoutine);
        ResetLevel();
        for(int i = enemyGroup.childCount-1;i>-1;i--)
            Pooler.instance.Depop(enemyGroup.GetChild(0).name,enemyGroup.GetChild(0).gameObject);
        HDV.Repair();
        selectableBlock = true;
        EconomyManager.instance.SetGold(levelManager.selectedLevel.startGold);
        
    }

    private void ResetLevel()
    {
        waveCount = 0;
        currentWave = 0;
        
    }


    [SerializeField] private TMP_Text waveText;
    private string key;
    
    public IEnumerator LevelCoroutine(LevelSO level)
    {
        /*Sound*/ AudioManager.instance.Play(1, true);
        UI_Manager.instance.CloseMenu(13);
        waveCount = level.waves.Count;
        currentWave = 0;
        Debug.Log(waveCount);
        while (waveCount > 0)
        {
            StartCoroutine(SpawnWave(level.waves[currentWave]));
            
            while (enemyGroup.childCount > 0) yield return null;
            
            
            if (waveCount > 0)
            {
                selectableBlock = true;
                yield return preparationTime;
                UI_Manager.instance.CloseMenu(13);
            }
            /*Sound*/ AudioManager.instance.StopMusic();
            AudioManager.instance.Play(2, true);
            yield return null;
        }
        while (enemyGroup.childCount > 0) yield return null;
        ResetLevel();
        ClearBuildings();
        levelManager.selectedLevel.isCompleted = true;
        Debug.Log(key);
        islandCreator.blocksCount[levelManager.selectedLevel.block.index]++;
        levelManager.selectedLevel = null;
        HDV.Repair();
        UI_Manager.instance.CloseMenuWithoutTransition(8);
        UI_Manager.instance.OpenMenuWithoutTransition(12);
        

    }
    public List<Building> buildings = new List<Building>();

    public void ClearBuildings()
    {
        UI_Manager.instance.CloseMenu(13);
        selectedBlock = null;
        for (int i = buildings.Count - 1; i > -1; i--)
            grid.GridElements[buildings[i].index.x,buildings[i].index.y].block.SellBuilding();
    }
    
    IEnumerator SpawnWave(Wave wave)
    {
        
        GameObject bargeGO;

        currentWave++;
        selectableBlock = false;
        selectedBlock = null;
        foreach (var bargeSo in wave.bargesInWave)
        {
            SpawnBarge(bargeSo);
            yield return new WaitForSeconds(bargeSo.waitingTime);
        }
        waveCount--;
        foreach (var building in buildings) building.ResetTarget();
        yield return null;
    }

    private void SpawnBarge(BargeSO bargeSo)
    {
        GameObject bargeGO;
        spawnPoint = bargeSpawn[Random.Range(0,4)];
        bargeGO = Pooler.instance.Pop("Barge");
        bargeGO.transform.position = spawnPoint;
        bargeGO.transform.parent = enemyGroup;
        bargeGO.transform.DOMove(grid.GetNearestBlock(spawnPoint).transform.position, (grid.GetNearestBlock(spawnPoint).transform.position - spawnPoint).magnitude / bargeSo.bargeSpeed)
            .OnComplete(() => StartCoroutine(SpawnEnemies(bargeSo,bargeGO,spawnPoint)));
    }
    
    private IEnumerator SpawnEnemies(BargeSO barge,GameObject go,Vector3 spawn)
    {
        var bargeGO = go;
        GameObject enemyGO;
        foreach (var troop in barge.troops)
        {
            enemyGO = Pooler.instance.Pop(troop.enemy.enemyStats.eName);
            enemyGO.transform.position = bargeGO.transform.position;
            enemyGO.transform.parent = enemyGroup;
            enemyGO.GetComponent<Enemy>().OnSpawn(barge,troop.cristalToEarn);
            yield return new WaitForSeconds(0.5f);
        }
        bargeGO.transform.DOMove(spawn - transform.position, (spawn - transform.position).magnitude / barge.bargeSpeed).OnComplete(() =>
            Pooler.instance.Depop("Barge",bargeGO));
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

    public bool editorActivated = false;

    public void SetEditor(bool value)
    {
        editorActivated = value;
        gridGroup.SetActive(editorActivated);
    }

    public void Upgrade()
    {
        if (selectedBlock.building.buildingSO.type != BuildingType.Tower) return;
        /*Sound*/AudioManager.instance.Play(23);
        Tower to = (Tower) selectedBlock.building;
        to.Upgrade();
    }

    public void Repair()
    {
        if (selectedBlock is null) return;
        selectedBlock.building.Repair();
    }
}