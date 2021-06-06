using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAfectable
{
    void OnHeat();
    void OnFreeze();
    void OnPoison();
    void OnMagic();
    void OnAntidote();
    void OnExplode();
}
