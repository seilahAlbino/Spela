using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public int numberofPlayer;
    public int playerN = 0;//remover do inspector
    public Animator anim;
    public int money = 11111111;
    public int[] lifePlayer;
    public int[] staminaPlayer;
    public int[] defPlayer;
    public float[] speedPlayer;
    public SpriteRenderer sprite;
    public GameObject maskPower;
    public GameObject[] power;
    public Sprite[] powerIconImage;
    public Sprite[] powerIconTransparentImage;
    public Sprite[] powerBackgroundImage;
    public int[] secondsPower;
    public int[] secondsAtivePower;
    public GameObject trail;
    public Transform spawnPower;
    [HideInInspector]
    public int life;
    [HideInInspector]
    public int def;
    [HideInInspector]
    public int stamina;
    [HideInInspector]
    public bool canSufferDamage;
    [HideInInspector]
    public int petN;
    public GameObject pauseObj;
    Vector2 moveInput;
    [HideInInspector]
    public int maxLife, maxDef, maxStamina;
    public GameObject powerParticle;
    bool canRechargeStamina;
    bool canDecreasePower;
    float powerSeconds;
    public float moveSpeed;
    Rigidbody2D rb;
    public static bool isSlowMode = false;

    [Header("Other Settings")]
    public Text txt;
    public Transform[] comingSoonPoints;
    public Text comingSoonTxt;
    public Transform WeaponMask;
    public Transform[] weapons;
    public Transform weaponsParentWorld;
    public Transform transformEx;
    public Transform transformPetEx;
    public Transform WeaponText;
    public Transform ItemText;
    public Slider lifeSlider;
    public Slider defSlider;
    public Slider staminaSlider;
    public Text lifeText;
    public Text defText;
    public Text staminaText;
    public Image powerBackground;
    public Image powerIcon;
    public Text powerTime;
    public Power powers;
    public GameObject floatingDamage;
    [HideInInspector]
    public static bool isPaused = false;
    Gradient gradient;
    GameObject shield;
    public bool canSufferDamagePower = true;
    public bool isInvisible = false;
    public GameObject circleHeal;
    GameObject circleHealGO;

    float offsetX = 1.85f;
    float offsetY = 0.76f;
    Vector3 offset = new Vector3(1.85f, 0.76f);
    void Start()
    {
        Screen.fullScreen = PlayerInfoVar.isFullscreen;
        trail.SetActive(false);
        powerParticle.GetComponent<ParticleSystemRenderer>().enabled = false;
        playerN = PlayerInfoVar.PlayerN;
        numberofPlayer = PlayerInfoVar.NumberPlayer;
        petN = PlayerInfoVar.PlayerN;
        moveSpeed = speedPlayer[playerN];
        life = lifePlayer[playerN];
        def = defPlayer[playerN];
        stamina = staminaPlayer[playerN];
        maxLife = life;
        maxDef = def;
        maxStamina = stamina;
        canSufferDamage = true;
        canRechargeStamina = true;
        powerBackground.sprite = powerBackgroundImage[playerN];
        powerIcon.sprite = powerIconTransparentImage[playerN];
        powerSeconds = secondsPower[playerN];
        powerTime.text = powerSeconds + " seg";
        canDecreasePower = true;
        powers = GetComponent<Power>();
        rb = GetComponent<Rigidbody2D>();
        anim.SetInteger("player", playerN);
        gradient = PlayerInfoVar.gradients[playerN];
        shield = transform.GetChild(6).gameObject;
        
        var main = powerParticle.GetComponent<ParticleSystem>().main;
        main.startColor = new ParticleSystem.MinMaxGradient(gradient);

        LoadGame();
    }

    private void FixedUpdate()
    {
        if (!isPaused) Move();
    }

    void Update()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        Physics2D.IgnoreLayerCollision(10, 3, true);
        //if(isInvisible) 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pauseObj.SetActive(!pauseObj.active);
            rb.velocity = new Vector2(0,0);
            /*if (Time.timeScale == 0)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;*/
        }
        if (!isPaused)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                SaveGame();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                LoadGame();
            }
            txt.text = money.ToString();
            WriteCommingSoon();

            GetItem();

            WritePlayerInfo();

            if (stamina < maxStamina && canRechargeStamina)
            {
                stamina++;
                canRechargeStamina = false;
                StartCoroutine(StaminaRecharge(0.4f));
            }

            if (!canSufferDamage)
            {
                StartCoroutine(ExecuteAfterTime(5f));
            }

            if (life <= 0)
            {
                SpawnManager.enemies.Clear();
                SpawnManager.countEnemies = 0;
                SpawnItensManager.itemsInScene.Clear();
                SpawnItensManager.countItems = 0;

                Restart();
                SaveGame();
            }
            PowerController();

        }
    }

    void Move() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        sprite.flipX = mousePos.x < screenPoint.x;
        anim.SetBool("isMoving", moveInput.x != 0 || moveInput.y != 0);
    }

    void WriteCommingSoon(){

        for(int i = 0; i < comingSoonPoints.Length; i++)
        {
            
            if (Vector2.Distance(comingSoonPoints[i].position, /*new Vector2(transform.position.x + 1.85f, transform.position.y + 0.76f)*/transform.position) < 1.5f){
                    comingSoonTxt.gameObject.SetActive(true);
                    break;
            }else{
                comingSoonTxt.gameObject.SetActive(false);
            }
        }
    }

    void WritePlayerInfo(){
        staminaSlider.value = (stamina * staminaSlider.maxValue) / maxStamina;
        lifeSlider.value = (life * lifeSlider.maxValue) / maxLife;
        defSlider.value = (def * defSlider.maxValue) / maxDef;
        lifeText.text = life + "/" + maxLife;
        defText.text = def + "/" + maxDef;
        staminaText.text = stamina + "/" + maxStamina;
    }

    void Restart(){
        Application.LoadLevel("Menu");
    }

    void PowerController(){
        if (powerSeconds > 0 && canDecreasePower){
            StartCoroutine(powerSecondsDecrease(Time.timeScale));
            canDecreasePower = false;
            powerBackground.transform.SetParent(maskPower.transform.parent);
            powerIcon.transform.SetParent(maskPower.transform.parent);
            maskPower.GetComponent<RectTransform>().sizeDelta = new Vector2(maskPower.GetComponent<RectTransform>().sizeDelta.x, maskPower.GetComponent<RectTransform>().sizeDelta.y+(328.53f/secondsPower[playerN]));
            powerBackground.transform.SetParent(maskPower.transform);
            powerIcon.transform.SetParent(maskPower.transform);

            if(secondsAtivePower[playerN] == powerSeconds) {
                moveSpeed = speedPlayer[playerN];

                if (isSlowMode) isSlowMode = false;
                if(!canSufferDamagePower) canSufferDamagePower = true;
                if(shield.activeInHierarchy) shield.SetActive(false);
                if(trail.activeInHierarchy) trail.SetActive(false);
                if (isInvisible)
                {
                    isInvisible = false;
                    anim.SetBool("isInvisible", false);
                    Physics2D.IgnoreLayerCollision(7, 3, false);
                }
                if(circleHealGO != null) Destroy(circleHealGO);
                if(circleHeal.activeInHierarchy) circleHeal.SetActive(false);
            }

        }
        powerTime.text = (int)powerSeconds > 0 ? (int)powerSeconds + " seg" : "Pronto";
        if((int) powerSeconds <= 0){
            
            powerParticle.GetComponent<ParticleSystemRenderer>().enabled = true;
            powerIcon.sprite = powerIconImage[playerN];
            if(Input.GetKey(KeyCode.E) && stamina >= 20)
            {
                powerBackground.transform.SetParent(maskPower.transform.parent);
                powerIcon.transform.SetParent(maskPower.transform.parent);
                maskPower.GetComponent<RectTransform>().sizeDelta = new Vector2(maskPower.GetComponent<RectTransform>().sizeDelta.x, maskPower.GetComponent<RectTransform>().sizeDelta.y-328.53f);
                powerBackground.transform.SetParent(maskPower.transform);
                powerIcon.transform.SetParent(maskPower.transform);
                powerIcon.sprite = powerIconTransparentImage[playerN];
                powerSeconds = secondsPower[playerN];
                stamina-=20;
                Power();
                powerParticle.GetComponent<ParticleSystemRenderer>().enabled = false;
            }
        }
    }

    void GetItem(){
        //get potions
        for (int i = 0; i < SpawnItensManager.itemsInScene.Count; i++)
        {
            GameObject item = SpawnItensManager.itemsInScene[i];
            if (Vector2.Distance(item.transform.position, /*new Vector2(transform.position.x + 1.85f, transform.position.y + 0.76f)*/transform.position) < 1.5)
            {
                ItemText.gameObject.SetActive(true);
                ItemText.transform.GetComponent<MeshRenderer>().sortingOrder = 8;
                ItemText.gameObject.GetComponent<TextMesh>().text = item.GetComponent<ItemInformation>().name + "\n[F]";
                ItemText.SetParent(item.transform);
                ItemText.position = new Vector3(item.transform.position.x + item.GetComponent<ItemInformation>().xTextMargin, item.transform.position.y + .254f, 0);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if(item.name == "LifeRecharge(Clone)")
                    {
                        life+=3;
                        if (life > maxLife) life = maxLife;
                    } else if (item.name == "StaminaRecharge(Clone)")
                    {
                        stamina += 50;
                        if (stamina > maxStamina) stamina = maxStamina;
                    } else if (item.name == "ArmorRecharge(Clone)")
                    {
                        def += 3;
                        if (def > maxDef) def = maxDef;
                    }
                    ItemText.gameObject.SetActive(false);
                    ItemText.SetParent(null);
                    SpawnItensManager.itemsInScene.Remove(item);
                    SpawnItensManager.countItems--;
                    Destroy(item);
                }
                break;
            }
            else
            {
                ItemText.gameObject.SetActive(false);
                ItemText.SetParent(null);
            }
        }

        //get weapons
        for (int i = 0; i < weapons.Length; i++)
        {
            
            if (Vector2.Distance(weapons[i].position, /*new Vector2(transform.position.x + 1.85f, transform.position.y + 0.76f)*/transform.position) < 1.5){

                if (Input.GetKeyDown(KeyCode.F) && weapons[i].transform.parent.name != "Weapon")
                {
                    foreach (Transform child in WeaponMask)
                    {//remove equipped weapon
                        child.SetParent(weaponsParentWorld);
                        child.transform.rotation = Quaternion.identity;
                        child.GetComponent<SpriteRenderer>().flipY = false;
                    }
                    weapons[i].SetParent(WeaponMask, false);
                    Vector3 temp = new Vector3(0.025f, 0, 0);
                    weapons[i].position = transformEx.position;

                    WeaponText.SetParent(null);

                    break;
                }

                if (!weapons[i].parent.parent.parent){
                    WeaponText.gameObject.SetActive(true);
                    WeaponText.transform.GetComponent<MeshRenderer>().sortingOrder = 8;
                    WeaponText.gameObject.GetComponent<TextMesh>().text = weapons[i].GetComponent<GunInformation>().name + "\n[F]";
                    Rarity weaponRarity = weapons[i].GetComponent<GunInformation>().rarity;
                    switch (weaponRarity)
                    {
                        case Rarity.Comum:
                            WeaponText.gameObject.GetComponent<TextMesh>().color = GunInformation.rarityColors[0];
                            break;
                        case Rarity.Rara:
                            WeaponText.gameObject.GetComponent<TextMesh>().color = GunInformation.rarityColors[1];
                            break;
                        case Rarity.Epica:
                            WeaponText.gameObject.GetComponent<TextMesh>().color = GunInformation.rarityColors[2];
                            break;
                        case Rarity.Lendaria:
                            WeaponText.gameObject.GetComponent<TextMesh>().color = GunInformation.rarityColors[3];
                            break;
                    }

                    WeaponText.SetParent(weapons[i]);
                    WeaponText.position = new Vector3(weapons[i].position.x + weapons[i].gameObject.GetComponent<GunInformation>().xTextMargin, weapons[i].position.y + .254f, 0);
                    break;
                }
                else
                {
                    WeaponText.gameObject.SetActive(false);
                    WeaponText.SetParent(null);
                }
            }
            else
            {
                WeaponText.gameObject.SetActive(false);
                WeaponText.SetParent(null);
            }
        }
    }
    void Power(){
        switch(playerN){
            case 0: powers.Power1(power[0], spawnPower);
                    break;
            case 1: moveSpeed = 13f;
                    trail.SetActive(true);
                    break;
            case 2:
                    isSlowMode = true;
                    break;
            case 3:
                circleHealGO = Instantiate(circleHeal, transform, true);
                circleHealGO.transform.SetParent(null);
                circleHealGO.transform.position = /*transform.position + offset*/spawnPower.position;
                break;
            case 4:
                for(int i = 0; i < SpawnManager.countEnemies; i++)
                {
                    GameObject enemy = SpawnManager.enemies[i];
                    if(Vector2.Distance(/*transform.position + offset*/spawnPower.position, enemy.transform.position) < 10)
                    {
                        enemy.GetComponent<EnemyController>().isKnocked = true;
                        canSufferDamagePower = false;
                    }
                }
                shield.SetActive(true);
                break;
            case 5:
                isInvisible = true;
                anim.SetBool("isInvisible", true);
                Physics2D.IgnoreLayerCollision(7, 3, true);
                break;
            case 6:
                break;

        }
    }

    public void SaveGame()
    {
        //ES3.Save<Transform>("Player"+numberofPlayer+"_playerPosition", transform);
        //ES3.Save<Transform>("Player" + numberofPlayer + "_petPosition", GameObject.Find("Pet").transform);
        Debug.Log(numberofPlayer);
        ES3.Save<int>("Player" + numberofPlayer + "_playerMoneyKrokonius", money);
        ES3.Save<string>("Player" + numberofPlayer + "_hour", System.DateTime.Now.ToString("HH:mm:ss"));
        ES3.Save<string>("Player" + numberofPlayer + "_date", System.DateTime.Now.ToString("dd/MM/yyyy"));
        ES3.Save<string>("Player" + numberofPlayer + "_dungeonLevel", Portal.dungeonLevel);
        for (int i = 0; i < GetComponent<UpgradesMenu>().itens.Length; i++)
        {
            ItemInformation itemInfo = GetComponent<UpgradesMenu>().itens[i].GetComponent<ItemInformation>();
            ES3.Save<int>("Player" + numberofPlayer + "_"+itemInfo.name + "_lvl", itemInfo.currentUpgrade);
        }

        Debug.Log("Saved");
    }
    void LoadGame()
    {
        //ES3.Save<Transform>("Player" + numberofPlayer + "_playerPosition", transform);

        if (PlayerInfoVar.typeStart.Equals("loadGame"))
        {
            //ES3.Save<int>("Player" + numberofPlayer + "_playerMoneyKrokonius", 9999); //for debug
           /* if (ES3.KeyExists("Player" + numberofPlayer + "_playerPosition"))
                transform.position = ES3.Load<Transform>("Player" + numberofPlayer + "_playerPosition").position;*/
            if (ES3.KeyExists("Player" + numberofPlayer + "_playerMoneyKrokonius")){
                money = ES3.Load<int>("Player" + numberofPlayer + "_playerMoneyKrokonius");
                txt.text = money.ToString();
            }
            if (ES3.KeyExists("Player" + numberofPlayer + "_petPosition"))
                //GameObject.Find("Pet").transform.position = ES3.Load<Transform>("Player" + numberofPlayer + "_petPosition").position;
            for (int i = 0; i < GetComponent<UpgradesMenu>().itens.Length; i++)
            {
                ItemInformation itemInfo = GetComponent<UpgradesMenu>().itens[i].GetComponent<ItemInformation>();
                if(ES3.KeyExists("Player" + numberofPlayer + "_"+itemInfo.name + "_lvl"))
                {
                    itemInfo.currentUpgrade = ES3.Load<int>("Player" + numberofPlayer + "_"+itemInfo.name + "_lvl");
                    if (itemInfo.name.Equals("LifeUp"))//sai
                    {
                        maxLife = lifePlayer[playerN]  + (itemInfo.currentUpgrade * itemInfo.newUpgrade);
                        life += itemInfo.currentUpgrade * itemInfo.newUpgrade;
                    }
                    else if (itemInfo.name.Equals("PetAttackUp"))
                    {
                       // GameObject.Find("Pet").GetComponent<PetController>().attackMin[GameObject.Find("Pet").GetComponent<PetController>().PetN] += itemInfo.currentUpgrade * itemInfo.newUpgrade;
                        //GameObject.Find("Pet").GetComponent<PetController>().attackMax[GameObject.Find("Pet").GetComponent<PetController>().PetN] += itemInfo.currentUpgrade * itemInfo.newUpgrade;
                    }
                }
            }
            Debug.Log("Loaded");
        }
    }

    public void sufferDamage(int damage){
        if(canSufferDamagePower && canSufferDamage){
            canSufferDamage = false;
            while(damage > 0){
            if(def > 0){
                def--;
                damage--;
            }else{
                if(life > 0){
                    life--;
                    damage--;
                }  
            }
            }
        }
            
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        canSufferDamage = true;
    }

    IEnumerator StaminaRecharge(float time)
    {
        yield return new WaitForSeconds(time);

        canRechargeStamina = true;
    }

    IEnumerator powerSecondsDecrease(float time)
    {
        yield return new WaitForSeconds(time);
        canDecreasePower = true;
        powerSeconds--;
    }
}