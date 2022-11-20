using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetController : MonoBehaviour
{
    public int PetN;
    public GameObject player;
    public Animator anim;

    public float[] speeds;
    public float[] attackSpeeds;
    public int[] attackMin;
    public int[] attackMax;
    float initialSpeed;
    public float keepDistance = .3f;
    public AnimationClip[] idleAnimations;
    public AnimationClip[] runAnimations;
    AnimatorOverrideController animatorOverrideController;
    public SpriteRenderer sprite;
    public GameObject floatingDamage;
    float input_x;
    float input_y;
    public static float speed;
    float lastDirection_x;
    float lastDirection_y;

    Vector2 petPos;
    Vector2 playerPos;
    Vector2 direction;
    public bool canAttack = false;
    public bool stayClose = true;
    public GameObject enemyTarget;
    Rigidbody2D rb;
    bool allowAttack;
    //NavMeshAgent agent;
    void Start()
    {
        allowAttack = true;
        speed = speeds[PetN];
        initialSpeed = speed;
        //agent.speed = speed;
        PetN = PlayerInfoVar.PetN;
        petPos = transform.position;

        rb = GetComponent<Rigidbody2D>();
        anim.SetInteger("pet", PetN);
        lastDirection_x = 1;
        lastDirection_y = 1;
        playerPos = SetDirection(lastDirection_x, lastDirection_y, player.transform.position);
        //agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;
        //agent.updateUpAxis = false;
    }

    void Update()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.transform.parent.GetComponent<Collider2D>(), true);
        Physics2D.IgnoreLayerCollision(9, 10, true);
        Physics2D.IgnoreLayerCollision(11,9,true);
        input_x = Input.GetAxis("Horizontal");
        input_y = Input.GetAxis("Vertical");
        if(Mathf.Sqrt(
                Mathf.Pow(petPos.x - direction.x, 2) +
                Mathf.Pow(petPos.y - direction.y, 2)) > 0.8)
                anim.SetBool("isMoving", true);
        else
            anim.SetBool("isMoving", false);

        if (input_x > 0 || input_x < 0) lastDirection_x = input_x;
        if(input_y > 0 || input_y < 0) lastDirection_y = input_y;

        petPos = transform.position;
        playerPos = SetDirection(lastDirection_x, lastDirection_y, player.transform.position);

        sprite.flipX = transform.position.x < player.transform.parent.position.x;
        if (enemyTarget == null)
        {
            enemyTarget = floatingDamage;
            speed = initialSpeed;
            //agent.speed = initialSpeed;
            //allowAttack = false;
            canAttack = false;
            stayClose = true;
            StartCoroutine(ExecuteAfterTime(3f));
        }
        direction = stayClose ? playerPos : (Vector2) enemyTarget.transform.position;
        

        //agent.SetDestination(direction);

        //if(enemyTarget.GetComponent<>)
        //rb.velocity = new Vector2(direction.x * input_x, direction.y * input_y);
        if (allowAttack) Attack();
    }

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(petPos, direction, speed * Time.deltaTime);
        if (!stayClose) anim.SetBool("isMoving",true);
        
    }

    Vector2 SetDirection(float input_x, float input_y, Vector2 playerPos){
           if (input_x < 0)
            {
                playerPos.x += keepDistance;
            }
            else if (input_x > 0)
            {
                playerPos.x -= keepDistance;
            }

            if (input_y < 0)
            {
                playerPos.y += keepDistance;
            }
            else if (input_y > 0)
            {
                playerPos.y -= keepDistance;
            }
        
        return playerPos;
    }

    void Attack()
    {
        if (!canAttack)
        {
            for (int i = 0; i < SpawnManager.enemies.Count; i++)
            {
                GameObject enemy = SpawnManager.enemies[i];

                if(Mathf.Sqrt(
                Mathf.Pow(playerPos.x - enemy.transform.position.x, 2) +
                Mathf.Pow(playerPos.y - enemy.transform.position.y, 2)) < 6)
                {
                    enemyTarget = enemy;
                    stayClose = false;
                    canAttack = true;
                    allowAttack = false;
                    //agent.speed = attackSpeeds[PetN];
                    speed = attackSpeeds[PetN];
                    break;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == enemyTarget && canAttack)
        {
            Debug.Log("Estocando");
            if (enemyTarget.GetComponent<EnemyController>() != null)
            {
               // Debug.Log("Estocand2o");
                if (!enemyTarget.GetComponent<EnemyController>().isDead)
                {
                    //agent.speed = initialSpeed;
                    speed = initialSpeed;
                    int dmg = Random.Range(attackMin[PetN], attackMax[PetN]);
                    enemyTarget.GetComponent<EnemyController>().life -= dmg;
                    //allowAttack = false;
                    canAttack = false;
                    stayClose = true;
                    GameObject dmgFloating = Instantiate(floatingDamage, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                    dmgFloating.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 9;
                    dmgFloating.transform.GetChild(0).GetComponent<TextMesh>().text = dmg + "";
                    Destroy(dmgFloating, .5f);
                    enemyTarget = floatingDamage;
                    StartCoroutine(ExecuteAfterTime(3f));
                }
            }else if (enemyTarget.GetComponent<Bomber>() != null){
                if (!enemyTarget.GetComponent<Bomber>().isDead)
                {
                    //agent.speed = initialSpeed;
                    speed = initialSpeed;
                    int dmg = Random.Range(attackMin[PetN], attackMax[PetN]);
                    enemyTarget.GetComponent<Bomber>().life -= dmg;
                    //allowAttack = false;
                    canAttack = false;
                    stayClose = true;
                    GameObject dmgFloating = Instantiate(floatingDamage, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                    dmgFloating.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 9;
                    dmgFloating.transform.GetChild(0).GetComponent<TextMesh>().text = dmg + "";
                    Destroy(dmgFloating, .5f);
                    enemyTarget = floatingDamage;
                    StartCoroutine(ExecuteAfterTime(3f));
                }
            }
            else if (enemyTarget.GetComponent<Bat>() != null)
            {
                if (!enemyTarget.GetComponent<Bat>().isDead)
                {
                    //agent.speed = initialSpeed;
                    speed = initialSpeed;
                    int dmg = Random.Range(attackMin[PetN], attackMax[PetN]);
                    enemyTarget.GetComponent<Bat>().life -= dmg;
                    //allowAttack = false;
                    canAttack = false;
                    stayClose = true;
                    GameObject dmgFloating = Instantiate(floatingDamage, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                    dmgFloating.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 9;
                    dmgFloating.transform.GetChild(0).GetComponent<TextMesh>().text = dmg + "";
                    Destroy(dmgFloating, .5f);
                    enemyTarget = floatingDamage;
                    StartCoroutine(ExecuteAfterTime(3f));
                }
            }
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        allowAttack = true;
    }
}
