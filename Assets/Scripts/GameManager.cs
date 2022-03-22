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
    /*IEnumerator GameLoop()
    {
        while (true)
        {
            while (downtown)
            {
                downtownCanvas.SetActive(true);
                yield return null;
            }
            downtownCanvas.SetActive(false);
            EndWave(true);
            for (int i = 0; i < 3; i++)
            {
                while (building)
                {
                    SelectTower();
                    yield return null;
                }
                GameObject go = Pooler.instance.Pop("Enemy");
                go.transform.position = new Vector3(5,1,0);
        
                go = Pooler.instance.Pop("Enemy");
                go.transform.position = new Vector3(0,1,5);
                yield return new WaitForSeconds(10);
                EndWave(true);
            }
            EndWave(false);
            downtown = true;
            downtownCanvas.SetActive(true);
            buildButton.SetActive(true);
            yield return null;
        }
    }

    void SelectTower()
    {
        if (Input.touchCount > 0)
        {
            
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = cam.ScreenPointToRay(touch.position);
                if (IsPointerOverUI()) return;
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit))
                {
                    if(selectedBlock is not null) selectedBlock.Deselect();
                    uiManager.CloseBlockUI();
                    if (hit.transform.GetComponent<Block>())
                    {
                        selectedBlock = blocks[hit.transform.position];
                        selectedBlock.Select();
                        uiManager.OpenBlockUI();
                    }
                }
            }
        }
    }

    public void nextLevel()
    {
        downtown = false;
    }
    
    public void Build()
    {
        GameObject building = Pooler.instance.Pop("Tower");
        building.transform.parent = selectedBlock.transform;
        building.transform.localPosition = Vector3.up;
        selectedBlock.energy -= 2;
        selectedBlock.Deselect();
        uiManager.CloseBlockUI();
    }


    public void EndWave(bool b)
    {
        if(selectedBlock is not null) selectedBlock.Deselect(); 
        uiManager.CloseBlockUI();
        building = b;
        waveCanvas.SetActive(b);
    }

    public void BuildTetris()
    {
        Pooler.instance.Pop("Tetris");
        buildButton.SetActive(false);
    }*/
}
