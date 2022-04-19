using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] TMP_Text textTimer, textShips, textDestroyed;
    private float timer = 0, t, t1, t2;
    private int shipsCount = 0, shipsDestroyed = 0, f = 0;
    ObjectPooler objectPooler;

    public int ShipsCount { get => shipsCount; set { shipsCount = value; textShips.text = "Ships  " + shipsCount.ToString();}}
    public int ShipsDestroyed { get => shipsDestroyed; set { shipsDestroyed = value; textDestroyed.text = "Destroyed  " + shipsDestroyed.ToString();}}

#region Singleton
    public static ShipSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }
#endregion

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        t1 = 0; t2 = 1;
        t = t1 + t2; 
        Invoke("SpawnerMethod", t);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        DisplayTime(timer);
    }

    void SpawnerMethod(){        
        if(f==t){
            Invoke("Sequence", 1);
            f=0;
        }else{
            objectPooler.SpawnFromPool("Ship");   
            Invoke("SpawnerMethod", 1/t);
            f++;
        }
    }

    void Sequence(){        
        t1 = t2;
        t2 = t;
        t = t1 + t2;
        SpawnerMethod();
    }

    void DisplayTime(float timer){
        float minutes = Mathf.FloorToInt(timer / 60);  
        float seconds = Mathf.FloorToInt(timer % 60);        
        textTimer.text = string.Format("Timer  {0:00}:{1:00}", minutes, seconds);
    }
}
