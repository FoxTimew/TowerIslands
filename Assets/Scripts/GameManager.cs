
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


    [SerializeField] private int gridSize = 10;
    [SerializeField] private Block blockPrefab;
    public Grid grid;


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

    
    
    private void InitGrid()
    {
        grid = new Grid(gridSize);
        foreach (var index in grid.hdvIndex)
        {
            var element = grid.GridElements[index.x, index.y];
            var block = Instantiate(blockPrefab,element.position,Quaternion.identity);
            block.name = $"Block{index.x} {index.y}";
            element.block = block;
            element.walkable = true;
            block.index = index;
            grid.GridElements[index.x, index.y] = element;
            Debug.Log($"{grid.GridElements[index.x, index.y].walkable} {grid.GridElements[index.x, index.y].block.name}");
        }
    }
    
    #endregion






}