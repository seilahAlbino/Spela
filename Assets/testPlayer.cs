using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 5;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * 5;
        transform.position = new Vector3(transform.position.x + x, transform.position.y + y);
    }
}
