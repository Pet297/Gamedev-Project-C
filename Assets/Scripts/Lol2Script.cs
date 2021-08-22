using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lol2Script : MonoBehaviour
{
    public float Gravity = 0.25f;
    public float SpdY = 0f;
    public float SpdX = 0f;
    public float ThrowTime = 0.5f;
    public float WaitTime = 0.5f;

    public float ThrowSpdY = 4f;
    public float ThrowSpdX = 1f;

    public GameObject Rest1 = null;
    public GameObject Rest2 = null;

    int phase = 0;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (phase % 2 == 0 && timer > ThrowTime)
        {
            phase++;
            timer -= ThrowTime;
        }
        else if (phase % 2 == 1 && timer > WaitTime)
        {
            phase++;
            SpdY = ThrowSpdY;
            if (phase == 2) SpdX = ThrowSpdX;
            else SpdX = -ThrowSpdX;
            timer -= WaitTime;
        }
        phase %= 4;

        if (phase == 1) gameObject.transform.position = Rest1.transform.position;
        else if (phase == 3) gameObject.transform.position = Rest2.transform.position;
        else
        {
            SpdY -= Gravity;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + SpdX * Time.fixedDeltaTime, gameObject.transform.position.y + SpdY * Time.fixedDeltaTime, 0);
        }
    }
}
