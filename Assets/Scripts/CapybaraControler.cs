using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapybaraControler : MonoBehaviour
{
    private GameObject Player;
    public GameObject Text;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 toTarget = new Vector2(Player.transform.position.x - transform.position.x, Player.transform.position.y - transform.position.y);
        Text.SetActive(toTarget.magnitude < 2);
    }
}
