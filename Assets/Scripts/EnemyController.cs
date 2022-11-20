using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{

    public float speed;
    GameObject player;
    public Animator anim;
    public bool isDead = false;
    public int life;
    public int damage;
    Rigidbody2D rb;
    NavMeshAgent agent;
    float speed2;
    bool isSlowed = false;
    public bool isFrozen = false;
    public bool isKnocked = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        speed2 = speed;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void FixedUpdate()
    {
        if (player != null && !isDead && !isFrozen)
        {
            //rb.velocity = (player.transform.position - transform.position).normalized * speed * Time.deltaTime;
            //rb.AddForce((player.transform.position - transform.position).normalized * speed, ForceMode2D.Impulse);
            //Vector3 direction = (player.transform.position - transform.position).normalized;
            //rb.MovePosition(player.transform.position *speed* Time.deltaTime);
            
            if (!player.GetComponent<PlayerController>().isInvisible && !isKnocked)
            {
                if(Random.Range(0,100)<10) agent.SetDestination(/*new Vector3(player.transform.position.x+1.85f, player.transform.position.y+0.76f, player.transform.position.z)*/player.transform.position);
            }
            if (isKnocked)
            {
                agent.isStopped = true;
                agent.ResetPath();
                //agent.enabled = false;
                speed = -10;
                //rb.MovePosition(player.transform.position * -2 * Time.deltaTime);
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                StartCoroutine(knocked(1));
            }
        }
    }

    void Update()
    {
        if(isSlowed != PlayerController.isSlowMode)
        {
            isSlowed = !isSlowed;
            if (isSlowed)
                agent.speed /= 3f;
            else
                agent.speed *= 3f;
        }

        if (life <= 0 && !isDead) Kill();
    }

    private void OnCollisionEnter2D(Collision2D  collision)
    {
        if (collision.gameObject.tag == "Bullet" && !isDead)
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet.effect != Effect.Nenhum)
            {
                switch (bullet.effect)
                {
                    case Effect.Gelo:
                        if (!isFrozen)
                        {
                            if (Random.Range(0, 100) <= 100)
                            {
                                isFrozen = true;
                                transform.GetChild(1).gameObject.SetActive(true);
                                if(anim != null) anim.SetBool("isFrozen", true);
                                StartCoroutine(Unfreeze(3));
                            }
                        }
                        break;
                }
            }

            int dmg = Random.Range(bullet.damageMin, bullet.damageMax);
            life -= dmg;
            GameObject dmgFloating = Instantiate(player.GetComponent<PlayerController>().floatingDamage.gameObject, transform.position, Quaternion.identity);
            dmgFloating.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 9;
            dmgFloating.transform.GetChild(0).GetComponent<TextMesh>().text = dmg + "";
            Destroy(dmgFloating, .5f);
            
            //gameObject.layer = LayerMask.NameToLayer("EnemyDead");
            //Debug.Log(gameObject.layer);
        }
        else if(collision.gameObject.tag == "Player" && !isDead && player.GetComponent<PlayerController>().canSufferDamage && player.GetComponent<PlayerController>().canSufferDamagePower)
        {
            player.GetComponent<PlayerController>().sufferDamage(damage);
            /*while(damageGiven > 0)
            {
                if (player.GetComponent<PlayerController>().def > 0)
                {
                    player.GetComponent<PlayerController>().def--;
                }
                else
                {
                    if (player.GetComponent<PlayerController>().life > 0)
                    {
                        player.GetComponent<PlayerController>().life--;
                    }
                }
                damageGiven--;
                player.GetComponent<PlayerController>().canSufferDamage = false;
            }*/

        }
    }

    public void Kill()
    {
        anim.SetTrigger("Death");
        isDead = true;
        gameObject.layer = 11;
        Destroy(gameObject, 0.5f);
        SpawnManager.enemies.Remove(gameObject);
        SpawnManager.countEnemies--;
        player.GetComponent<PlayerController>().money+=2000;
        player.GetComponent<PlayerController>().txt.text = player.GetComponent<PlayerController>().money.ToString();
    }

    IEnumerator Unfreeze(float time)
    {
        yield return new WaitForSeconds(time);

        isFrozen = false;
        transform.GetChild(1).gameObject.SetActive(false);
        anim.SetBool("isFrozen", false);
    }
    IEnumerator knocked(float time)
    {
        yield return new WaitForSeconds(time);
        speed = speed2;
        isKnocked = false;
        agent.enabled = true;
    }
}
