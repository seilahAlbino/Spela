using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    bool hasSound, hasMusic, isFullscreen;
    public AudioSource musicAudio;
    GameObject menuPause;
    GameObject menuNextDungeon;
    int option = 0, optionMax = 2;
    public Animator anim;
    GameObject pet;
    public Animator animDungeon;
    public GameObject[] textsOptions;
    public GameObject[] textsOptionsDungeon;
    GameObject player;
    public GameObject[] menus;
    Color32 curColorSavedGame = new Color32(241, 137, 10, 255);
    bool OptionsMenuActive = false;
    int optionDungeon = 0;
    public Portal portal;
    public Transform location;
    private void Start()
    {
        pet = GameObject.Find("Pet");
        menuPause = GameObject.Find("PauseMenu");
        menuNextDungeon = GameObject.Find("NextDungeon");
        player = GameObject.Find("Player");
    }
    public void Update()
    {
        if (menuPause.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            if (!OptionsMenuActive)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    option++;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    option--;
                }

                if (option < 0) option = optionMax;
                if (option > optionMax) option = 0;

                anim.SetInteger("option", option);
                //Debug.Log(option);
                textsOptions[0].GetComponent<Text>().color = curColorSavedGame;
                textsOptions[1].GetComponent<Text>().color = curColorSavedGame;
                textsOptions[2].GetComponent<Text>().color = curColorSavedGame;

                textsOptions[option].GetComponent<Text>().color = Color.white;

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    switch (option)
                    {
                        case 0:
                            Resume();
                            break;
                        case 1:
                            OptionsMenu();
                            break;
                        case 2:
                            QuitGame();
                            break;
                    }
                }
            }
        }




        if (menuNextDungeon.transform.GetChild(0).gameObject.activeInHierarchy)
        {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    optionDungeon++;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    optionDungeon--;
                }

                if (optionDungeon < 0) optionDungeon = optionMax;
                if (optionDungeon > optionMax) optionDungeon = 0;

                animDungeon.SetInteger("option", optionDungeon);
            //Debug.Log(option);
            textsOptionsDungeon[0].GetComponent<Text>().color = curColorSavedGame;
            textsOptionsDungeon[1].GetComponent<Text>().color = curColorSavedGame;
            textsOptionsDungeon[2].GetComponent<Text>().color = curColorSavedGame;

            textsOptionsDungeon[optionDungeon].GetComponent<Text>().color = Color.white;

                if (Input.GetKeyDown(KeyCode.Return))
                {
                menuNextDungeon.transform.GetChild(0).gameObject.SetActive(false);
                    switch (optionDungeon)
                    {
                        case 0:
                            player.GetComponent<PlayerController>().SaveGame();
                            break;
                        case 1:
                            Portal.playInDungeon = false;
                            player.transform.position = location.position;
                            pet.transform.position = location.position;
                        break;
                        case 2:
                        portal.StartDungeon();
                            break;
                    }
                }
            
        }
    }

    public void Resume()
    {
        option = 0;
        PlayerController.isPaused = false;
        menuPause.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OptionsMenu()
    {
        option = 0;
        OptionsMenuActive = true;
        menus[0].SetActive(false);
        menus[1].SetActive(true);
    }

    public void Back()
    {
        OptionsMenuActive = false;
        menus[0].SetActive(true);
        menus[1].SetActive(false);
    }

    public void QuitGame()
    {
        player.GetComponent<PlayerController>().SaveGame();
        SpawnManager.enemies.Clear();
        SpawnManager.countEnemies = 0;
        SpawnItensManager.itemsInScene.Clear();
        SpawnItensManager.countItems = 0;
        Application.LoadLevel("Menu");
    }

    public void Sound(bool hasSound)
    {
        this.hasSound = hasSound;
        PlayerInfoVar.hasSound = hasSound;
    }

    public void Music(bool hasMusic)
    {
        this.hasMusic = hasMusic;
        PlayerInfoVar.hasMusic = hasMusic;
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
        PlayerInfoVar.isFullscreen = isFullscreen;
    }
}
