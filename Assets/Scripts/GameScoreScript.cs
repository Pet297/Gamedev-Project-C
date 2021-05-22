using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScoreScript : MonoBehaviour
{
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Increase(int value)
    {
        score += value;
    }
    public void Decrease(int value)
    {
        score -= value;
        if (score < 0) score = 0;
    }
    public int Score => score;
}
