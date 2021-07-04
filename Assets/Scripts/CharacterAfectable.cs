using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAfectable : AbstractAfectable
{
    PotionEffectsScript pes;

    // Start is called before the first frame update
    void Start()
    {
        pes = gameObject.GetComponent<PotionEffectsScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnHeat() => pes.ApplyBurning(2);
    public override void OnFreeze() => pes.ApplyFrozen(3);
    public override void OnPoison() => pes.ApplyPoison(10);
    public override void OnMagic() { }
    public override void OnAntidote() => pes.CancelPoison();
    public override void OnExplode() { }
}
