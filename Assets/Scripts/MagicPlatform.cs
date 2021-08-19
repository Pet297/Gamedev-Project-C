using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPlatform : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector2 lastPosition;

    public Vector2 CeilingCheck;
    public LayerMask Ground;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + new Vector2(0,3) * Time.fixedDeltaTime);

        if (CheckGroundCollision(CeilingCheck)) gameObject.SetActive(false);

        lastPosition = rigid.position;
    }

    bool CheckGroundCollision(Vector2 checkPositon)
    {
        Vector2 checkPos = new Vector2
            (gameObject.transform.position.x + checkPositon.x,
            gameObject.transform.position.y + checkPositon.y);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPos, new Vector2(0.4f, 0.05f), Ground);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if ((Ground.value & (1 << colliders[i].gameObject.layer)) > 0) return true;
            }
        }
        return false;
    }
}
