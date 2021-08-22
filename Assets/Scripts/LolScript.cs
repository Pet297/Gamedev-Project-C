using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LolScript : MonoBehaviour
{
    public Sprite Throwing;
    public Sprite NotThrowing;

    public float ThrowTime;
    public float ThrowDuration;
    public float ThrowOffset;

    float timer = 0;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timer %= ThrowTime;
        if (timer > ThrowOffset && timer <= ThrowOffset + ThrowDuration) sr.sprite = Throwing;
        else sr.sprite = NotThrowing;

    }
}
