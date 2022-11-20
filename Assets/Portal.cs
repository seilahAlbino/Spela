using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform playerTransform;
    public Transform petTransform;
    public Transform Arena;
    public AbstractDungeonGenerator Corridor;
    public GameObject menuDungeon;
    public static bool playInDungeon = false;
    public static int dungeonLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        dungeonLevel = ES3.KeyExists("Player" + PlayerInfoVar.NumberPlayer + "_dungeonLevel") ?
            ES3.Load<int>("Player" + PlayerInfoVar.NumberPlayer + "_dungeonLevel") : 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < 3)
        {
            playInDungeon = true;
            StartDungeon();
        }
        if (SpawnManager.countEnemies == 0 && playInDungeon)
        {   //                                  GANHOU
            dungeonLevel++;
            menuDungeon.SetActive(true);



        }
    }

    public void StartDungeon()
    {
        SpawnManager.countEnemies = 0;
        SpawnManager.enemies = new List<GameObject>();
        Corridor.RunProceduralGeneration();
    }
}
