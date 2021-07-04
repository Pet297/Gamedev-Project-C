using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffectsScript : MonoBehaviour
{
    HealthPointsScript hps;
    SpriteRenderer sr;

    float poisonEffectTime = -1;
    float strengthEffectTime = -1;
    float agilityEffectTime = -1;
    float frozenEffectTime = -1;
    float burningEffectTime = -1;
    float invisibleEffectTime = -1;

    float healthLossTiming1 = 0;
    float healthLossTiming2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        hps = gameObject.GetComponent<HealthPointsScript>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        healthLossTiming1 += Time.deltaTime * 5;
        healthLossTiming2 += Time.deltaTime * 10;

        if (burningEffectTime > 0f && frozenEffectTime > 0f)
        {
            burningEffectTime = -1f;
            frozenEffectTime = -1f;
        }

        poisonEffectTime -= Time.deltaTime;
        strengthEffectTime -= Time.deltaTime;
        agilityEffectTime -= Time.deltaTime;
        frozenEffectTime -= Time.deltaTime;
        burningEffectTime -= Time.deltaTime;
        invisibleEffectTime -= Time.deltaTime;

        if (healthLossTiming1 > 1f)
        {
            healthLossTiming1 -= 1f;
            if (poisonEffectTime > 0f) hps.Decrease(1);
        }

        if (healthLossTiming2 > 1f)
        {
            healthLossTiming2 -= 1f;
            if (burningEffectTime > 0f) hps.Decrease(1);
        }

        float r = 1;
        float g = 1;
        float b = 1;
        float a = 1;

        if (poisonEffectTime > 0) { r *= 0.5f; b *= 0.5f; }
        if (strengthEffectTime > 0) { g *= 0.8f; }
        if (agilityEffectTime > 0) { r *= 0.8f; }
        if (frozenEffectTime > 0) { r *= 0.3f; }
        if (burningEffectTime > 0) { b *= 0.25f; g *= 0.5f; }
        if (invisibleEffectTime > 0) { a *= 0.25f; }

        sr.color = new Color(r, g, b, a);
    }

    public float JumpHeight => ((agilityEffectTime > 0f) ? 2 : 1);
    public float Strength => ((strengthEffectTime > 0f) ? 2 : 1);
    public float MoveSpeed => ((frozenEffectTime > 0f) ? 0.33f : 1);
    public bool Visible => invisibleEffectTime < 0f;

    public void ApplyPoison(float time) { if (time > poisonEffectTime) poisonEffectTime = time; }
    public void ApplyStrength(float time) { if (time > strengthEffectTime) strengthEffectTime = time; }
    public void ApplyAgility(float time) { if (time > agilityEffectTime) agilityEffectTime = time; }
    public void ApplyFrozen(float time) { if (time > frozenEffectTime) frozenEffectTime = time; }
    public void ApplyBurning(float time) { if (time > burningEffectTime) burningEffectTime = time; }
    public void ApplyInvisible(float time) { if (time > invisibleEffectTime) invisibleEffectTime = time; }

    public void CancelBurning() => burningEffectTime = -1;
    public void CancelFrozen() => frozenEffectTime = -1;
    public void CancelPoison() => poisonEffectTime = -1;
}
