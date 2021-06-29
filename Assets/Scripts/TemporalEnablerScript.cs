using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalEnablerScript : MonoBehaviour
{
    float enableTimer = 0;

    public float EnableTime = 0.3f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enableTimer -= Time.deltaTime;
        if (enableTimer <= 0) gameObject.SetActive(false);
    }

    public void EnableOnce()
    {
        enableTimer = EnableTime;
        gameObject.SetActive(true);
    }
}
