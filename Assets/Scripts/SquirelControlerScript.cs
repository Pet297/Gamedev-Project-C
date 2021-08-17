using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirelControlerScript : MonoBehaviour
{
    public float AttackFrequency = 4.0f;
    public float AttackDelay = 1.0f;
    public GameObject ShootOrigin;

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

            AttackTime += Time.deltaTime;
            if (AttackTime > AttackFrequency)
            {
                animator.SetBool("Attack", true);
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
        GameObject magic = pool.GetPooledObject();

        if (magic != null && Player != null)
        {
            Vector2 toTarget = new Vector2(Player.transform.position.x - ShootOrigin.transform.position.x, Player.transform.position.y - ShootOrigin.transform.position.y);
            Vector2 toTarget2 = new Vector2(toTarget.x, toTarget.y + toTarget.magnitude / 6f);
            toTarget2 = 12 * toTarget2.normalized;

            magic.transform.position = ShootOrigin.transform.position;
            magic.GetComponent<RatProjectileScript>().ResetTarget(Player, toTarget2.x, toTarget2.y, -2);
            magic.SetActive(true);
        }


        animator.SetBool("Attack", false);
        AttackTime = 0;
        Attacked = false;
    }
}
