using System.Collections;
using System.IO.Compression;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool{
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<Pool> pools; 
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public int startDestroyTime = 5;
    private List<GameObject> shipsActive = new List<GameObject>();

#region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
#endregion

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);   
            }

            poolDictionary.Add(pool.tag, objectPool);
        }

        InvokeRepeating("EnqueueObj", startDestroyTime, 1);
    }
    
    public GameObject SpawnFromPool(string tag){

        if(!poolDictionary.ContainsKey(tag)){
            Debug.LogWarning("Pool with tag" + tag + "doesn't exist.");
            return null;
        }
        
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        shipsActive.Add(objToSpawn);

        IPooledObject pooledObj = objToSpawn.GetComponent<IPooledObject>();
        if(pooledObj != null){
            pooledObj.OnObjectSpawn();
        }
        ShipSpawner.Instance.ShipsCount++;      
        poolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }

    void EnqueueObj(){
        GameObject objToEnqueue = shipsActive[Random.Range(0, shipsActive.Count)];        
        objToEnqueue.SetActive(false);
        ShipSpawner.Instance.ShipsDestroyed += 1;      
        poolDictionary["Ship"].Enqueue(objToEnqueue);
        shipsActive.Remove(objToEnqueue);
    }
}
