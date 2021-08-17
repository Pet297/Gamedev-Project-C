using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutriaControllerScript : MonoBehaviour
{
    public Vector2 GroundCheck;
    public Vector2 WallLeftCheck;
    public Vector2 WallRightCheck;

    public GameObject Damager;
    private TemporalEnablerScript attackD;

    public float TerminalVelocity = -0.2001f;
    public float Gravity = 0.1f;
    public float MaxSpeed = 0.2f;

    public LayerMask Ground;

    private GameObject Player;
    private PlayerController pc;
    private SpriteRenderer renderer;
    private Animator animator;

    private Rigidbody2D rigidbody2D;

    bool touchesGround = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        attackD = Damager.GetComponent<TemporalEnablerScript>();

        Player = GameObject.FindWithTag("Player");
        pc = Player?.GetComponent<PlayerController>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        stateTimer += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool grounded = CheckGroundCollision(GroundCheck);
        bool wallLeft = CheckGroundCollision(WallLeftCheck);
        bool wallRight = CheckGroundCollision(WallRightCheck);

        bool landed = !touchesGround && grounded;
        bool fell = touchesGround && !grounded;

        UpdateState(landed, fell, grounded, false);

        float horizontalMove = 0f;

        if (Player != null && pc != null && pc.Visible && pc.InSameRoom1(gameObject.transform.position) && !NextToPlayer)
        {
            if (Player.transform.position.x > transform.position.x)
            {
                if (currentState != NutriaState.ATTACK) horizontalMove = MaxSpeed;
                else horizontalMove = MaxSpeed * (2.4f * (0.5f - stateTimer) / 0.5f);
                renderer.flipX = true;
            }
            else
            {
                if (currentState != NutriaState.ATTACK) horizontalMove = -MaxSpeed;
                else horizontalMove = -MaxSpeed * (2.4f * (0.5f - stateTimer) / 0.5f);
                renderer.flipX = false;
            }
        }

        rigidbody2D.MovePosition(transform.position + new Vector3(horizontalMove, speedY, 0));

        speedY -= Gravity;
        if (speedY < TerminalVelocity) speedY = TerminalVelocity;

        if (grounded && stateTimer > 0.5f) speedY += 0.101f;

        touchesGround = grounded;
    }

    bool CheckGroundCollision(Vector2 checkPos)
    {
        Vector2 checkPos2 = new Vector2
            (gameObject.transform.position.x + checkPos.x,
            gameObject.transform.position.y + checkPos.y);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPos2, new Vector2(0.4f, 0.05f), Ground);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if ((Ground.value & (1 << colliders[i].gameObject.layer)) > 0) return true;
            }
        }
        return false;
    }

    bool PlayerAbove => Player != null && pc != null && pc.Visible && pc.InSameRoom1(gameObject.transform.position) && Player.transform.position.y - transform.position.y > 2.0f;
    bool PlayerClose => Player != null && pc != null && pc.Visible && pc.InSameRoom1(gameObject.transform.position) && Mathf.Abs(Player.transform.position.x - transform.position.x) < 4f;
    bool NextToPlayer => Player != null && pc != null && pc.Visible && pc.InSameRoom1(gameObject.transform.position) && Mathf.Abs(Player.transform.position.x - transform.position.x) < 0.7f;

    public enum NutriaState
    {
        STAND, ATTACK
    }
    public NutriaState currentState = NutriaState.STAND;
    float stateTimer = 0f;
    void EnterState(NutriaState newState)
    {
        if (newState != currentState)
        {
            stateTimer = 0f;

            // LEAVE OLD
            switch (currentState)
            {
            }

            currentState = newState;

            // ENTER NEW
            switch (currentState)
            {
                case NutriaState.STAND:
                    break;
                case NutriaState.ATTACK:
                    break;
            }

            // SET ANIMATOR
            switch (currentState)
            {
                case NutriaState.STAND:
                    animator.SetBool("Moving", true);
                    animator.SetBool("Attacking", false);
                    break;
                case NutriaState.ATTACK:
                    animator.SetBool("Moving", false);
                    animator.SetBool("Attacking", true);
                    break;
            }
        }
    }

    void UpdateState(bool landed, bool fell, bool grounded, bool attack)
    {
        switch (currentState)
        {
            case NutriaState.STAND:
                if (attack) EnterState(NutriaState.ATTACK);
                else if (stateTimer > 2.0f) EnterState(NutriaState.ATTACK);
                break;
            case NutriaState.ATTACK:
                if (stateTimer > 0.2f) attackD.EnableOnce();
                if (stateTimer > 0.5f) EnterState(NutriaState.STAND);
                break;
        }
    }

    float speedY;
}
