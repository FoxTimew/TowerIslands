using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    public static Pooler instance;

    private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();
    [SerializeField] private List<PoolKey> poolKeys = new List<PoolKey>();
    
    [Serializable]
    public class Pool
    {
        public GameObject prefab;
        public Queue<GameObject> queue = new Queue<GameObject>();

        public int baseCount;
        public float baseRefreshSpeed = 5;
        [HideInInspector] public float refreshSpeed = 5;
    }

    [Serializable]
    public class PoolKey
    {
        public string key;
        public Pool pool;
    }

    void Awake()
    {
        if (instance != null) 
        { 
            Destroy(gameObject);
            return;
        }
        instance = this;
        InitPools();
        PopulatePools();
    }

    private int i;
    private void InitPools()
    {
        for (i = 0; i < poolKeys.Count; i++)
        {
            pools.Add(poolKeys[i].key, poolKeys[i].pool);
        }
    }
    
    private void PopulatePools()
    {
        foreach (var pool in pools)
        {
            for (i = 0; i < pool.Value.baseCount; i++)
            {
                AddInstance(pool.Value);
            }           
        }
    }

    private GameObject objectInstance;
    private void AddInstance(Pool pool)
    {
        objectInstance = Instantiate(pool.prefab, transform);
        objectInstance.SetActive(false);
        pool.queue.Enqueue(objectInstance);
    }

    void Start()
    {
        InitRefreshCount();
    }

    private void InitRefreshCount()
    {
        foreach (KeyValuePair<string,Pool> pool in pools)
        {
            StartCoroutine(RefreshPool(pool.Value,pool.Value.baseRefreshSpeed));
        }
    }

    private IEnumerator RefreshPool(Pool pool, float t)
    {
        yield return new WaitForSeconds(t);

        if (pool.queue.Count < pool.baseCount)
        {
            AddInstance(pool);
            pool.refreshSpeed = pool.baseRefreshSpeed * (pool.queue.Count / pool.baseCount);
        }
        
        StartCoroutine(RefreshPool(pool,pool.refreshSpeed));
    }

    public GameObject Pop(string key)
    {
        if (pools[key].queue.Count == 0)
        {
            Debug.LogWarning("pull of "+key+"is empty");
            AddInstance(pools[key]);
        }
        objectInstance = pools[key].queue.Dequeue();
        objectInstance.SetActive(true);
        
        return objectInstance;
    }

    public void Depop(string key, GameObject go)
    {
        pools[key].queue.Enqueue(go);
        go.transform.parent = transform; //Au cas où on a déplacé le GO
        go.SetActive(false);
    }

    public void DelayedDepop(float t, string key, GameObject go)
    {
        StartCoroutine(DelayedDepopCoroutine(t, key, go));
    }

    private IEnumerator DelayedDepopCoroutine(float t, string key, GameObject go)
    {
        yield return new WaitForSeconds(t);
        Depop(key,go);
    }
    
    public Dictionary<string, Pool> GetPools()
    {
        return pools;
    }
}
