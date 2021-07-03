using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    ObjectPoolScript ops;

    // Start is called before the first frame update
    void Start()
    {
        ops = GetComponent<ObjectPoolScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BombSpawned()
    {
        Invoke("Explode", 2);
    }

    void Explode()
    {
        gameObject.SetActive(false);

        GameObject g = ops.GetPooledObject();
        if (g != null) g.transform.position = gameObject.transform.position;
        g?.SetActive(true);
        g?.GetComponent<ExplosionScript>().StartExplosion();
    }
}
