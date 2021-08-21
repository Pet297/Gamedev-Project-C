using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomParentScript : MonoBehaviour
{
    public GameObject Player;
    public int RoomX;
    public int RoomY;

    PlayerController pc;
    bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        pc = Player.GetComponent<PlayerController>();
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool activate = pc.MaxOneRoomAway(RoomX, RoomY);
        if (activated != activate)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(activate);
            }
        }
        activated = activate;
    }
}
