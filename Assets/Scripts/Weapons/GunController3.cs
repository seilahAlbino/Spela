using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController3 : MonoBehaviour
{

    LineRenderer laserRenderer;
    int laserDistance = 100;
    int maxReflect = 3;
    Vector3 directLaser = new Vector3();
    bool loopActive = true;
    public Transform point1;
    public Transform point2;
    Vector3 pos;

    public int damage;
    public int stamina;
    bool downStamina;
    AudioSource shootAudio;
    // Start is called before the first frame update
    void Start()
    {
        shootAudio = GetComponent<AudioSource>();
        downStamina = true;
        laserRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(transform.parent.parent.name == "Weapon"){
            if(!PlayerController.isPaused){
                if (Input.GetButton("Fire1") && transform.parent.parent.parent.GetComponent<PlayerController>().stamina >= this.stamina)
                {
                    Shoot();
                    if (downStamina) {
                        transform.parent.parent.parent.GetComponent<PlayerController>().stamina -= this.stamina;
                        downStamina = false;
                        StartCoroutine(DownStamina(0.1f));
                    }
                }
                if (Input.GetButtonUp("Fire1") || transform.parent.parent.parent.GetComponent<PlayerController>().stamina < this.stamina)
                {
                    laserRenderer.positionCount = 0;
                }
            }
        }
    }
    void Shoot()
    {
        loopActive = true;
        shootAudio.Play();
        int countLaser = 1;  
        if(transform.parent.GetComponent<SpriteRenderer>().flipY){
            pos = point2.localPosition;
        }else{
            pos = point1.localPosition;
        }
        pos.x += transform.parent.parent.parent.position.x;
        pos.y += transform.parent.parent.parent.position.y;

        directLaser = pos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        laserRenderer.positionCount = countLaser;
        laserRenderer.SetPosition(0, point1.position);
        directLaser *= -1;
        while(loopActive){
            int ignoreLayers = (1 << 6)|(1<<7)|(1<<13);
            RaycastHit2D hit = Physics2D.Raycast(pos, directLaser, laserDistance, ignoreLayers);
                if (hit){
                if (hit.transform.gameObject.layer == 7)
                {
                    Debug.Log(hit.transform.name);
                    if (hit.transform.gameObject.GetComponent<EnemyController>() != null) { 
                        if (!hit.transform.gameObject.GetComponent<EnemyController>().isDead)
                        {
                            hit.transform.gameObject.GetComponent<EnemyController>().Kill();
                        }
                    }else if (hit.transform.gameObject.GetComponent<Bomber>() != null) {
                            if (!hit.transform.gameObject.GetComponent<Bomber>().isDead)
                            {
                                hit.transform.gameObject.GetComponent<Bomber>().Kill();
                            }
                        }
                    else if (hit.transform.gameObject.GetComponent<Bat>() != null)
                    {
                        if (!hit.transform.gameObject.GetComponent<Bat>().isDead)
                        {
                            hit.transform.gameObject.GetComponent<Bat>().Kill();
                        }
                    }

                }
                if (hit.transform.gameObject.layer != 3)
                    {
                        countLaser++;
                        laserRenderer.positionCount = countLaser;
                        directLaser = Vector3.Reflect(directLaser, hit.normal);
                        pos = (Vector2)directLaser.normalized + (Vector2)hit.point;
                        laserRenderer.SetPosition(countLaser - 1, hit.point);
                    }
                    else
                    {
                        countLaser++;
                        laserRenderer.positionCount = countLaser;
                        laserRenderer.SetPosition(countLaser - 1, pos + (directLaser.normalized * laserDistance));
                        loopActive = false;
                    }
                }
                else{
                        countLaser++;
                        laserRenderer.positionCount = countLaser;
                        laserRenderer.SetPosition(countLaser-1, pos+(directLaser.normalized*laserDistance));
                        loopActive = false;
                    }
            
            

            if(countLaser > maxReflect){
                loopActive = false;
            }
        }
    }

    IEnumerator DownStamina(float time)
    {
        yield return new WaitForSeconds(time);

        //downStamina = true;
    }
}
