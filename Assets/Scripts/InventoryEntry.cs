using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEntry
{
    public InventoryEntry(string s, int i)
    {
        key = s;
        value = i;
    }

    public string key;
    public int value;

    public void Increase() => value++;
    public void Decrease() => value--;
    public bool CanUse() => value > 0;
    public int GetCount() => value;
}
