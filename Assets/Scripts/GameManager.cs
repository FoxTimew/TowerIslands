using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject blockGroup;
    public Camera cam;

    public CityCenter cityCenter;
    public IslandCreator islandCreator;
    public LevelManager levelManager;
    
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject welcomePage;
    
    [SerializeField] private Block[] baseBlock;

    public Block selectedBlock;
    public Dictionary<Vector2, Block> blocks = new Dictionary<Vector2, Block>();


    [Header("Test")] [SerializeField] private LevelSO level1Test;
    [SerializeField] private Block blockToTest;
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
        if (blockToTest != null)
        {
            blocks.Add(blockToTest.transform.position, blockToTest);
        }
        foreach (var block in baseBlock)
            blocks.Add(block.transform.position, block);
    }

    #endregion


    private bool building;
    
    public void StartLevel(LevelSO level)
    {
        building = true;
        StartCoroutine(LevelCoroutine(level));
    }

    public void StartLevelTest()
    {
        StartLevel(level1Test);
        Debug.Log("Test initiated");
    }
    public void StartWave()
    {
        building = false;
    }

    public List<Enemy> enemies;
    private IEnumerator LevelCoroutine(LevelSO level)
    {
        var waveCount = level.waves.Count;
        Enemy enemy;
        
        while (waveCount > 0)
        {
            Debug.Log("Wave count > 0");
            while (building)
            {
                Debug.Log("building");
                if (Input.touchCount > 0)
                {
                    Debug.Log("touch count >0");
                    if (!IsPointerOverUI())
                    {
                        Debug.Log("IsPointerOverUI");
                        if (selectedBlock is not null)
                        {
                            Debug.Log("Selected block is not null");
                            selectedBlock.Deselect();
                            selectedBlock = null;
                            levelManager.CloseBlockUI();
                        }
                        Touch touch = Input.GetTouch(0);
                        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(touch.position), Vector3.forward);
                        if (hit.collider != null)
                        {
                            Debug.Log("Hit collider not null");
                            if (blocks.ContainsKey(hit.transform.position))
                            {
                                Debug.Log("Block countained in collection");
                                if (blocks[hit.transform.position].selectable)
                                {
                                    Debug.Log("Block is selectable");
                                    selectedBlock = blocks[hit.transform.position];
                                    selectedBlock.Select();
                                    levelManager.OpenBlockUI();
                                }

                            }
                        }
                    }
                
                }
                yield return null;
            }
            levelManager.CloseBlockUI();
            startButton.SetActive(false);
            foreach (var barge in level.waves[waveCount-1].bargesInWave)
            {
                GameObject go = Pooler.instance.Pop("Barge");
                var des = Vector2.one * int.MaxValue;
                go.transform.parent = null;
                go.transform.position = new Vector3(-4.5f, -2.5f, 0);
                foreach (var pos in blocks.Keys)
                {
                    if (blocks[pos].tower is not null) continue;
                    if ((go.transform.position - (Vector3) des).magnitude > (go.transform.position - (Vector3) pos).magnitude) 
                        des = pos;
                }
                go.transform.DOMove(des, ((Vector3) des - go.transform.position).magnitude / 1).SetEase(Ease.Linear);
                yield return new WaitForSeconds(((Vector3) des - go.transform.position).magnitude /1 );
                
                Debug.Log(barge.troops.Count);
                for (int i = 0; i < barge.troops.Count ; i++)
                {
                     GameObject enemyGO = Pooler.instance.Pop("enemy");
                     enemyGO.transform.parent = null; 
                     enemyGO.transform.position = go.transform.position;
                     enemy = enemyGO.GetComponent<Enemy>();
                     enemy.OnSpawn(barge,i);
                     //Pooler.instance.DelayedDepop(2f,"enemy",enemyGO);
                     yield return new WaitForSeconds(0.5f);
                }
                Pooler.instance.Depop("Barge",go);
            }
            while (enemies.Count > 0) yield return null;
            startButton.SetActive(true);
            building = true;
            waveCount--;
            yield return null;
            
        }
        cityCenter.ResetHealth();
        startButton.SetActive(false);
        welcomePage.SetActive(true);

    }



    public void TowerButton()
    {
        if (selectedBlock.tower is not null)
        {
            selectedBlock.SetEnergy(-selectedBlock.tower.stats.energyRequired);
            Pooler.instance.Depop("Tower",selectedBlock.tower.gameObject);
            selectedBlock.tower = null;
            levelManager.OpenBlockUI();
        }
        else
        {
            GameObject go = Pooler.instance.Pop("Tower");
            go.transform.parent = selectedBlock.transform;
            go.transform.position = selectedBlock.transform.position;
            selectedBlock.tower = go.GetComponent<AXD_TowerShoot>();
            selectedBlock.SetEnergy(selectedBlock.tower.stats.energyRequired);
            levelManager.OpenBlockUI();
        }
    }
    
    
    
    
}