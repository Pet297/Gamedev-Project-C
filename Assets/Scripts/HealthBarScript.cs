using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPercentage(float perc)
    {
        rt.localScale = new Vector3(perc, 1, 1);
    }
}
