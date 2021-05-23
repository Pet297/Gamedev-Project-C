using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    List<InventoryEntry> entries = new List<InventoryEntry>();

    public List<GameObject> GUIElements = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(string itemType)
    {
        FindEntry(itemType).Increase();
        UpdateGUIElements(itemType);
    }
    public bool CanUse(string itemType) => FindEntry(itemType).CanUse();
    public void RemoveItem(string itemType)
    {
        FindEntry(itemType).Decrease();
        UpdateGUIElements(itemType);
    }
    public int GetCount(string itemType) => FindEntry(itemType).GetCount();

    private InventoryEntry FindEntry(string s)
    {
        foreach (InventoryEntry de in entries)
        {
            if (de.key == s) return de;
        }

        InventoryEntry de2 = new InventoryEntry(s, 0);
        entries.Add(de2);
        return de2;
    }

    private void UpdateGUIElements(string itemType)
    {
        foreach(GameObject go in GUIElements)
        {
            GUIElementScript g = go.GetComponent<GUIElementScript>();
            if (go != null)
            {
                if (g.ItemType == itemType) g.UpdateCount(FindEntry(itemType).GetCount());
            }
        }
    }
}
