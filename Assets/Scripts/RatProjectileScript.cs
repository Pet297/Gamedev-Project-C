using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatProjectileScript : MonoBehaviour
{
    float SpeedX = 5f;
    float SpeedY = 0f;
    float GravityY = -2f;

    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Translate(SpeedX * Time.deltaTime, SpeedY * Time.deltaTime, 0);
        SpeedY += GravityY * Time.deltaTime;

        if (Player != null)
        {
            Vector2 toTarget = new Vector2(Player.transform.position.x - transform.position.x, Player.transform.position.y - transform.position.y);
            if (toTarget.magnitude > 50) gameObject.SetActive(false);
        }
    }

    public void ResetTarget(GameObject go, float spdx, float spdy, float gravity)
    {
        Player = go;
        SpeedX = spdx;
        SpeedY = spdy;
        GravityY = gravity;
    }
}
