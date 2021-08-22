using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverControlerScript : MonoBehaviour
{
    public float ThrowFrequency = 4.0f;
    public float ThrowDelay = 1.0f;

    private GameObject Player;
    private PlayerController pc;

    private float ThrowTime = 0;
    private bool WasThrown = false;
    private Animator animator;
    private ObjectPoolScript pool;
    private SpriteRenderer renderer;

    private PotionEffectsScript pes;

    // Start is called before the first frame update
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        pool = gameObject.GetComponent<ObjectPoolScript>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        Player = GameObject.FindWithTag("Player");
        pc = Player?.GetComponent<PlayerController>();
        pes = GetComponent<PotionEffectsScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Player != null && pc != null && pc.Visible && pc.InSameRoom(gameObject.transform.position))
        {
            if (Player.transform.position.x > transform.position.x) renderer.flipX = true;
            else renderer.flipX = false;

            ThrowTime += Time.deltaTime * pes.MoveSpeed;
            if (ThrowTime > ThrowFrequency)
            {
                animator.SetBool("ThrowLog", true);
            }
            if (ThrowTime > (ThrowFrequency + ThrowDelay) && !WasThrown)
            {
                WasThrown = true;
                Throw();
            }
        }
    }

    public void Throw()
    {
        bool flipX = renderer.flipX;
        // LOG SPAWN
        GameObject log = pool.GetPooledObject();
        if (log != null)
        {
            log.transform.position = gameObject.transform.position;
            log.GetComponent<LogMovementScript>().Speed = new Vector3( (flipX ? 10f : -10f), -10f, 0);
            log.SetActive(true);
        }


        animator.SetBool("ThrowLog", false);
        ThrowTime = 0;
        WasThrown = false;
    }
}
