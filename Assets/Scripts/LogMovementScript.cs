using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMovementScript : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector3 Speed = new Vector3(0f,0f,0f);

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((lastPosition - transform.position).magnitude < 0.01f)
        {
            gameObject.SetActive(false);
            //"explode"
        }

        rb.MovePosition(transform.position + Time.deltaTime * Speed);
        lastPosition = transform.position;
    }
}
