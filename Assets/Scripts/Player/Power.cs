using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{

    public void Power1(GameObject item, Transform pos){
        for(int i = 0; i < 18; i++) {
            Instantiate(item, pos.position, item.transform.rotation);
            item.transform.Rotate(0,0,20);   
        }
    }

    public void Power2(GameObject item, Transform pos){
        for(int i = 0; i < 36; i++) {
            Instantiate(item, pos.position, item.transform.rotation);
            item.transform.Rotate(0,0,10);   
        }
    }
}
