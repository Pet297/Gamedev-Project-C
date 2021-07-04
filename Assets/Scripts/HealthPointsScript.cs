using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointsScript : MonoBehaviour
{
    int health = 100;
    public int MaxHealth = 100;

    public float InvincibilitySeconds = 1.0f;
    public GameObject HealthBar = null;
    private HealthBarScript hbs = null;

    //public GameObject Player = null;
    public int PointReward = 0;
    private GameScoreScript gss = null;

    float timeSinceHit = 0f;

    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
        if (HealthBar != null) hbs = HealthBar.GetComponent<HealthBarScript>();
        GameObject Player = GameObject.Find("Player");
        if (Player != null) gss = Player.GetComponent<GameScoreScript>();
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
        UpdateHealthBar();
    }
    public void Decrease(int value)
    {
        health -= value;
        if (health <= 0)
        {
            health = 0;
            GameObject.Destroy(gameObject);
            if (gss != null) gss.Increase(PointReward);
        }
        UpdateHealthBar();
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
        if (health > MaxHealth) health = MaxHealth;
        UpdateHealthBar();
    }
    public void IncreaseMax(int value)
    {
        MaxHealth += value;
        health += value;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        hbs?.SetPercentage(Percentage);
    }

    public int Health => health;
    public float Percentage => health / (float)MaxHealth;
    public float TimeSinceHit => timeSinceHit;
}
