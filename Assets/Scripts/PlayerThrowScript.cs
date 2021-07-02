using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowScript : MonoBehaviour
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

    public void Throw(float spdx, float spdy)
    {
        GameObject g = ops.GetPooledObject();
        g.transform.position = gameObject.transform.position;
        g?.GetComponent<Rigidbody2D>()?.AddForce(new Vector3(spdx,spdy,0));
    }
}
