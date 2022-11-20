using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [HideInInspector]
    public static int difficulty = 0;
    [HideInInspector]
    public static bool started = false;
    public GameObject buttons;
    public GameObject[] difficultyTypes;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(difficulty != 0)
        {
            if (started)
            {
                buttons.SetActive(false);
                difficultyTypes[difficulty-1].SetActive(false);
            }
            else
            {
                DrawUI();
            }
        }
    }

    void DrawUI()
    {
        for(int i = 0; i < difficultyTypes.Length; i++)
        {
            if (i + 1 != difficulty) 
                difficultyTypes[i].SetActive(false);
            else
                difficultyTypes[i].SetActive(true);
        }
    }
}
