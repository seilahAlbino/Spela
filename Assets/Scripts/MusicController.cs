using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicAudio;
    // Start is called before the first frame update
    void Start()
    {
        musicAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
