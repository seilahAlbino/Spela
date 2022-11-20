using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    Animator anim;
    GameObject player;
    bool explosionOn = false;
    public int difficulty = 0;
    public GameObject explosion;
    public Transform spawn;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, new Vector2(player.transform.position.x + 0.3f, player.transform.position.y + 0.5f)) < 0.5f) {
            if (difficulty != 0)
                Arena.difficulty = difficulty;
            else {
                if(Arena.difficulty != 0)
                {
                    Explosion();
                    StartCoroutine(TeleportPlayer(0.5f));
                }
            }
            anim.SetBool("pressed", true);
        }else
            anim.SetBool("pressed", false);

    }

    void Explosion()
    {
        if (explosionOn) return;
        explosionOn = true;
        GameObject explosionGo = Instantiate(explosion, transform);
        Destroy(explosionGo, 2f);
    }

    IEnumerator TeleportPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        player.transform.position = spawn.transform.position;
        explosionOn = false;
        Arena.started = true;
    }
}
