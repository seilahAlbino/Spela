using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInformation : MonoBehaviour
{
    public string name;
    public float xTextMargin;

    [Header("If is a upgrade")]
    public int idUpgrade;
    public int currentUpgrade;

    public int upgrade;
    public int newUpgrade;


    public float height;
    public float width;

    public int price;
    public int newPrice;
    public string description;

    private void Start()
    {
        SpawnItensManager.itemsInScene.Add(gameObject);
        SpawnItensManager.countItems++;
    }
}
