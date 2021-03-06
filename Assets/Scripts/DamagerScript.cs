using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerScript : MonoBehaviour
{
    public LayerMask AffectedLayer;
    public int DealtDamage = 20;
    public bool SelfDestroy = true;

    Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = (gameObject.GetComponent(typeof(Collider2D)) as Collider2D);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        int realDmg = DealtDamage;
        PotionEffectsScript pes = gameObject?.transform?.parent?.gameObject?.GetComponent<PotionEffectsScript>();

        if (pes != null) realDmg = (int)(realDmg * pes.Strength);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(gameObject.transform.position.x + collider.offset.x, gameObject.transform.position.y + collider.offset.y), new Vector2(collider.bounds.size.x*2, collider.bounds.size.y*2), 0, AffectedLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if ((AffectedLayer.value & (1 << colliders[i].gameObject.layer)) > 0)
            {
                HealthPointsScript hps = (colliders[i].GetComponent(typeof(HealthPointsScript)) as HealthPointsScript);
                if (hps != null) hps.GetHit(realDmg);
                if (SelfDestroy) this.gameObject.SetActive(false);
            }
        }
    }
}
