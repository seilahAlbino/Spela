using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public int nPlayers;
    public int nPets;
    public float[] xIdle;
    public float[] yIdle;
    public int[] priceCharacther;
    public Text[] savedGamesText;
    public Text[] optionsMenu;
    public Animator anim;
    public Animator animPet;
    public GameObject MainMenu;
    public GameObject StartMenu;
    public GameObject SavedGamesMenu;
    public GameObject OptionsMenu;
    public GameObject[] Menus;
    public Animator SavedGamesAnimator;
    public Animator MenuOptionsAnimator;
    public GameObject block;
    public Text priceText;
    public GameObject LoadingBar;
    int nPlayer = 0;
    int nPet = 0;
    int currentSavedGame = 0;
    public Text krokoniusTxt;
    int money;
    int buyTimes;
    Color32 curColorSavedGame = new Color32(241, 137, 10, 255);
    bool[] unlockedCharacthers;
    public Animator buyCharAnim;
    string typeStart = "newGame"; //newGame or loadGame
    int optionMenu = 0;
    int menu = 0;
    public Transform[] randomMasks;
    public AudioSource musicAudio;

    [HideInInspector]
    public bool hasMusic = true, hasSound = true, isFullscreen = true;

    private void Start()
    {
        Screen.fullScreen = isFullscreen;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        for (int i = 0; i < 4; i++)
        {
            if (ES3.KeyExists("Player" + i + "_hour")
                && ES3.KeyExists("Player" + i + "_date"))
            {
                Debug.Log(Menus[2]);
                Menus[1].transform
                    .GetChild(1)
                    .GetChild(i)
                    .GetChild(1).gameObject
                    .GetComponent<Text>().text =
                    ES3.Load<string>("Player" + i + "_date") + "\n" + ES3.Load<string>("Player" + i + "_hour");
            }
        }
    }

    private void Update()
    {

        if (menu == 1)
        {
            Debug.Log(menu);
            //Menu de escolha
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentSavedGame == 0) currentSavedGame = 1;
                else if (currentSavedGame == 2) currentSavedGame = 3;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentSavedGame == 1) currentSavedGame = 0;
                else if (currentSavedGame == 3) currentSavedGame = 2;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentSavedGame == 2) currentSavedGame = 0;
                else if (currentSavedGame == 3) currentSavedGame = 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentSavedGame == 0) currentSavedGame = 2;
                else if (currentSavedGame == 1) currentSavedGame = 3;
            }
            savedGamesText[0].GetComponent<Text>().color = curColorSavedGame;
            savedGamesText[1].GetComponent<Text>().color = curColorSavedGame;
            savedGamesText[2].GetComponent<Text>().color = curColorSavedGame;
            savedGamesText[3].GetComponent<Text>().color = curColorSavedGame;

            savedGamesText[currentSavedGame].GetComponent<Text>().color = Color.white;
            SavedGamesAnimator.SetInteger("curSavedGame", currentSavedGame);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if ((typeStart.Equals("loadGame") && ES3.KeyExists("Player" + currentSavedGame + "_hour"))
                || typeStart.Equals("newGame"))
                {
                    if (ES3.KeyExists("Player" + currentSavedGame + "_playerMoneyKrokonius"))
                        money = ES3.Load<int>("Player" + currentSavedGame + "_playerMoneyKrokonius");
                    else
                        money = 0;

                    if (ES3.KeyExists("Player" + currentSavedGame + "_unlockedCharacthers"))
                        unlockedCharacthers = ES3.Load<bool[]>("Player" + currentSavedGame + "_unlockedCharacthers");
                    else
                    {
                        unlockedCharacthers = new bool[nPlayers + 1];

                        for (int i = 1; i < unlockedCharacthers.Length; i++)
                            unlockedCharacthers[i] = false;
                        unlockedCharacthers[0] = true;

                        ES3.Save<bool[]>("Player" + currentSavedGame + "_unlockedCharacthers", unlockedCharacthers);
                    }

                    if (typeStart.Equals("newGame"))
                    {
                        money = 0;
                        unlockedCharacthers = new bool[nPlayers + 1];

                        for (int i = 1; i < unlockedCharacthers.Length; i++)
                            unlockedCharacthers[i] = false;
                        unlockedCharacthers[0] = true;

                    }

                    block.SetActive(false);
                    MenuMain();
                    MenuStart();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape)){
                MenuMain();
                currentSavedGame = 0;
            }
        }
        else if (menu == 2)
        {
            krokoniusTxt.text = money.ToString();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuMain();
            }
        }
        else if (menu == 0)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                optionMenu++;
                if (optionMenu >= optionsMenu.Length) optionMenu = 0;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                optionMenu--;
                if (optionMenu < 0) optionMenu = optionsMenu.Length - 1;
            }

            optionsMenu[0].GetComponent<Text>().color = curColorSavedGame;
            optionsMenu[1].GetComponent<Text>().color = curColorSavedGame;
            optionsMenu[2].GetComponent<Text>().color = curColorSavedGame;
            optionsMenu[3].GetComponent<Text>().color = curColorSavedGame;

            optionsMenu[optionMenu].GetComponent<Text>().color = Color.white;
            MenuOptionsAnimator.SetInteger("curOption", optionMenu);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (optionMenu)
                {
                    case 0:
                        MenuSavedGames("newGame");
                        break; 
                    case 1:
                        MenuSavedGames("loadGame");
                        break;
                    case 2:
                        MenuOptions();
                        break;
                    case 3:
                        Quit();
                        break;
                }
            }
        }
    }

    void StartGame(){
        if (unlockedCharacthers[nPlayer])
        {
            //Application.LoadLevel("SampleScene");
            PlayerInfoVar.PlayerN = nPlayer;
            PlayerInfoVar.PetN = nPet;
            PlayerInfoVar.NumberPlayer = currentSavedGame;
            PlayerInfoVar.typeStart = typeStart;
            PlayerInfoVar.hasMusic = hasMusic;
            PlayerInfoVar.hasSound = hasSound;
            PlayerInfoVar.isFullscreen = isFullscreen;
            ES3.Save<bool[]>("Player" + currentSavedGame + "_unlockedCharacthers", unlockedCharacthers);
            LoadingBar.SetActive(true);
            Debug.Log(nPet);
        }
    }

    void MenuMain(){
        menu = 0;
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        StartMenu.SetActive(false);
        SavedGamesMenu.SetActive(false);
        //moveMenu();
    }

    void MenuStart(){
        menu = 2;
        MainMenu.SetActive(false);
        StartMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        //moveMenu();
    }

    void MenuSavedGames(string typeStart)
    {
        menu = 1;
        this.typeStart = typeStart;
        currentSavedGame = 0;
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        SavedGamesMenu.SetActive(true);
        //moveMenu();
    }

    public void MenuOptions()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void Sound(bool hasSound)
    {
        this.hasSound = hasSound;
    }

    public void Music (bool hasMusic)
    {
        this.hasMusic = hasMusic;
        if (hasMusic)
        {
            musicAudio.Play();
        }
        else
        {
            musicAudio.Pause();
        }
    }

    public void Fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        this.isFullscreen = isFullscreen;
    }

    void moveMenu()
    {
        for(int i = 0; i < Menus.Length; i++)
            if (menu != i) Menus[i].transform.position = Vector3.Lerp(Menus[i].transform.position, randomMasks[Random.Range(0, randomMasks.Length-1)].transform.position, 0.05f);
            else Menus[i].transform.position = Vector3.Lerp(Menus[i].transform.position, new Vector3(0,0,0), 0.05f);
    }

    void NextCharacter(){
        nPlayer++;
        if(nPlayer > nPlayers){
            nPlayer = 0;
        }
        if (!unlockedCharacthers[nPlayer])
        {
            buyTimes = 0;
            priceText.text = priceCharacther[nPlayer].ToString();
            block.SetActive(true);
        }
        else
            block.SetActive(false);
        anim.SetInteger("player", nPlayer);
    }
    void BackCharacter()
    {
        nPlayer--;
        if (nPlayer < 0)
        {
            nPlayer = nPlayers;
        }
        if (!unlockedCharacthers[nPlayer])
        {
            buyTimes = 0;
            priceText.text = priceCharacther[nPlayer].ToString();
            block.SetActive(true);
        }
        else
            block.SetActive(false);
        anim.SetInteger("player", nPlayer);
    }

    void BackPet(){
        nPet--;
        if(nPet < 0){
            nPet = nPets;
        }
        animPet.SetInteger("pet", nPet);
    }

    void NextPet()
    {
        nPet++;
        if (nPet > nPets)
        {
            nPet = 0;
        }
        animPet.SetInteger("pet", nPet);
    }

    void BuyCharacther()
    {
        if(money >= priceCharacther[nPlayer])
        {
            buyTimes++;
            buyCharAnim.Play("Buy");
            if (buyTimes == 3)
            {
                money -= priceCharacther[nPlayer];
                unlockedCharacthers[nPlayer] = true;
                block.SetActive(false);
                ES3.Save<int>("Player" + currentSavedGame + "_playerMoneyKrokonius", money);
                ES3.Save<bool[]>("Player" + currentSavedGame + "_unlockedCharacthers", unlockedCharacthers);
            }
        }
    }

    public void Quit(){
        Application.Quit();
    }
}