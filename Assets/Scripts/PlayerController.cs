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

    private Rigidbody2D rigidbody2D;
    bool touchesGround = false;
    PlayerState currentState = PlayerState.STAND;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
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
        bool landed = !touchesGround && touchesGroundNow;
        bool fell = touchesGround && !touchesGroundNow;

        //if (jump && touchesGround) gameObject.transform.Translate(0, 0.001f, 0);

        UpdateState(jump, landed, fell);

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

    public enum PlayerState { STAND, JUMP, LAND, FALL, JUMP_ATK, CLIMB, ATK, CROUCH, CROUCH_ATK, AIM, SHOOT }

    void EnterState(PlayerState newState)
    {
        if (newState != currentState)
        {
            // leave state
            switch (currentState)
            {
            }

            currentState = newState;

            // enter state
            switch (currentState)
            {
                case PlayerState.JUMP:
                    //rigidbody2D.AddForce(new Vector2(0f, JumpForce));
                    spdY = JumpForce;
                    break;
            }
        }
    }
    void UpdateState(bool jump, bool landed, bool fell)
    {
        switch (currentState)
        {
            case PlayerState.STAND:
                if (jump) EnterState(PlayerState.JUMP);
                else if (fell) EnterState(PlayerState.FALL);
                break;
            case PlayerState.JUMP:
                if (landed) EnterState(PlayerState.STAND);
                break;
            case PlayerState.FALL:
                if (landed) EnterState(PlayerState.STAND);
                break;
        }
    }
}
