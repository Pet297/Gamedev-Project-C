using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAfectable : MonoBehaviour
{
    public abstract void OnHeat();
    public abstract void OnFreeze();
    public abstract void OnPoison();
    public abstract void OnMagic();
    public abstract void OnAntidote();
    public abstract void OnExplode();
}
