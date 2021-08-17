using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScoreScript : MonoBehaviour
{
    int score = 0;

    public GameObject textObject;

    public List<GameObject> saveCatIndicators;
    List<CollectCatScript> saveCatScripts;
    bool[] savedCats;

    private Text displayText;

    // Start is called before the first frame update
    void Awake()
    {
        displayText = textObject.GetComponent<Text>();
        saveCatScripts = new List<CollectCatScript>();

        foreach (GameObject go in saveCatIndicators)
        {
            saveCatScripts.Add(go.GetComponent<CollectCatScript>());
        }
        savedCats = new bool[saveCatIndicators.Count];
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

    public void SaveCat(int index)
    {
        savedCats[index] = true;
        saveCatScripts[index].GetCat();
    }

    public bool Cat0IsSaved => savedCats[0];

    private void UpdateText()
    {
        string toDisplay = (score > 0 ? score : -score).ToString();
        while (toDisplay.Length < 6) toDisplay = "0" + toDisplay;
        if (score < 0) toDisplay = "-" + toDisplay;
        displayText.text = toDisplay;
    }
}
