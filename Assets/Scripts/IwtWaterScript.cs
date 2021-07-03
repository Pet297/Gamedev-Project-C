using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IwtWaterScript : AbstractAfectable
{
    public int CurrentState = 0;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("State", CurrentState);
    }

    public override void OnHeat()
    {
        if (CurrentState == 1)
        {
            //SPAWN EXPLOSION
        }
        else if (CurrentState == 2) CurrentState = 0;
    }
    public override void OnFreeze()
    {
        if (CurrentState == 0) CurrentState = 2;
        else if (CurrentState == 3)
        {
            //SPAWN PLATFORM
        }
    }
    public override void OnPoison()
    {
        if (CurrentState == 0) CurrentState = 1;
        else if (CurrentState == 3) CurrentState = 0;
    }
    public override void OnMagic()
    {
        if (CurrentState == 0) CurrentState = 3;
    }
    public override void OnAntidote()
    {
        if (CurrentState == 1) CurrentState = 0;
    }
    public override void OnExplode()
    {
        Debug.Log("explode");
        if (CurrentState == 2) GameObject.Destroy(gameObject);
    }
}
