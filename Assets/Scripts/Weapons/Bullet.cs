using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public GameObject player;
    public int damageMin;
    public int damageMax;
    bool isSlowed;
    public Effect effect;
    public GameObject particle;
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    void Update()
    {
        if (isSlowed != PlayerController.isSlowMode)
        {
            isSlowed = !isSlowed;
            if (isSlowed)
                speed /= 3f;
            else
                speed *= 3f;
        }

        transform.Translate(Vector3.right * Time.deltaTime * speed);        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.transform.tag);
        //Debug.Log(transform.tag);
        if (transform.tag != "EnemyBullet")
        {
            if (collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "Player")
            {
                if (particle != null)
                {
                    GameObject particleGO = Instantiate(particle, transform);
                    Destroy(particleGO, 3f);
                }
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player")
            {
                if (particle != null)
                {
                    GameObject particleGO = Instantiate(particle, transform);
                    Destroy(particleGO, 3f);
                }
                if(player.GetComponent<PlayerController>().life>0) 
                    player.GetComponent<PlayerController>().sufferDamage(Random.Range(damageMin, damageMax));
            }
            if(collision.gameObject.tag != "Enemy") Destroy(gameObject);
        }
        
    }
}

public enum Effect
{
    Nenhum,
    Gelo
}
