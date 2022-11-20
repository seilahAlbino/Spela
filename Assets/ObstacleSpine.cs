using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpine : MonoBehaviour
{
    public int damage = 1;
    public int seconds = 2;
    public PlayerController playerControl;
    float timer;
    public float distance = 0.5f;
    bool isStoped = false;

    void Update(){
        float distanceWithPlayer = Vector2.Distance(transform.position,new Vector2(playerControl.transform.position.x+0.3f,playerControl.transform.position.y+0.3f));
        if(distanceWithPlayer < distance && !isStoped){
            playerControl.moveSpeed = 1;
            isStoped = true;
            timer = Time.time;
            playerControl.sufferDamage(damage);
        }

        if(isStoped) {
            if(Time.time - timer > seconds){
                playerControl.sufferDamage(damage);
                timer = Time.time;
            } 
            if(distanceWithPlayer >= distance){
                playerControl.moveSpeed = playerControl.speedPlayer[playerControl.playerN];
                isStoped = false;
            }
        }
    }

}
