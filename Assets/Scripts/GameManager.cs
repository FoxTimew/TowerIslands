
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

    public GameObject blockGroup;
    public Camera cam;

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
        selectedSprite.SetActive(selectedBlock is not null);
        if (!Input.GetMouseButtonDown(0)) return;
        hit2D = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit2D)
        {
            if (hit2D.transform.GetComponent<Block>() || hit2D.transform.GetComponent<Block>().selectable == true) 
            {
                selectedBlock = hit2D.transform.GetComponent<Block>();
                selectedSprite.transform.position = selectedBlock.transform.position;
            }
            else selectedBlock = null;
        }
        else selectedBlock = null;
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
    






}