using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    float posX = 0;
    float posY = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float newPosX = gameObject.transform.position.x;
        float newPosY = gameObject.transform.position.y;

        gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(newPosY - posY, newPosX - posX));

        posX = newPosX;
        posY = newPosY;
    }
}
