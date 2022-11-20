using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItensManager : MonoBehaviour
{
    public static List<GameObject> itemsInScene = new List<GameObject>();
    public Transform[] spawnPoints;
    public GameObject[] items;
    [HideInInspector]
    public static int maxItems = 3, countItems = 0;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnItems", 0.5f, 1f);
    }

    // Update is called once per frame
    void SpawnItems()
    {
        if (countItems < maxItems)
        {
            int index = Random.Range(0, spawnPoints.Length);
            GameObject itemGO = Instantiate(items[Random.Range(0, items.Length)], spawnPoints[index].position, Quaternion.identity);
            countItems++;
            itemsInScene.Add(itemGO);
        }
    }
}
