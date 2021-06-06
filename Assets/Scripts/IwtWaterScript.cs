using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IwtWaterScript : MonoBehaviour, IAfectable
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

    public void OnHeat()
    {
        if (CurrentState == 1)
        {
            //SPAWN EXPLOSION
        }
        else if (CurrentState == 2) CurrentState = 0;
    }
    public void OnFreeze()
    {
        if (CurrentState == 0) CurrentState = 2;
        else if (CurrentState == 3)
        {
            //SPAWN PLATFORM
        }
    }
    public void OnPoison()
    {
        if (CurrentState == 0) CurrentState = 1;
        else if (CurrentState == 3) CurrentState = 0;
    }
    public void OnMagic()
    {
        if (CurrentState == 0) CurrentState = 3;
    }
    public void OnAntidote()
    {
        if (CurrentState == 1) CurrentState = 0;
    }
    public void OnExplode()
    {
        if (CurrentState == 2) GameObject.Destroy(gameObject);
    }
}
