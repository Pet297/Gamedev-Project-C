using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffectorScript : MonoBehaviour
{
    public enum PotionEffect { Poison, Strength, Agility, Frozen, Burning }
    public PotionEffect Type = PotionEffect.Strength;
    public float Duration = 5f;

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
