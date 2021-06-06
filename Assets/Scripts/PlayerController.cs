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

    private Animator animator;
    //custom gravity impl
    float spdY = -0.2f;

    private Rigidbody2D rigidbody2D;
    bool touchesGround = false;
    PlayerState currentState = PlayerState.STAND;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    bool jump = false;
    void Update()
    {
        jump = jump || Input.GetButtonDown("Jump");
    }

    void FixedUpdate()
    {
        bool touchesGroundNow = CheckGroundCollision();

        float horizontalMove = Input.GetAxisRaw("Horizontal") * MaxSpeed;
        float verticalAxis = Input.GetAxisRaw("Vertical");

        bool landed = !touchesGround && touchesGroundNow;
        bool fell = touchesGround && !touchesGroundNow;
        bool crouching = verticalAxis < -0.5f;

        //if (jump && touchesGround) gameObject.transform.Translate(0, 0.001f, 0);

        UpdateState(jump, landed, fell, crouching);

        /*if (!touchesGround)*/ rigidbody2D.MovePosition(transform.position + new Vector3(horizontalMove, /*rigidbody2D.velocity.y + 0.01f*/ spdY, 0));
        //else rigidbody2D.MovePosition(transform.position + new Vector3(horizontalMove, 0.01f, 0));

        spdY -= Gravity;
        if (spdY < -0.2f) spdY = -0.2f;

        touchesGround = touchesGroundNow;
        jump = false;
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

            }
        }
    }
    void UpdateState(bool jump, bool landed, bool fell, bool crouching)
    {
        switch (currentState)
        {
            case PlayerState.STAND:
                if (jump) EnterState(PlayerState.JUMP);
                else if (crouching) EnterState(PlayerState.CROUCH);
                else if (fell) EnterState(PlayerState.FALL);
                break;
            case PlayerState.JUMP:
                if (landed) EnterState(PlayerState.STAND);
                break;
            case PlayerState.FALL:
                if (landed) EnterState(PlayerState.STAND);
                break;
            case PlayerState.CROUCH:
                if (!crouching) EnterState(PlayerState.STAND);
                break;
        }
    }
}
