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

    int CatCount
    {
        get
        {
            int ct = 0;
            foreach (bool b in savedCats) if (b) ct++;
            return ct;
        }
    }

    public string GetCompletionText()
    {
        return $"You won!!\n\nYour got score of {score}.\nMax possible score is 1086500.\n(With easter egg, it's 1142500)\n\nIn total, you saved {CatCount} cats out of 10.\n\nThe ending mechanic isn't implemented. If you saved a lot\nof cats, people from your vilage celebrate you.\nIf you didn't kill almost any rodent and didn't steal their loot,\nsome of them became friendlier.\nIf you killed 1 cat, it's an accident.\nIf you killed more (or even all 6), you are a monster.\n\nTo exit, press escape.";
    }
    public string GetDeathMessage()
    {
        return $"You died as a hero!!\n\nYour got score of {score}\n\nIn total, you saved {CatCount} cats out of 10.{(CatCount > 0 ? $"\nThose cats were recaptured and continue to serve the rodents." : "")}\n\nThe game over mechanic isn't implemented.\n\nTo exit, press escape.\n";
    }
}
