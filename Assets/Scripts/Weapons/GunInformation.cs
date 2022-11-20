using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInformation : MonoBehaviour
{
    public string name;
    public Rarity rarity;
    public float xTextMargin;
    public static Color[] rarityColors =
    {
        new Color32(179, 179, 179, 255),
        new Color32(66, 177, 201, 255),
        new Color32(236, 26, 201, 255),
        new Color32(253, 198, 66, 255)
    };
}

public enum Rarity
{
    Comum,
    Rara,
    Epica,
    Lendaria
}