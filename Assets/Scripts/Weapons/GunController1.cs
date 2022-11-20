using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController1 : MonoBehaviour
{

    SpriteRenderer sprite;
    public GameObject bullet;
    public Transform spawnBullet;
    Animator anim;
    AudioSource shootAudio;
    bool shoot;
    public float time = 1;
    public int stamina;
    [Header("1 = shotgun | 2 = pistol")]
    public int type = 1;
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
                    transform.parent.parent.GetComponent<PlayerController>().stamina -= this.stamina;
                    StartCoroutine(ExecuteAfterTime(time));
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
        if (type == 1)
        {
            Instantiate(bullet, spawnBullet.position, spawnBullet.rotation);
            spawnBullet.Rotate(0, 0, 10);
            Instantiate(bullet, spawnBullet.position, spawnBullet.rotation);
            spawnBullet.Rotate(0, 0, -20);
            Instantiate(bullet, spawnBullet.position, spawnBullet.rotation);
            spawnBullet.Rotate(0, 0, 10);
        }else if(type == 2)
        {
            Instantiate(bullet, spawnBullet.position, spawnBullet.rotation);
        }
            
            shoot = false;
            if (PlayerInfoVar.hasSound) shootAudio.PlayOneShot(shootAudio.clip);
    }

    IEnumerator ExecuteAfterTime(float time)
    {
     yield return new WaitForSeconds(time);
 
     shoot = true;
    }
}
