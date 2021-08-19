using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IwtWaterScript : AbstractAfectable
{
    public int CurrentState = 0;
    public GameObject LevelCollison;

    private ObjectPoolScript[] pools;
    private Animator animator;
    private DamagerScript damager;

    float spawnTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        damager = gameObject.GetComponent<DamagerScript>();
        pools = GetComponents<ObjectPoolScript>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("State", CurrentState);
        spawnTimer += Time.deltaTime;
    }

    public override void OnHeat()
    {
        if (CurrentState == 1)
        {
            GameObject.Destroy(gameObject);

            GameObject exp = pools[1].GetPooledObject();
            if (exp != null)
            {
                exp.transform.position = gameObject.transform.position;
                exp.SetActive(true);
                exp.GetComponent<ExplosionScript>().StartExplosion();
            }
        }
        else if (CurrentState == 2) SetState(0);
    }
    public override void OnFreeze()
    {
        if (CurrentState == 0) SetState(2);
        else if (CurrentState == 3 && spawnTimer > 2.5f)
        {
            spawnTimer = 0f;

            GameObject mgc = pools[0].GetPooledObject();

            if (mgc != null)
            {
                mgc.transform.position = gameObject.transform.position;
                mgc.SetActive(true);
            }
        }
    }
    public override void OnPoison()
    {
        if (CurrentState == 0) SetState(1);
        else if (CurrentState == 3) SetState(0);
    }
    public override void OnMagic()
    {
        if (CurrentState == 0) SetState(3);
    }
    public override void OnAntidote()
    {
        if (CurrentState == 1) SetState(0);
    }
    public override void OnExplode()
    {
        if (CurrentState == 2) GameObject.Destroy(gameObject);
    }

    private void SetState(int state)
    {
        CurrentState = state;

        switch (state)
        {
            case 0:
                LevelCollison.SetActive(false);
                damager.enabled = false;
                break;
            case 1:
                LevelCollison.SetActive(false);
                damager.enabled = true;
                break;
            case 2:
                LevelCollison.SetActive(true);
                damager.enabled = false;
                break;
            case 3:
                LevelCollison.SetActive(false);
                damager.enabled = false;
                break;
        }
    }
}
