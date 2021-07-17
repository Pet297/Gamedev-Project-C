using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffectorScript : MonoBehaviour
{
    public LayerMask AffectedLayer;

    public enum PotionEffect { Poison, Strength, Agility, Frozen, Burning }
    public PotionEffect Type = PotionEffect.Strength;
    public float Duration = 5f;

    Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = (gameObject.GetComponent(typeof(Collider2D)) as Collider2D);
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(collider.bounds.size.x / 2, collider.bounds.size.y / 2), AffectedLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if ((AffectedLayer.value & (1 << colliders[i].gameObject.layer)) > 0)
            {
                PotionEffectsScript pe = colliders[i].GetComponent<PotionEffectsScript>();

                if (pe != null)
                {
                    switch (Type)
                    {
                        case PotionEffect.Agility:
                            pe.ApplyAgility(Duration);
                            break;
                        case PotionEffect.Burning:
                            pe.ApplyBurning(Duration);
                            break;
                        case PotionEffect.Frozen:
                            pe.ApplyFrozen(Duration);
                            break;
                        case PotionEffect.Poison:
                            pe.ApplyPoison(Duration);
                            break;
                        case PotionEffect.Strength:
                            pe.ApplyStrength(Duration);
                            break;
                    }
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collider = collision.collider.gameObject;
        PotionEffectsScript pe = collider.GetComponent<PotionEffectsScript>();

        if (pe != null)
        {
            switch (Type)
            {
                case PotionEffect.Agility:
                    pe.ApplyAgility(Duration);
                    break;
                case PotionEffect.Burning:
                    pe.ApplyBurning(Duration);
                    break;
                case PotionEffect.Frozen:
                    pe.ApplyFrozen(Duration);
                    break;
                case PotionEffect.Poison:
                    pe.ApplyPoison(Duration);
                    break;
                case PotionEffect.Strength:
                    pe.ApplyStrength(Duration);
                    break;
            }
            this.gameObject.SetActive(false);
        }
    }
}
