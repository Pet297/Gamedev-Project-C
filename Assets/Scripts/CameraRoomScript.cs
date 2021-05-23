using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoomScript : MonoBehaviour
{
    public GameObject ToFollow;

    public float RoomSizeY = 20;
    public float RoomSizeX = 40;

    public float MoveDuration = 0.1f;
    bool moving = false;
    float phase = 0f;
    int fromX = 0;
    int fromY = 0;
    int toX = 0;
    int toY = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float playerX = ToFollow.transform.position.x;
        float playerY = ToFollow.transform.position.y;

        int roomX = (int)Math.Floor((playerX + RoomSizeX / 2) / RoomSizeX);
        int roomY = (int)Math.Floor((playerY + RoomSizeY / 2) / RoomSizeY);

        if (!moving)
        {
            if (roomX != fromX || roomY != fromY)
            {
                toX = roomX;
                toY = roomY;
                moving = true;
                phase = 0f;
            }
        }

        if (moving) phase += Time.deltaTime / MoveDuration;
        if (phase > 1f)
        {
            moving = false;
            fromX = toX;
            fromY = toY;
            phase = 0;
        }

        //phase = Mathf.SmoothDamp(0, 1, ref phase, MoveDuration);

        float roomfX = (1 - phase) * fromX + phase * toX;
        float roomfY = (1 - phase) * fromY + phase * toY;

        gameObject.transform.position = new Vector3(
            roomfX * RoomSizeX, roomfY * RoomSizeY + RoomSizeY / 18f, gameObject.transform.position.z
            );

        if (!moving)
        {
            fromX = roomX;
            fromY = roomY;
        }
    }
}
