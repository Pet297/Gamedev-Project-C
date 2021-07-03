using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IwtSandScript : AbstractAfectable
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
        if (CurrentState == 0) CurrentState = 1;
    }
    public override void OnFreeze() { }
    public override void OnPoison() { }
    public override void OnMagic() { }
    public override void OnAntidote() { }
    public override void OnExplode()
    {
        if (CurrentState == 1) GameObject.Destroy(gameObject);
    }
}
