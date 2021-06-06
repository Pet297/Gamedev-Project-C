using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodibleScript : MonoBehaviour, IAfectable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHeat() { }
    public void OnFreeze() { }
    public void OnPoison() { }
    public void OnMagic() { }
    public void OnAntidote() { }
    public void OnExplode()
    {
        GameObject.Destroy(gameObject);
    }
}
