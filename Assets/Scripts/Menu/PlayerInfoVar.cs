using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfoVar : MonoBehaviour
{
    public static int NumberPlayer = 4;
    public static int PlayerN = 0;
    //fazer poderes (mago)
    public static int PetN = 3;
    public static string typeStart = "loadGame"; //   newGame/loadGame
    public int dungeonLvl;
    public int life;
    public int def;
    public int stamina;
    public float[] position;
    public static bool hasMusic = true, hasSound = true, isFullscreen = true;

    public static Gradient[] gradients = {new Gradient
    {
        alphaKeys = new[]
    {
            new GradientAlphaKey(1, 1f)
        },

        colorKeys = new[]
    {
            new GradientColorKey(new Color32(35, 71, 171, 255), 0f),
            new GradientColorKey(new Color32(25, 77, 219, 255), 0.25f),
            new GradientColorKey(new Color32(24, 100, 200, 255), 0.5f),
            new GradientColorKey(new Color32(25, 77, 219, 255), 0.75f),
            new GradientColorKey(new Color32(35, 71, 171, 255), 1f)
        }
    },
    new Gradient
    {
        alphaKeys = new[]
    {
            new GradientAlphaKey(1, 1f)
        },

        colorKeys = new[]
    {
            new GradientColorKey(new Color32(217, 68, 0, 255), 0f),
            new GradientColorKey(new Color32(214, 86, 13, 255), 0.25f),
            new GradientColorKey(new Color32(180, 103, 20, 255), 0.5f),
            new GradientColorKey(new Color32(214, 86, 13, 255), 0.75f),
            new GradientColorKey(new Color32(217, 68, 0, 255), 1f)
        }
    },
    new Gradient
    {
        alphaKeys = new[]
    {
            new GradientAlphaKey(1, 1f)
        },

        colorKeys = new[]
    {
            new GradientColorKey(new Color32(255, 233, 23, 255), 0f),
            new GradientColorKey(new Color32(217, 197, 3, 255), 0.25f),
            new GradientColorKey(new Color32(217, 194, 40, 255), 0.5f),
            new GradientColorKey(new Color32(217, 197, 3, 255), 0.75f),
            new GradientColorKey(new Color32(255, 233, 23, 255), 1f)
        }
    },
    new Gradient
    {
        alphaKeys = new[]
    {
            new GradientAlphaKey(1, 1f)
        },

        colorKeys = new[]
    {
            new GradientColorKey(new Color32(231, 104, 198, 255), 0f),
            new GradientColorKey(new Color32(253, 80, 196, 255), 0.25f),
            new GradientColorKey(new Color32(255, 0, 217, 255), 0.5f),
            new GradientColorKey(new Color32(253, 80, 196, 255), 0.75f),
            new GradientColorKey(new Color32(231, 104, 198, 255), 1f)
        }
    },
    new Gradient
    {
        alphaKeys = new[]
    {
            new GradientAlphaKey(1, 1f)
        },

        colorKeys = new[]
    {
            new GradientColorKey(new Color32(71, 218, 241, 255), 0f),
            new GradientColorKey(new Color32(29, 216, 243, 255), 0.25f),
            new GradientColorKey(new Color32(0, 191, 219, 255), 0.5f),
            new GradientColorKey(new Color32(29, 216, 243, 255), 0.75f),
            new GradientColorKey(new Color32(71, 218, 241, 255), 1f)
        }
    },
    new Gradient
    {
        alphaKeys = new[]
    {
            //new GradientAlphaKey(0, 0f),
            new GradientAlphaKey(1, 1f)
        },

        colorKeys = new[]
    {
            new GradientColorKey(new Color32(255, 255, 255, 255), 0f),
            new GradientColorKey(new Color32(197, 197, 197, 255), 0.25f),
            new GradientColorKey(new Color32(168, 168, 168, 255), 0.5f),
            new GradientColorKey(new Color32(197, 197, 197, 255), 0.75f),
            new GradientColorKey(new Color32(255, 255, 255, 255), 1f)
        }
    },
    new Gradient
    {
        alphaKeys = new[]
    {
            new GradientAlphaKey(1, 1f)
        },

        colorKeys = new[]
    {
            new GradientColorKey(new Color32(192, 164, 64, 255), 0f),
            new GradientColorKey(new Color32(159, 133, 41, 255), 0.25f),
            new GradientColorKey(new Color32(132, 105, 8, 255), 0.5f),
            new GradientColorKey(new Color32(159, 133, 41, 255), 0.75f),
            new GradientColorKey(new Color32(192, 164, 64, 255), 1f)
        }
    }
    };

    public PlayerInfoVar(PlayerController player)
    {
        PlayerN = player.playerN;
        PetN = player.petN;
        life = player.life;
        def = player.def;
        stamina = player.stamina;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}


/*private int[] gradientValues = {35,71,171,
                                    25,77,219,
                                    24,100,200,
                                    
                                    217,68,0,
                                    214,86,13,
                                    180,103,20,
                                    
                                    255,233,23,
                                    217,197,3,
                                    217,194,40,
                                    
                                    231,104,198,
                                    253,80,196,
                                    255,0,217,
                                    
                                    71,218,241,
                                    29,216,243,
                                    0,191,219,
                                    
                                    255,255,255,
                                    197,197,197,
                                    168,168,168,
                                    
                                    192,164,64,
                                    159,133,41,
                                    132,105,8};

    public static Gradient[] gradients = new Gradient[8];

    void Start(){
        for(int i = 0; i < gradientValues.Length; i++){
            gradients[i] = new Gradient{
                        alphaKeys = new[]
                    {
                            new GradientAlphaKey(1, 1f)
                        },

                        colorKeys = new[]
                    {
                            new GradientColorKey(new Color32(192, 164, 64, 255), 0f),
                            new GradientColorKey(new Color32(159, 133, 41, 255), 0.25f),
                            new GradientColorKey(new Color32(132, 105, 8, 255), 0.5f),
                            new GradientColorKey(new Color32(159, 133, 41, 255), 0.75f),
                            new GradientColorKey(new Color32(192, 164, 64, 255), 1f)
                        }
                    };
        }
    }*/