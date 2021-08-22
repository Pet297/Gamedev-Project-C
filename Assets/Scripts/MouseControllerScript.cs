using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControllerScript : MonoBehaviour
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
    void Update()
    {
        if (Player != null && pc != null && pc.Visible && pc.InSameRoom(gameObject.transform.position))
        {
            if (Player.transform.position.x > transform.position.x) renderer.flipX = true;
            else renderer.flipX = false;

            AttackTime += Time.deltaTime * pes.MoveSpeed;
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
        // LOG SPAWN
        GameObject magic = pool.GetPooledObject();
        if (magic != null)
        {
            magic.transform.position = gameObject.transform.position;
            magic.GetComponent<MouseProjectileScript>().ResetTarget(Player);
            magic.SetActive(true);
        }


        animator.SetBool("Attack", false);
        AttackTime = 0;
        Attacked = false;
    }
}
