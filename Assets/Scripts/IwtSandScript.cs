using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IwtSandScript : MonoBehaviour, IAfectable
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
        if (CurrentState == 0) CurrentState = 1;
    }
    public void OnFreeze() { }
    public void OnPoison() { }
    public void OnMagic() { }
    public void OnAntidote() { }
    public void OnExplode()
    {
        if (CurrentState == 1) GameObject.Destroy(gameObject);
    }
}
