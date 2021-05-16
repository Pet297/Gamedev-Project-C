using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoomScript : MonoBehaviour
{
    public GameObject ToFollow;

    public float RoomSizeY = 20;
    public float RoomSizeX = 40;

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

        gameObject.transform.position = new Vector3(
            roomX * RoomSizeX, roomY * RoomSizeY, gameObject.transform.position.z
            );
    }
}
