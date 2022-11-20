using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController2 : MonoBehaviour
{

    SpriteRenderer sprite;
    public GameObject[] bullet;
    public Transform spawnBullet;
    public Transform spawnBullet2;
    Animator anim;
    AudioSource shootAudio;
    bool shoot;

    public int stamina;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        shootAudio = GetComponent<AudioSource>();
        shoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.parent.name == "Weapon"){
            if(!PlayerController.isPaused){
                Aim();
                if (Input.GetButton("Fire1") && shoot && transform.parent.parent.GetComponent<PlayerController>().stamina >= this.stamina)
                {
                    Shoot();
                    transform.parent.parent.GetComponent<PlayerController>().stamina-=this.stamina;
                    StartCoroutine(ExecuteAfterTime(0.1f));
                }
            }
        }
    }

    void Aim()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);

        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        sprite.flipY = mousePos.x < screenPoint.x; 
    }

    void Shoot()
    {
            Instantiate(bullet[Random.Range(0, bullet.Length)], sprite.flipY ? spawnBullet2.position : spawnBullet.position, spawnBullet.rotation);
            transform.Translate(-0.075f * Time.deltaTime, 0, 0);
            if(PlayerInfoVar.hasSound) shootAudio.PlayOneShot(shootAudio.clip);
            shoot = false;
    }

    IEnumerator ExecuteAfterTime(float time)
 {
     yield return new WaitForSeconds(time);
 
     shoot = true;
 }
}
