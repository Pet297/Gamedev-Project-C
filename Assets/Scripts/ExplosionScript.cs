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

    public void StartExplosion()
    {
        Invoke("EndExplosion", 0.5f);
    }

    void EndExplosion()
    {
        gameObject.SetActive(false);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collider = collision.collider.gameObject;
        //AbstractAfectable af = collider.GetComponent<AbstractAfectable>();
        AbstractAfectable af = (AbstractAfectable)GetComponent(typeof(AbstractAfectable));

        if (af != null)
        {
            Debug.Log("exploding");
            af.OnExplode();
        }
    }
}
