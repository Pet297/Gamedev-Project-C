using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffectsScript : MonoBehaviour
{
    HealthPointsScript hps;

    float poisonEffectTime = -1;
    float strengthEffectTime = -1;
    float agilityEffectTime = -1;
    float frozenEffectTime = -1;
    float burningEffectTime = -1;

    float healthLossTiming1 = 0;
    float healthLossTiming2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        hps = (gameObject.GetComponent(typeof(HealthPointsScript)) as HealthPointsScript);
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
    }

    public float JumpHeight => ((agilityEffectTime > 0f) ? 2 : 1);
    public float Strength => ((strengthEffectTime > 0f) ? 2 : 1);
    public float MoveSpeed => ((frozenEffectTime > 0f) ? 0.33f : 1);

    public void ApplyPoison(float time) { if (time > poisonEffectTime) poisonEffectTime = time; }
    public void ApplyStrength(float time) { if (time > strengthEffectTime) strengthEffectTime = time; }
    public void ApplyAgility(float time) { if (time > agilityEffectTime) agilityEffectTime = time; }
    public void ApplyFrozen(float time) { if (time > frozenEffectTime) frozenEffectTime = time; }
    public void ApplyBurning(float time) { if (time > burningEffectTime) burningEffectTime = time; }
}
