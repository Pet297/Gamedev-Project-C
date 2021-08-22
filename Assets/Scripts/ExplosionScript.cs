using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = (gameObject.GetComponent(typeof(Collider2D)) as Collider2D);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(gameObject.transform.position.x + collider.offset.x, gameObject.transform.position.y + collider.offset.y), new Vector2(collider.bounds.size.x / 2, collider.bounds.size.y / 2), int.MaxValue);
        for (int i = 0; i < colliders.Length; i++)
        {
            GameObject collider = colliders[i].gameObject;
            AbstractAfectable af = (AbstractAfectable)colliders[i].GetComponent(typeof(AbstractAfectable));

            if (af != null)
            {
                af.OnExplode();
            }

            HealthPointsScript hps = (colliders[i].GetComponent(typeof(HealthPointsScript)) as HealthPointsScript);
            if (hps != null) hps.GetHit(150);
        }
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
        /*GameObject collider = collision.collider.gameObject;
        //AbstractAfectable af = collider.GetComponent<AbstractAfectable>();
        AbstractAfectable af = (AbstractAfectable)GetComponent(typeof(AbstractAfectable));

        if (af != null)
        {
            Debug.Log("exploding");
            af.OnExplode();
        }*/
    }
}
