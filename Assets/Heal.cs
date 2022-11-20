using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public bool isInside;
    public GameObject player;
    public int frames = 0, maxFrames = 5*60;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
            if(isInside){
                frames++;
                if(frames >= maxFrames){
                    if(player.GetComponent<PlayerController>().life < player.GetComponent<PlayerController>().maxLife) 
                        player.GetComponent<PlayerController>().life++;
                    frames = 0;
                }
            }
        else
            player = GameObject.Find("Player");
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.layer == 3)
        isInside = true;
    }
    void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.layer == 3){
            frames = 0;
            isInside = false;
        }
    }
}
