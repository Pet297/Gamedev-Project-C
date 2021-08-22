using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterController : MonoBehaviour
{
    public Vector2 GroundCheck;
    public Vector2 WallLeftCheck;
    public Vector2 WallRightCheck;

    public GameObject Damager;
    private TemporalEnablerScript attackD;

    public float TerminalVelocity = -0.2f;
    public float Gravity = 0.1f;
    public float MaxSpeed = 0.2f;
    public float JumpPower = 0.8f;
    public bool IsHamster = true;

    public LayerMask Ground;

    private GameObject Player;
    private PlayerController pc;
    private SpriteRenderer renderer;
    private Animator animator;

    private Rigidbody2D rigidbody2D;
    private PotionEffectsScript pes;

    bool touchesGround = false;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        pes = GetComponent<PotionEffectsScript>();
        attackD = Damager.GetComponent<TemporalEnablerScript>();

        Player = GameObject.FindWithTag("Player");
        pc = Player?.GetComponent<PlayerController>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (pc.Visible) stateTimer += Time.deltaTime * pes.MoveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool grounded = CheckGroundCollision(GroundCheck);
        bool wallLeft = CheckGroundCollision(WallLeftCheck);
        bool wallRight = CheckGroundCollision(WallRightCheck);

        bool jump = PlayerAbove && PlayerClose && currentState == HamsterState.STAND;
        bool landed = !touchesGround && grounded;
        bool fell = touchesGround && !grounded;

        jump = jump || (wallLeft && !renderer.flipX && !PlayerClose);
        jump = jump || (wallRight && renderer.flipX && !PlayerClose);

        UpdateState(jump,landed,fell,grounded,false);

        float horizontalMove = 0f;

        if (Player != null && pc != null && pc.Visible && pc.InSameRoom(gameObject.transform.position) && !NextToPlayer)
        {
            if (Player.transform.position.x > transform.position.x)
            {
                if (currentState != HamsterState.ATTACK) horizontalMove = MaxSpeed;
                else horizontalMove = MaxSpeed * 1.6f;
                renderer.flipX = true;
            }
            else
            {
                if (currentState != HamsterState.ATTACK) horizontalMove = -MaxSpeed;
                else horizontalMove = -MaxSpeed * 1.6f;
                renderer.flipX = false;
            }
        }

        rigidbody2D.MovePosition(transform.position + new Vector3(horizontalMove * pes.MoveSpeed, speedY, 0));

        speedY -= Gravity;
        if (speedY < TerminalVelocity) speedY = TerminalVelocity;

        touchesGround = grounded;
        jump = false;
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

    public enum HamsterState
    {
        STAND, JUMP, FALL, ATTACK
    }
    public HamsterState currentState = HamsterState.STAND;
    float stateTimer = 0f;
    void EnterState(HamsterState newState)
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
                case HamsterState.STAND:
                    break;
                case HamsterState.JUMP:
                    
                    break;
                case HamsterState.ATTACK:
                    break;
            }

            // SET ANIMATOR
            switch (currentState)
            {
                case HamsterState.STAND:
                    animator.SetBool("Moving", false);
                    animator.SetBool("Jumping", false);
                    animator.SetBool("Atacking", false);
                    break;
                case HamsterState.JUMP:
                    animator.SetBool("Moving", false);
                    animator.SetBool("Jumping", true);
                    animator.SetBool("Atacking", false);
                    break;
                case HamsterState.FALL:
                    animator.SetBool("Moving", false);
                    animator.SetBool("Jumping", true);
                    animator.SetBool("Atacking", false);
                    break;
                case HamsterState.ATTACK:
                    animator.SetBool("Moving", false);
                    animator.SetBool("Jumping", false);
                    animator.SetBool("Atacking", true);
                    break;
            }
        }
    }

    void UpdateState(bool jump, bool landed, bool fell, bool grounded, bool attack)
    {
        switch (currentState)
        {
            case HamsterState.STAND:
                if (jump) { Jump(); EnterState(HamsterState.JUMP); }
                else if (fell) EnterState(HamsterState.FALL);
                else if (attack) EnterState(HamsterState.ATTACK);
                else if (stateTimer > 0.6f && !IsHamster && PlayerClose) { EnterState(HamsterState.ATTACK); }
                else if (stateTimer > 1.5f) { SmallJump(); EnterState(HamsterState.JUMP); if (IsHamster) attackD.EnableOnce(); }
                break;
            case HamsterState.JUMP:
                if (grounded) EnterState(HamsterState.STAND);
                break;
            case HamsterState.FALL:
                if (grounded) EnterState(HamsterState.STAND);
                break;
            case HamsterState.ATTACK:
                if (stateTimer > 0.2f) attackD.EnableOnce();
                if (stateTimer > 0.3f) EnterState(HamsterState.STAND);
                break;
        }
    }

    float speedY;
    void Jump()
    {
        speedY = JumpPower;
    }
    void SmallJump()
    {
        speedY = JumpPower/2f;
    }
}
