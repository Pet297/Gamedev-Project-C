using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSaveScript : MonoBehaviour
{
    public GameObject cat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveCat()
    {
        if (cat != null)
        {
            Object.Destroy(cat);
            //TODO: "Collect the cat"
        }
    }
}
