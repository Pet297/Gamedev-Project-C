using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerScript : MonoBehaviour
{
    public LayerMask AffectedLayer;
    public int DealtDamage = 20;

    BoxCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = (gameObject.GetComponent(typeof(BoxCollider2D)) as BoxCollider2D);
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(collider.bounds.size.x / 2, collider.bounds.size.y / 2), AffectedLayer);
        foreach (Collider2D c in colliders)
        {
            HealthPointsScript hps = (c.GetComponent(typeof(HealthPointsScript)) as HealthPointsScript);
            if (hps != null) hps.GetHit(DealtDamage);
        }
    }
}
