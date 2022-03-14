using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    #region temp

    public bool building = true;
    [SerializeField] private GameObject canvas;

    #endregion
    

    [Header("Manager")] 
    [SerializeField] private UIManager uiManager;
    
    
    [SerializeField] private Camera cam;
    [SerializeField] private  Block[] baseBlock;
    

    public Block selectedBlock;
    public Dictionary<Vector3, Block> blocks = new Dictionary<Vector3, Block>();

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
    }

    void Start()
    {
        foreach (var block in baseBlock)
            blocks.Add(block.transform.position,block);
        foreach (var block in blocks.Values)
            block.FindAdjacents();
        StartCoroutine(GameLoop());
    }
    
    
    #endregion


    IEnumerator GameLoop()
    {
        while (true)
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
        canvas.SetActive(b);

        
    }
}