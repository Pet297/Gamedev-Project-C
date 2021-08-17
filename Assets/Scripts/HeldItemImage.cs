using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HeldItemImage : MonoBehaviour
{
    public List<string> types;
    public List<Sprite> Images;

    Image imageScript;

    // Start is called before the first frame update
    void Start()
    {
        imageScript = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(string item)
    {
        int i = types.IndexOf(item);
        if (i == -1) i = 0;
        imageScript.sprite = Images[i];
    }
}
