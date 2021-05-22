using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointsScript : MonoBehaviour
{
    int health = 100;
    public int MaxHealth = 100;

    public float InvincibilitySeconds = 1.0f;
    float timeSinceHit = 0f;

    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceHit += Time.deltaTime;
    }

    public void Increase(int value)
    {
        health += value;
        if (health > MaxHealth) health = MaxHealth;
    }
    public void Decrease(int value)
    {
        health -= value;
        if (health < 0)
        {
            health = 0;
            GameObject.Destroy(gameObject);
        }
    }
    public void GetHit(int damage)
    {
        if (timeSinceHit > InvincibilitySeconds)
        {
            Decrease(damage);
            timeSinceHit = 0;
        }
    }

    public void IncreaseRelative(float percentage)
    {
        health += (int)(percentage * MaxHealth);
    }
    public void IncreaseMax(int value)
    {
        MaxHealth += value;
        health += value;
    }

    public int Health => health;
    public float Percentage => health / (float)MaxHealth;
    public float TimeSinceHit => timeSinceHit;
}
