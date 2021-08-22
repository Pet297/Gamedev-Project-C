using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExitLevelScript : MonoBehaviour
{
    public GameObject canvasHud;
    public GameObject canvasEnd;
    public TextMeshProUGUI endText;

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
        if (col.gameObject.tag == "Player") Debug.Log("lol");
        canvasHud.SetActive(false);
        canvasEnd.SetActive(true);
        endText.text = col.gameObject.GetComponent<GameScoreScript>().GetCompletionText();
    }
}
