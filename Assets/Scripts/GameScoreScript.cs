using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScoreScript : MonoBehaviour
{
    int score = 0;

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

    public void Increase(int value)
    {
        score += value;
        UpdateText();
    }
    public void Decrease(int value)
    {
        score -= value;
        UpdateText();
    }
    public int Score => score;

    private void UpdateText()
    {
        string toDisplay = (score > 0 ? score : -score).ToString();
        while (toDisplay.Length < 6) toDisplay = "0" + toDisplay;
        if (score < 0) toDisplay = "-" + toDisplay;
        displayText.text = toDisplay;
    }
}
