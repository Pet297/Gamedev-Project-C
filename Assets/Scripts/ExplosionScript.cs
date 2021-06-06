using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
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
        GameObject collider = collision.collider.gameObject;
        IAfectable af = collider.GetComponent<IAfectable>();

        if (af != null)
        {
            af.OnExplode();
        }
    }
}
