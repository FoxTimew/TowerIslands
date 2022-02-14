using System;
using System.Collections;
using System.Collections.Generic;
using Prototype.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour
{
    public static Building current;

    public GridLayout gridLayout;

    private Grid grid;

    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private TileBase whiteTile;

    public GameObject prefab1;
    public GameObject prefab2;

    private PlaceableObject objToPlace;

    #region Unity Methods

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.touchCount < 1) return;
        InitObject(prefab1);
    }

    #endregion

    #region Utils

    public static Vector3 GetTouchWorldPosition()
    {
        if (Input.touchCount < 1) return Vector3.zero;
        Touch touch = Input.GetTouch(0);
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        else
            return Vector3.zero;
    }

    public Vector3 SnapToGrid(Vector3 pos)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        pos = grid.GetCellCenterWorld(cellPos);
        return pos;
    }

    #endregion

    #region Building Placement

    public void InitObject(GameObject prefab)
    {
        Vector3 pos = SnapToGrid(Vector3.zero);

        GameObject go = Pooler.instance.Pop("Tower");
        objToPlace = go.GetComponent<PlaceableObject>();
        objToPlace.AddComponent<Drag>();
    }

    #endregion
}
