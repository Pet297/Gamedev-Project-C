using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSquirrelControler : MonoBehaviour
{
    public Vector2 UpCheck;
    public Vector2 LeftCheck;
    public Vector2 RightCheck;
    public Vector2 DownCheck;

    public float TerminalVelocity = -0.2001f;
    public float Gravity = 0.1f;
    public float MaxSpeed = 0.2f;
    public float FlapStrength = 0.3f;
    public float FlapDelay = 0.2f;

    public LayerMask Ground;

    private GameObject Player;
    private PlayerController pc;
    private SpriteRenderer renderer;
    private Animator animator;

    private Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        Player = GameObject.Find("Player");
        pc = Player.GetComponent<PlayerController>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        stateTimer += Time.deltaTime;
        flapTimer += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool down = CheckGroundCollision(DownCheck);
        bool left = CheckGroundCollision(LeftCheck);
        bool right = CheckGroundCollision(RightCheck);
        bool up = CheckGroundCollision(UpCheck);

        bool chasing = PlayerCloseX && PlayerCloseY;

        UpdateState(chasing, up, down, left, right);

        if (Player != null && pc.Visible && pc.InSameRoom1(gameObject.transform.position) && !NextToPlayer)
        {
            if (currentlyFlying)
            {
                Vector2 toTarget = new Vector2(Player.transform.position.x - gameObject.transform.position.x, Player.transform.position.y - gameObject.transform.position.y);
                speedX = toTarget.normalized.x * MaxSpeed;
                dirY = toTarget.normalized.y * MaxSpeed * 0.3f;

                if (flapTimer > FlapDelay && dirY >= 0) Flap();
                else if (dirY < 0) flapTimer = 0;

                renderer.flipX = speedX > 0;
            }
        }

        rigidbody2D.MovePosition(transform.position + new Vector3(speedX, speedY, 0));

        speedY -= Gravity;
        if (speedY < TerminalVelocity) speedY = TerminalVelocity;
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

    bool PlayerAbove => Player != null && pc.Visible && pc.InSameRoom1(gameObject.transform.position) && Player.transform.position.y - transform.position.y > 2.0f;
    bool PlayerCloseX => Player != null && pc.Visible && pc.InSameRoom1(gameObject.transform.position) && Mathf.Abs(Player.transform.position.x - transform.position.x) < 7f;
    bool PlayerCloseY => Player != null && pc.Visible && pc.InSameRoom1(gameObject.transform.position) && Mathf.Abs(Player.transform.position.x - transform.position.x) < 7f;
    bool NextToPlayer => Player != null && pc.Visible && pc.InSameRoom1(gameObject.transform.position) && Mathf.Abs(Player.transform.position.x - transform.position.x) < 0.7f;

    public enum FSState
    {
        RESTING, CHASING
    }
    public FSState currentState = FSState.RESTING;
    float stateTimer = 0f;
    void SetAnimatorState(FSState chaseState, bool up, bool down, bool left, bool right)
    {
        if (chaseState == FSState.CHASING)
        {
            animator.SetBool("Top", false);
            animator.SetBool("Bottom", false);
            animator.SetBool("Fly", true);
            animator.SetBool("Side", false);

            currentlyFlying = true;
        }
        else
        {
            if (!up && !down && !left && !right)
            {
                animator.SetBool("Top", false);
                animator.SetBool("Bottom", false);
                animator.SetBool("Fly", true);
                animator.SetBool("Side", false);

                currentlyFlying = true;
            }
            else if (left)
            {
                animator.SetBool("Top", false);
                animator.SetBool("Bottom", false);
                animator.SetBool("Fly", false);
                animator.SetBool("Side", true);
                renderer.flipX = false;

                currentlyFlying = false;
                speedX = -0.2f;
                speedY = 0;
            }
            else if (right)
            {
                animator.SetBool("Top", false);
                animator.SetBool("Bottom", false);
                animator.SetBool("Fly", false);
                animator.SetBool("Side", true);
                renderer.flipX = true;

                currentlyFlying = false;
                speedX = 0.2f;
                speedY = 0;
            }
            else if (up)
            {
                animator.SetBool("Top", true);
                animator.SetBool("Bottom", false);
                animator.SetBool("Fly", false);
                animator.SetBool("Side", false);

                currentlyFlying = false;
                speedX = 0;
                speedY = 0.2f;
            }
            else if (down)
            {
                animator.SetBool("Top", false);
                animator.SetBool("Bottom", true);
                animator.SetBool("Fly", false);
                animator.SetBool("Side", false);

                currentlyFlying = false;
                speedX = 0;
                speedY = -0.2f;
            }
        }
    }

    void UpdateState(bool chase, bool up, bool down, bool left, bool right)
    {
        currentState = chase ? FSState.CHASING : FSState.RESTING;

        //if (currentState == FSState.RESTING) flapTimer = 0f;

        SetAnimatorState(currentState, up, down, left, right);
    }

    float flapTimer = 0f;
    float speedY;
    float dirY = 1;
    float speedX;
    bool currentlyFlying = false;

    void Flap()
    {
        speedY = FlapStrength;
        flapTimer -= FlapDelay;
    }
}
