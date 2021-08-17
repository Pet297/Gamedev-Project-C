using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSaveScript : MonoBehaviour
{
    public GameObject cat;
    public int index;

    private GameScoreScript gss = null;

    // Start is called before the first frame update
    void Start()
    {
        GameObject Player = GameObject.FindWithTag("Player");
        if (Player != null) gss = Player.GetComponent<GameScoreScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveCat()
    {
        if (cat != null)
        {
            GameObject.Destroy(cat);
            gss.SaveCat(index);
        }
    }
}
