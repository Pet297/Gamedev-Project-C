using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float JumpForce = 3f;
    public float Gravity = 0.05f;
    public float MaxSpeed = 0.05f;
    public Vector2 GroundCheck;
    public LayerMask Ground;

    //custom gravity impl
    float spdY = -0.2f;

    public GameObject AttackL;
    public GameObject AttackR;

    private Rigidbody2D rigidbody2D;
    private SpriteRenderer renderer;
    private Animator animator;

    private TemporalEnablerScript attackR;
    private TemporalEnablerScript attackL;
    bool touchesGround = false;
    PlayerState currentState = PlayerState.STAND;

    private void Awake()
    {
        attackR = AttackR.GetComponent<TemporalEnablerScript>();
        attackL = AttackL.GetComponent<TemporalEnablerScript>();

        renderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    bool jump = false;
    bool attack = false;
    float stateTimer = 0f;
    void Update()
    {
        jump = jump || Input.GetButtonDown("Jump");
        attack = attack || Input.GetButtonDown("Fire1");

        stateTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        bool touchesGroundNow = CheckGroundCollision();

        float horizontalMove = Input.GetAxisRaw("Horizontal") * MaxSpeed;
        float verticalAxis = Input.GetAxisRaw("Vertical");

        bool landed = !touchesGround && touchesGroundNow;
        bool fell = touchesGround && !touchesGroundNow;
        bool crouching = verticalAxis < -0.5f;


        UpdateState(jump, landed, fell, crouching, attack);

        rigidbody2D.MovePosition(transform.position + new Vector3(horizontalMove, spdY, 0));

        spdY -= Gravity;
        if (spdY < -0.2f) spdY = -0.2f;

        touchesGround = touchesGroundNow;
        jump = false;
        attack = false;
    }

    bool CheckGroundCollision()
    {
        Vector2 checkPos = new Vector2
            (gameObject.transform.position.x + GroundCheck.x,
            gameObject.transform.position.y + GroundCheck.y);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPos, new Vector2(0.4f,0.05f), Ground);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    /*
     * 
     *        FALL  CLIMB     JUMP ---- JUMP-ATK
     *            \   |     /       \     |
     *             \  |    /         \    |
     *              \ |   /           \   |
     *    MELEE A --- STAND --- AIM    LAND ---- [STAND]          
     *                |    \    |
     *                |     \   |
     *                |      \  |
     *   CROUCH A --- CROUCH   SHOOT
     * 
     */

    public enum PlayerState { STAND, CROUCH, CROUCH_ATK, CROUCH_MOVE, ATK, ITEM, BOW, BOW_SHOOT, JUMP, LAND, JUMP_ATK, CLIMB, FALL }

    void EnterState(PlayerState newState)
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
                case PlayerState.JUMP:
                    spdY = JumpForce;
                    break;
                case PlayerState.ATK:
                    if (renderer.flipX) attackL.EnableOnce();
                    else attackR.EnableOnce();
                    break;
                case PlayerState.CROUCH_ATK:
                    if (renderer.flipX) attackL.EnableOnce();
                    else attackR.EnableOnce();
                    break;
                case PlayerState.JUMP_ATK:
                    if (renderer.flipX) attackL.EnableOnce();
                    else attackR.EnableOnce();
                    break;
            }

            // SET ANIMATOR
            switch (currentState)
            {
                case PlayerState.STAND:
                    animator.SetBool("Move", false);
                    animator.SetBool("Crouch", false);
                    animator.SetBool("Inventory", false);
                    animator.SetBool("Bow", false);
                    animator.SetInteger("BowAngle", 0);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Jump", false);
                    animator.SetBool("Jump2", false);
                    animator.SetBool("Land", false);
                    animator.SetBool("Climb", false);
                    break;
                case PlayerState.JUMP:
                    animator.SetBool("Move", false);
                    animator.SetBool("Crouch", false);
                    animator.SetBool("Inventory", false);
                    animator.SetBool("Bow", false);
                    animator.SetInteger("BowAngle", 0);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Jump", true);
                    animator.SetBool("Jump2", false);
                    animator.SetBool("Land", false);
                    animator.SetBool("Climb", false);
                    break;
                case PlayerState.FALL:
                    animator.SetBool("Move", false);
                    animator.SetBool("Crouch", false);
                    animator.SetBool("Inventory", false);
                    animator.SetBool("Bow", false);
                    animator.SetInteger("BowAngle", 0);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Jump", true);
                    animator.SetBool("Jump2", false);
                    animator.SetBool("Land", false);
                    animator.SetBool("Climb", false);
                    break;
                case PlayerState.LAND:
                    animator.SetBool("Move", false);
                    animator.SetBool("Crouch", false);
                    animator.SetBool("Inventory", false);
                    animator.SetBool("Bow", false);
                    animator.SetInteger("BowAngle", 0);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Jump", false);
                    animator.SetBool("Jump2", false);
                    animator.SetBool("Land", true);
                    animator.SetBool("Climb", false);
                    break;
                case PlayerState.CROUCH:
                    animator.SetBool("Move", false);
                    animator.SetBool("Crouch", true);
                    animator.SetBool("Inventory", false);
                    animator.SetBool("Bow", false);
                    animator.SetInteger("BowAngle", 0);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Jump", false);
                    animator.SetBool("Jump2", false);
                    animator.SetBool("Land", false);
                    animator.SetBool("Climb", false);
                    break;
                case PlayerState.CROUCH_ATK:
                    animator.SetBool("Move", false);
                    animator.SetBool("Crouch", true);
                    animator.SetBool("Inventory", false);
                    animator.SetBool("Bow", false);
                    animator.SetInteger("BowAngle", 0);
                    animator.SetBool("Attack", true);
                    animator.SetBool("Jump", false);
                    animator.SetBool("Jump2", false);
                    animator.SetBool("Land", false);
                    animator.SetBool("Climb", false);
                    break;
                case PlayerState.CROUCH_MOVE:
                    animator.SetBool("Move", true);
                    animator.SetBool("Crouch", true);
                    animator.SetBool("Inventory", false);
                    animator.SetBool("Bow", false);
                    animator.SetInteger("BowAngle", 0);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Jump", false);
                    animator.SetBool("Jump2", false);
                    animator.SetBool("Land", false);
                    animator.SetBool("Climb", false);
                    break;
                case PlayerState.CLIMB:
                    animator.SetBool("Move", false);
                    animator.SetBool("Crouch", false);
                    animator.SetBool("Inventory", false);
                    animator.SetBool("Bow", false);
                    animator.SetInteger("BowAngle", 0);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Jump", false);
                    animator.SetBool("Jump2", false);
                    animator.SetBool("Land", false);
                    animator.SetBool("Climb", true);
                    break;
                case PlayerState.ATK:
                    animator.SetBool("Move", false);
                    animator.SetBool("Crouch", false);
                    animator.SetBool("Inventory", false);
                    animator.SetBool("Bow", false);
                    animator.SetInteger("BowAngle", 0);
                    animator.SetBool("Attack", true);
                    animator.SetBool("Jump", false);
                    animator.SetBool("Jump2", false);
                    animator.SetBool("Land", false);
                    animator.SetBool("Climb", false);
                    break;
                case PlayerState.JUMP_ATK:
                    animator.SetBool("Move", false);
                    animator.SetBool("Crouch", false);
                    animator.SetBool("Inventory", false);
                    animator.SetBool("Bow", false);
                    animator.SetInteger("BowAngle", 0);
                    animator.SetBool("Attack", true);
                    animator.SetBool("Jump", true);
                    animator.SetBool("Jump2", false);
                    animator.SetBool("Land", false);
                    animator.SetBool("Climb", false);
                    break;

            }
        }
    }
    void UpdateState(bool jump, bool landed, bool fell, bool crouching, bool attack)
    {
        switch (currentState)
        {
            case PlayerState.STAND:
                if (jump) EnterState(PlayerState.JUMP);
                else if (crouching) EnterState(PlayerState.CROUCH);
                else if (fell) EnterState(PlayerState.FALL);
                else if (attack) EnterState(PlayerState.ATK);
                break;
            case PlayerState.JUMP:
                if (landed) EnterState(PlayerState.STAND);
                break;
            case PlayerState.FALL:
                if (landed) EnterState(PlayerState.STAND);
                else if (attack) EnterState(PlayerState.JUMP_ATK);
                break;
            case PlayerState.CROUCH:
                if (!crouching) EnterState(PlayerState.STAND);
                else if (attack) EnterState(PlayerState.CROUCH_ATK);
                break;
            case PlayerState.ATK:
                if (stateTimer > 0.4f) EnterState(PlayerState.STAND);
                break;
            case PlayerState.JUMP_ATK:
                if (landed) EnterState(PlayerState.STAND);
                break;
            case PlayerState.CROUCH_ATK:
                if (stateTimer > 0.4f) EnterState(PlayerState.CROUCH);
                break;
        }
    }
}
