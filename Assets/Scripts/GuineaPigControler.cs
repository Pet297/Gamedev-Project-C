using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuineaPigControler : MonoBehaviour
{
    public float AttackFrequency = 4.0f;
    public float AttackDelay = 1.0f;

    private GameObject Player;
    private PlayerController pc;

    private float AttackTime = 0;
    private bool Attacked = false;
    private Animator animator;
    private ObjectPoolScript pool;
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        pool = gameObject.GetComponent<ObjectPoolScript>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        Player = GameObject.FindWithTag("Player");
        pc = Player?.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null && pc != null && pc.Visible && pc.InSameRoom(gameObject.transform.position))
        {
            if (Player.transform.position.x > transform.position.x) renderer.flipX = true;
            else renderer.flipX = false;


            if (Player.transform.position.y - transform.position.y > -1f) AttackTime += Time.deltaTime;
            animator.SetBool("AimHigh", (Player.transform.position.y - transform.position.y > -1f) && Mathf.Abs(Player.transform.position.y - transform.position.y) > Mathf.Abs(Player.transform.position.x - transform.position.x));

            if (AttackTime > AttackFrequency)
            {
                animator.SetBool("Shoot", true);
            }
            if (AttackTime > (AttackFrequency + AttackDelay) && !Attacked)
            {
                Attacked = true;
                Throw();
            }
        }
    }

    public void Throw()
    {
        bool flipX = renderer.flipX;
        GameObject arrow = pool.GetPooledObject();

        if (arrow != null)
        {
            Vector2 toTarget = (new Vector2(Player.transform.position.x - gameObject.transform.position.x, Player.transform.position.y - gameObject.transform.position.y)).normalized;

            arrow.transform.position = gameObject.transform.position;
            arrow.GetComponent<GPProjectile>().ResetTarget(Player, toTarget.x * 15, toTarget.y * 15, 0.005f);
            arrow.SetActive(true);
        }


        animator.SetBool("Shoot", false);
        AttackTime = 0;
        Attacked = false;
    }
}
