using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBlockerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<GameScoreScript>().Cat0IsSaved)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
