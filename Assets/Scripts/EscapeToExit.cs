using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeToExit : MonoBehaviour
{
    public GameObject CanvasHud;
    public GameObject CanvasIntro;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        CanvasIntro.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (CanvasIntro.activeInHierarchy)
            {
                CanvasIntro.SetActive(false);
                CanvasHud.SetActive(true);
                Player.GetComponent<PlayerController>().enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
}
