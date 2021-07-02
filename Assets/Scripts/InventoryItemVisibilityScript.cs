using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemVisibilityScript : MonoBehaviour
{
    public GameObject Player;
    public string ItemType;

    private PlayerInventoryScript inventory;
    public Image image;


    // Start is called before the first frame update
    void Start()
    {
        inventory = Player.GetComponent<PlayerInventoryScript>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.enabled = inventory.GetCount(ItemType) > 0;
    }
}
