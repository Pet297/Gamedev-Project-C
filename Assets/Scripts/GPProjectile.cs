using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPProjectile : MonoBehaviour
{
    float SpeedX = 5f;
    float SpeedY = 0f;
    float GravityY = -0.05f;

    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + SpeedX * Time.deltaTime, gameObject.transform.position.y + SpeedY * Time.deltaTime, 0);
        SpeedY += GravityY;
    }

    public void ResetTarget(GameObject go, float spdx, float spdy, float gravity)
    {
        Player = go;
        SpeedX = spdx;
        SpeedY = spdy;
        GravityY = gravity;
    }
}
