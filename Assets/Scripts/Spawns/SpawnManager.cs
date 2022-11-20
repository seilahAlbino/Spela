using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static List<GameObject> enemies = new List<GameObject>();
    public Transform[] spawnPoints;
    public GameObject enemy;
    [HideInInspector]
    public static int countEnemies = 0;
    [HideInInspector]
    public bool spawn = false;
    // Start is called before the first frame update
}
