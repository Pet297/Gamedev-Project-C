using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMagicScript : MonoBehaviour
{
    Collider2D collider;
    public LayerMask AffectedLayer;

    // Start is called before the first frame update
    void Start()
    {
        collider = (gameObject.GetComponent(typeof(Collider2D)) as Collider2D);
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(gameObject.transform.position.x + collider.offset.x, gameObject.transform.position.y + collider.offset.y), new Vector2(collider.bounds.size.x * 2, collider.bounds.size.y * 2), 0, AffectedLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            AbstractAfectable af = colliders[i].GetComponent<AbstractAfectable>();
            if (af != null)
            {
                af.OnMagic();
            }
        }
    }

    /*public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collider = collision.collider.gameObject;
        AbstractAfectable af = collider.GetComponent<AbstractAfectable>();

        if (af != null)
        {
            af.OnMagic();
        }
    }*/
}
