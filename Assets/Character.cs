using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string name;
    AudioSource audio;
    GameObject player;
    public AudioClip[] audios;
    bool canTalk;
    int frames = 0, framesToTalk = 600;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        canTalk = true;
        player = GameObject.Find("Player");    
    }

    void Update()
    {
        if(!canTalk) frames++;
        if(frames >= framesToTalk)
        {
            frames = 0;
            canTalk = true;
        }

        if (Vector2.Distance(player.transform.position, transform.position) < 2f)
        {
            if (canTalk)
            {
                audio.PlayOneShot(audios[Random.Range(0, audios.Length)], 0.7f);
                canTalk = false;
            }
        }
    }
}
