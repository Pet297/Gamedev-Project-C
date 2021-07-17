using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIElementScript : MonoBehaviour
{
    public string ItemType = "NONE";
    public GameObject textObject;

    private Text displayText;

    // Start is called before the first frame update
    void Start()
    {
        displayText = textObject.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCount(int newCount)
    {
        if (newCount == 0) displayText.text = "---";
        else displayText.text = newCount.ToString();
    }
}
