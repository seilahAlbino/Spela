using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Bat : MonoBehaviour
{
    GameObject player;
    public GameObject bullet;
    public GameObject spawnBullet;
    SpriteRenderer sprite;
    public float framesShoot = 0, maxFramesShoot = 180;

    AudioSource deadAudio;
    public float speed;
    public Animator anim;
    public bool isDead = false;
    public int life;
    public int damage;
    public AudioClip audioClip;
    Rigidbody2D rb;
    NavMeshAgent agent;
    float speed2;
    bool isSlowed = false;
    public bool isFrozen = false;
    public bool isKnocked = false;
    bool playAudio = false;
    int frames = 0, framesToChangePos = 60;
    Vector3 attackPos;
    public GameObject enemyRemain;

    // Start is called before the first frame update
    void Start()
    {
        deadAudio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackPos = new Vector3(player.transform.position.x + Random.Range(-1.85f, 1.85f), player.transform.position.y + Random.Range(-0.76f, 0.76f), player.transform.position.z);
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        enemyRemain = GameObject.Find("RemainEnemies");
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController.isPaused)
        {
            if (isSlowed != PlayerController.isSlowMode)
            {
                isSlowed = !isSlowed;
                if (isSlowed)
                    agent.speed /= 3f;
                else
                    agent.speed *= 3f;
            }
            
            if (!isSlowed && !isDead && !isFrozen && Vector2.Distance(player.transform.position, transform.position) < 10)
            {
                if (!playAudio && deadAudio.clip != null)
                {
                    playAudio = true;
                    deadAudio.loop = true;
                    //deadAudio.clip = audioClip;
                    if (PlayerInfoVar.hasSound) deadAudio.Play();
                }
                Aim();
                framesShoot++;
                if (framesShoot >= maxFramesShoot)
                {
                    framesShoot = 0;
                    Shoot();
                }
            }
            else
            {
                if (!isDead)
                {
                    playAudio = false;
                    deadAudio.Stop();
                }
            }

            if (life <= 0 && !isDead) Kill();
        }
    }

    private void FixedUpdate()
    {
        frames++;
        if(frames >= framesToChangePos)
        {
            frames = 0;
            attackPos = new Vector3(player.transform.position.x + Random.Range(-1.85f*2, 1.85f*2), player.transform.position.y + Random.Range(-0.76f*2, 0.76f*2), player.transform.position.z);
        }
        if (player != null && !isDead && !isFrozen)
        {
            //rb.velocity = (player.transform.position - transform.position).normalized * speed * Time.deltaTime;
            //rb.AddForce((player.transform.position - transform.position).normalized * speed, ForceMode2D.Impulse);
            //Vector3 direction = (player.transform.position - transform.position).normalized;
            //rb.MovePosition(player.transform.position *speed* Time.deltaTime);
            
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
            else
            {
                if (!player.GetComponent<PlayerController>().isInvisible && !isKnocked)
                {
                    if (Vector2.Distance(player.transform.position, transform.position) < 20)
                    {
                        if (Random.Range(0, 100) < 10) agent.SetDestination(attackPos);
                    }
                    else
                    {
                        agent.SetDestination(transform.position);
                    }
                }
            }
        }
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
                                //attackPos = transform.position;
                                agent.speed = 0;
                                anim.speed = 0;
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
        }
        
    }

    public void Kill()
    {
        deadAudio.Stop();
        deadAudio.clip = audioClip;
        deadAudio.loop = false;

        if (PlayerInfoVar.hasSound) deadAudio.Play();

        agent.speed = 0;
        agent.isStopped = true;
        anim.SetBool("dead", true);
        isDead = true;
        gameObject.layer = 11;

        Destroy(gameObject, 2f);

        SpawnManager.enemies.Remove(gameObject);
        SpawnManager.countEnemies--;

        player.GetComponent<PlayerController>().money+=2000;
        player.GetComponent<PlayerController>().txt.text = player.GetComponent<PlayerController>().money.ToString();



    }

    IEnumerator Unfreeze(float time)
    {
        yield return new WaitForSeconds(time);
        //attackPos = new Vector3(player.transform.position.x + Random.Range(-1.85f, 1.85f), player.transform.position.y + Random.Range(-0.76f, 0.76f), player.transform.position.z);
        agent.speed = 3.5f;
        anim.speed = 1;
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
    Vector3 direction;

    void Aim()
    {
        //Vector3 mousePos = Input.mousePosition;
        //Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        /* Vector2 offset = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

         float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

         spawnBullet.transform.rotation = Quaternion.Euler(0, 0, angle-60);

         sprite.flipX = player.transform.position.x < transform.position.x;

         direction = player.transform.position - transform.position;
         direction.Normalize();
         float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;*/

        //Vector2.MoveTowards(transform.GetChild(0).transform.position, player.transform.position, 0.1f);
        //transform.GetChild(0).transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector2(player.transform.position.x, player.transform.position.y)), 24 * Time.deltaTime);
        //transform.GetChild(0).
        sprite.flipX = player.transform.position.x < transform.position.x;
        Vector2 targetPosition = player.transform.position + new Vector3(0,0.50f,0);
        Vector2 dir = targetPosition - (Vector2) transform.GetChild(0).transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bullet, transform.position, transform.GetChild(0).transform.rotation);
        //GameObject bulletGO = Instantiate(bullet, spawnBullet.transform.position, spawnBullet.transform.rotation);
        Destroy(bulletGO, 5f);
            //shoot = false;
            //shootAudio.Play();
    }
}
