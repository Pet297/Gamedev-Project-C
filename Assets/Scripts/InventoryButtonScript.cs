using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryButtonScript : MonoBehaviour
{
    public GameObject Player;
    public string ItemType;

    RectTransform rt;
    PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        pc = Player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            //Vector2 lp;
            Vector2 mousePos = Input.mousePosition;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, mousePos, Camera.main, out lp);

            if (RectTransformUtility.RectangleContainsScreenPoint(rt, mousePos)) 
        }*/
    }

    void OnMouseDown()
    {
        pc.SelectInventoryItem(ItemType);
    }
}
