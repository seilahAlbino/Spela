using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacters : MonoBehaviour
{
    public PlayerController player;
    public GameObject[] chars;
    bool isSlowed = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < chars.Length; i++)
        {
            if (i == player.playerN) chars[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isSlowed != PlayerController.isSlowMode)
        {
            isSlowed = !isSlowed;
            if (isSlowed)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i].transform.gameObject.GetComponent<Animator>().speed /= 3f;
                }
            }
            else
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i].transform.gameObject.GetComponent<Animator>().speed *= 3f;
                }
            }
 
        }
    }
}