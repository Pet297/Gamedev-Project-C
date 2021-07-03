using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodibleScript : AbstractAfectable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnHeat() { }
    public override void OnFreeze() { }
    public override void OnPoison() { }
    public override void OnMagic() { }
    public override void OnAntidote() { }
    public override void OnExplode()
    {
        GameObject.Destroy(gameObject);
    }
}
