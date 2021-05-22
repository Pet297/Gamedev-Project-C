using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    public string CollectibleType = "None";
    public LayerMask PlayerLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(0.5f, 0.5f), PlayerLayer);
        foreach (Collider2D c in colliders)
        {
            (c.GetComponent(typeof(CollectorScript)) as CollectorScript).Collect(CollectibleType);
        }
        if (colliders.Length > 0) GameObject.Destroy(gameObject);
    }
}
