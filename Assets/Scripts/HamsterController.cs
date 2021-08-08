using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterController : MonoBehaviour
{
    public GameObject FloorCheck;
    public GameObject WallLCheck;
    public GameObject WallRCheck;
    public GameObject JumpLCheck;
    public GameObject JumpRCheck;
    public LayerMask Ground;

    private GameObject Player;
    private PlayerController pc;
    private SpriteRenderer renderer;
    private Animator animator;

    private Rigidbody2D rigidbody2D;
    BoxCollider2D FloorBox;
    BoxCollider2D WallLBox;
    BoxCollider2D WallRBox;
    BoxCollider2D JumpLBox;
    BoxCollider2D JumpRBox;

    bool touchesGround = false;

    // Start is called before the first frame update
    void Start()
    {
        FloorBox = FloorCheck.GetComponent<BoxCollider2D>();
        WallLBox = WallLCheck.GetComponent<BoxCollider2D>();
        WallRBox = WallRCheck.GetComponent<BoxCollider2D>();
        JumpLBox = JumpLCheck.GetComponent<BoxCollider2D>();
        JumpRBox = JumpRCheck.GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        Player = GameObject.Find("Player");
        pc = Player.GetComponent<PlayerController>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool grounded = TestForCollision(FloorBox);
        bool leftjump = TestForCollision(WallLBox);
        bool rightjump = TestForCollision(WallRBox);
        bool leftwall = TestForCollision(JumpLBox);
        bool rightwall = TestForCollision(JumpRBox);

        bool jump = PlayerAbove && PlayerClose && currentState == HamsterState.STAND;
        bool landed = !touchesGround && grounded;
        bool fell = false && touchesGround && !grounded;

        UpdateState(jump,landed,fell,grounded,false);

        float horizontalMove = 0f;

        if (Player != null && pc.Visible && pc.InSameRoom(gameObject.transform.position))
        {
            if (Player.transform.position.x > transform.position.x) horizontalMove = 0.15f;
            else horizontalMove = -0.15f;
        }

        rigidbody2D.MovePosition(transform.position + new Vector3(horizontalMove, speedY, 0));

        speedY -= 0.05f;
        if (speedY < -0.2f) speedY = -0.2f;
        if (grounded && currentState != HamsterState.JUMP) speedY = 0.02f;

        touchesGround = grounded;
        jump = false;
    }

    bool TestForCollision(BoxCollider2D collider)
    {
        Vector2 checkPos = new Vector2(collider.bounds.center.x, collider.bounds.center.y);
        Vector2 checkSize = new Vector2(collider.bounds.size.x, collider.bounds.size.y);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPos, checkSize, 0);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if ((Ground.value & (1 << colliders[i].gameObject.layer)) > 0) return true;
            }
        }
        return false;
    }

    bool PlayerAbove => Player != null && pc.Visible && pc.InSameRoom(gameObject.transform.position) && Player.transform.position.y - transform.position.y > 0.0f;
    bool PlayerClose => Player != null && pc.Visible && pc.InSameRoom(gameObject.transform.position) && Mathf.Abs(Player.transform.position.x - transform.position.x) < 4f;
    bool NextToPlayer => Player != null && pc.Visible && pc.InSameRoom(gameObject.transform.position) && Mathf.Abs(Player.transform.position.x - transform.position.x) < 0.7f;

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
                    gameObject.transform.position = new Vector3(
            gameObject.transform.position.x,
            Mathf.Ceil(gameObject.transform.position.y) + 0.5f,
            gameObject.transform.position.z);
                    break;
                case HamsterState.JUMP:
                    Jump();
                    break;
                case HamsterState.ATTACK:
                    //if (renderer.flipX) attackL.EnableOnce();
                    //else attackR.EnableOnce();
                    break;
            }

            // SET ANIMATOR
            switch (currentState)
            {
                case HamsterState.STAND:
                    animator.SetBool("Moving", false || false);
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
                if (jump) EnterState(HamsterState.JUMP);
                else if (fell) EnterState(HamsterState.FALL);
                else if (attack) EnterState(HamsterState.ATTACK);
                break;
            case HamsterState.JUMP:
                if (landed) EnterState(HamsterState.STAND);
                break;
            case HamsterState.FALL:
                if (landed) EnterState(HamsterState.STAND);
                break;
            case HamsterState.ATTACK:
                if (stateTimer > 0.4f) EnterState(HamsterState.STAND);
                break;
        }
    }

    float speedY;
    void Jump()
    {
        speedY = 0.6f;
    }
}
