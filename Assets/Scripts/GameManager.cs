
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public CameraZoom cameraZoom;
    public static GameManager instance;
    public LevelManager levelManager;
    public GameObject blockGroup;
    public Camera cam;

    [SerializeField] private Building HDV;
    
    public GridIndex gridElement;
    [SerializeField] private int gridSize = 10;
    [SerializeField] private Block blockPrefab;
    
    public IslandCreator islandCreator;
    
    public Grid grid;

    public Block selectedBlock;
    private PolygonCollider2D pc;
    
    //private bool isMoving;

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
    }

    
    public GameObject selectedSprite;
    private RaycastHit2D hit2D;
    private void Update()
    {
        SelectBlock();
        if (HDV.buildingSO.healthPoints <= 0)
        {
            //Defeat
        }
    }

    private void SelectBlock()
    {
        selectedSprite.SetActive(selectedBlock is not null);
        
        if (!Input.GetMouseButtonDown(0)) return;
        hit2D = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit2D)
        {
            
            if (hit2D.transform.GetComponent<Block>() && hit2D.transform.GetComponent<Block>().selectable == true)
            {
                if (Utils.IsPointerOverUI()) return;
                selectedBlock = hit2D.transform.GetComponent<Block>();
                selectedSprite.transform.position = selectedBlock.transform.position;
                levelManager.OpenBlockUI();
            }
            else
            {
                if (Utils.IsPointerOverUI()) return;
                selectedBlock = null;
                levelManager.CloseBlockUI();
            }
        }
        else
        {
            if (Utils.IsPointerOverUI()) return;
            selectedBlock = null;
            levelManager.CloseBlockUI();
        }
    }

    #endregion
    
    public GameObject gridGroup;
    private void InitGrid()
    {
        grid = new Grid(gridSize);
        foreach (var index in grid.hdvIndex)
        {
            var element = grid.GridElements[index.x, index.y];
            var block = Instantiate(blockPrefab,element.position,Quaternion.identity,blockGroup.transform);
            block.name = $"Block{index.x} {index.y}";
            element.block = block;
            element.walkable = true;
            block.index = index;
            block.selectable = false;
            block.building = HDV;
            grid.GridElements[index.x, index.y] = element;
            //Debug.Log($"{grid.GridElements[index.x, index.y].walkable} {grid.GridElements[index.x, index.y].block.name}");
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




    [SerializeField] private Vector3 bargeSpawn;
    [SerializeField] private Transform enemyGroup;
    private bool waitStartWave = true;
    private GameObject go;
    private GameObject enemy;


    public void StartLevel(LevelSO level)
    {
        StartCoroutine(LevelCoroutine(level));
    }

    public void StartLevelTest()
    {
        StartLevel(level1Test);
        Debug.Log("Test initiated");
    }
    public void StartWave()
    {
        waitStartWave = false;
    }
    public IEnumerator LevelCoroutine(LevelSO level)
    {
        var waveCount = level.waves.Count;
        var currentWave = 0;
        while (waveCount > 0)
        {
            while (waitStartWave) yield return null;
            foreach (var bargeSo in level.waves[currentWave].bargesInWave)
            {
                go = Pooler.instance.Pop("barge");
                go.transform.position = bargeSpawn;
                go.transform.DOMove(grid.GetNearestBlock(bargeSpawn).transform.position, (grid.GetNearestBlock(bargeSpawn).transform.position - bargeSpawn).magnitude / bargeSo.bargeSpeed);
                yield return new WaitForSeconds(
                    (grid.GetNearestBlock(bargeSpawn).transform.position - bargeSpawn).magnitude / bargeSo.bargeSpeed);
                foreach (var troop in bargeSo.troops)
                {
                    enemy = Pooler.instance.Pop(troop.enemy.enemyStats.eName);
                    enemy.transform.position = go.transform.position;
                    enemy.transform.parent = enemyGroup;
                    enemy.GetComponent<Enemy>().OnSpawn(bargeSo,troop.cristalToEarn);
                    yield return new WaitForSeconds(0.5f);
                }
                Pooler.instance.Depop("barge",go);
                while (enemyGroup.childCount > 0) yield return null;
                waveCount--;
                currentWave++;
                waitStartWave = true;
            }
            yield return null;
        }
    }

    public void SellBuilding()
    {
        selectedBlock.DestroyBuilding();
    }


    


}