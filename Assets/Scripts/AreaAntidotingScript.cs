using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAntidotingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collider = collision.collider.gameObject;
        AbstractAfectable af = collider.GetComponent<AbstractAfectable>();

        if (af != null)
        {
            af.OnAntidote();
        }
    }
}
