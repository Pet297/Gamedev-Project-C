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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.collider.gameObject.layer) & PlayerLayer.value) > 0)
        {
            CollectorScript cs = collision.collider.GetComponent<CollectorScript>();
            cs.Collect(CollectibleType);
            GameObject.Destroy(gameObject);
        }
    }
}
