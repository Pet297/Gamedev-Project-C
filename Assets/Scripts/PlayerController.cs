using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float JumpForce = 1.3f;
    public float Gravity = 0.05f;
    public float MaxSpeed = 0.05f;
    public Vector2 GroundCheck;
    public LayerMask Ground;
    public int RoomSizeX = 40;
    public int RoomSizeY = 20;

    Vector2 GroundCheck1;
    Vector2 GroundCheck2;


    public Vector2 LadderCheck;
    public LayerMask LadderLayer;

    //custom gravity impl
    float spdY = -0.2f;

    public GameObject AttackL;
    public GameObject AttackR;
    public GameObject AttackL2;
    public GameObject AttackR2;
    public GameObject InventoryObject;
    public GameObject Camera;
    public GameObject SelectedItemDisplay;

    private Rigidbody2D rigidbody2D;
    private SpriteRenderer renderer;
    private Animator animator;
    private DamagerScript damagerR;
    private DamagerScript damagerL;
    private DamagerScript damagerR2;
    private DamagerScript damagerL2;
    private PlayerInventoryScript inventory;
    private HealthPointsScript hps;
    private PotionEffectsScript pes;
    private Camera cam;

    private HeldItemImage hii;

    private TemporalEnablerScript attackR;
    private TemporalEnablerScript attackL;
    private TemporalEnablerScript attackR2;
    private TemporalEnablerScript attackL2;
    bool touchesGround = false;
    bool inventoryVisible = false;
    PlayerState currentState = PlayerState.STAND;
    HeldItemType heldItemType = HeldItemType.NONE;

    private void Awake()
    {
        damagerR = AttackR.GetComponent<DamagerScript>();
        damagerL = AttackL.GetComponent<DamagerScript>();
        damagerR2 = AttackR2.GetComponent<DamagerScript>();
        damagerL2 = AttackL2.GetComponent<DamagerScript>();
        attackR = AttackR.GetComponent<TemporalEnablerScript>();
        attackL = AttackL.GetComponent<TemporalEnablerScript>();
        attackR2 = AttackR2.GetComponent<TemporalEnablerScript>();
        attackL2 = AttackL2.GetComponent<TemporalEnablerScript>();
        hii = SelectedItemDisplay.GetComponent<HeldItemImage>();

        renderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<PlayerInventoryScript>();
        hps = GetComponent<HealthPointsScript>();
        pes = GetComponent<PotionEffectsScript>();
        cam = Camera.GetComponent<Camera>();

        throwers.Clear();
        throwers.Add("BOMB", Thrower_Bomb.GetComponent<PlayerThrowScript>());
        throwers.Add("AF_P", Thrower_AF_P.GetComponent<PlayerThrowScript>());
        throwers.Add("AF_M", Thrower_AF_M.GetComponent<PlayerThrowScript>());
        throwers.Add("AF_F", Thrower_AF_F.GetComponent<PlayerThrowScript>());
        throwers.Add("AF_A", Thrower_AF_A.GetComponent<PlayerThrowScript>());
        throwers.Add("AF_H", Thrower_AF_H.GetComponent<PlayerThrowScript>());
        throwers.Add("PT_V", Thrower_PT_V.GetComponent<PlayerThrowScript>());
        throwers.Add("AR_R", Thrower_AR_R.GetComponent<PlayerThrowScript>());
        throwers.Add("AR_H", Thrower_AR_H.GetComponent<PlayerThrowScript>());
        throwers.Add("AR_F", Thrower_AR_F.GetComponent<PlayerThrowScript>());
        throwers.Add("AR_P", Thrower_AR_P.GetComponent<PlayerThrowScript>());
        throwers.Add("AR_M", Thrower_AR_M.GetComponent<PlayerThrowScript>());

        GroundCheck1 = GroundCheck + new Vector2(0.15f, 0f);
        GroundCheck2 = GroundCheck + new Vector2(-0.15f, 0f);
    }

    bool jump = false;
    bool attack = false;
    bool item = false;
    bool bow = false;
    bool climb = false;
    float stateTimer = 0f;
    void Update()
    {

        if (Input.GetMouseButtonDown(1)) inventoryVisible = !inventoryVisible;
        InventoryObject?.SetActive(inventoryVisible);

        jump = jump || Input.GetButtonDown("Jump");
        attack = attack || Input.GetButtonDown("Fire1");

        bool nattack = !Input.GetButton("Fire1");

        if (currentState == PlayerState.BOW_AIM)
        {
            if (nattack) attack = false;
            else attack = true;
        }

        stateTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        bool touchesGroundNow = CheckGroundCollision(GroundCheck) || CheckGroundCollision(GroundCheck1) || CheckGroundCollision(GroundCheck2);
        bool canClimb = CheckLadderCollision();

        float horizontalMove = Input.GetAxisRaw("Horizontal") * MaxSpeed;

        if (currentState != PlayerState.BOW_AIM && Mathf.Abs(horizontalMove) > 0.1f) renderer.flipX = horizontalMove < 0;

        float verticalAxis = Input.GetAxisRaw("Vertical");

        bool landed = !touchesGround && touchesGroundNow;
        bool fell = touchesGround && !touchesGroundNow;
        bool crouching = !canClimb && verticalAxis < -0.5f;
        bool climbing = canClimb && (verticalAxis < -0.5f || verticalAxis > 0.5f);

        if (climbing) climb = true;
        if (!canClimb) climb = false;

        bool bowaim = false;


        if (!attack && currentState == PlayerState.BOW_AIM)
        {

            switch (heldItem)
            {
                case "AR_R": ShootProjectile(heldItem); break;
                case "AR_H": ShootProjectile(heldItem); break;
                case "AR_F": ShootProjectile(heldItem); break;
                case "AR_P": ShootProjectile(heldItem); break;
                case "AR_M": ShootProjectile(heldItem); break;
            }

            bowaim = false;
        }


        if (attack && heldItemType != HeldItemType.NONE)
        {
            /*USE ITEM*/
            if (heldItemType == HeldItemType.POTION)
            {
                attack = false;
                switch(heldItem)
                {
                    case "PT_R": hps.IncreaseRelative(0.5f); break;
                    case "PT_O": pes.ApplyInvisible(60); break;
                    case "PT_G": pes.ApplyStrength(30); break;
                    case "PT_B": pes.ApplyAgility(30); break;
                }

                inventory.RemoveItem(heldItem);
                SelectInventoryItem("");
            }
            else if (heldItemType == HeldItemType.THROW)
            {
                switch (heldItem)
                {
                    case "BOMB": ShootProjectile(heldItem); break;
                    case "AF_P": ShootProjectile(heldItem); break;
                    case "AF_M": ShootProjectile(heldItem); break;
                }

                if (heldItem == "BOMB")
                {
                    inventory.RemoveItem(heldItem);

                    SelectInventoryItem("");
                }

                attack = false;
            }
            else if (heldItemType == HeldItemType.BOW)
            {
                bowaim = true;
            }
            else if (heldItemType == HeldItemType.THROW_POTION)
            {
                // DISTANCE
                // JAKO THROW NEBO POTION

                switch (heldItem)
                {
                    case "PT_V": pes.CancelPoison(); break;
                    case "AF_H": pes.CancelFrozen(); break;
                    case "AF_F": pes.CancelBurning(); break;
                    case "AF_A": pes.CancelPoison(); break;
                }

                switch (heldItem)
                {
                    case "PT_V": ShootProjectile(heldItem); break;
                    case "AF_H": ShootProjectile(heldItem); break;
                    case "AF_F": ShootProjectile(heldItem); break;
                    case "AF_A": ShootProjectile(heldItem); break;
                }

                attack = false;
            }
        }


        PlayerState previousState = currentState;
        UpdateState(jump, landed, fell, crouching, attack, climb, bowaim);

        rigidbody2D.MovePosition(transform.position + new Vector3(horizontalMove * GetMovementMult(), spdY, 0));

        if (!climb)
        {
            spdY -= Gravity;
            if (spdY < -0.2f) spdY = -0.2f;
        }
        else
        {
            spdY = verticalAxis / 5f;
        }

        if (previousState != PlayerState.JUMP && currentState == PlayerState.JUMP) touchesGround = false;
        //else if (currentState == PlayerState.CLIMB) touchesGround = true;
        else touchesGround = touchesGroundNow;

        jump = false;
        if (currentState != PlayerState.BOW_AIM) attack = false;
    }

    bool CheckGroundCollision(Vector2 checkPositon)
    {
        Vector2 checkPos = new Vector2
            (gameObject.transform.position.x + checkPositon.x,
            gameObject.transform.position.y + checkPositon.y);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPos, new Vector2(0.4f,0.05f), Ground);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if ((Ground.value & (1 << colliders[i].gameObject.layer)) > 0) return true;
            }
        }
        return false;
    }
    bool CheckLadderCollision()
    {
        Vector2 checkPos = new Vector2
            (gameObject.transform.position.x + LadderCheck.x,
            gameObject.transform.position.y + LadderCheck.y);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPos, new Vector2(0.4f, 1.0f), LadderLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if ((LadderLayer.value & (1 << colliders[i].gameObject.layer)) > 0) return true;
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

    public enum PlayerState { STAND, CROUCH, CROUCH_ATK, CROUCH_MOVE, ATK, ITEM, BOW_AIM, JUMP, CLIMB, FALL }
    public enum HeldItemType { NONE, THROW, POTION, THROW_POTION, BOW }

    void EnterState(PlayerState newState)
    {
        if (newState != currentState)
        {
            stateTimer = 0f;

            // LEAVE OLD
            switch (currentState)
            {
                case PlayerState.CLIMB:
                    climb = false;
                    break;
            }

            currentState = newState;

            // ENTER NEW
            switch (currentState)
            {
                case PlayerState.JUMP:
                    ApplyJumpForce();
                    break;
                case PlayerState.ATK:
                    if (renderer.flipX)
                    {
                        attackL.EnableOnce();
                        attackL2.EnableOnce();
                    }
                    else
                    {
                        attackR.EnableOnce();
                        attackR2.EnableOnce();
                    }
                    break;
                case PlayerState.CROUCH_ATK:
                    if (renderer.flipX)
                    {
                        attackL.EnableOnce();
                        attackL2.EnableOnce();
                    }
                    else
                    {
                        attackR.EnableOnce();
                        attackR2.EnableOnce();
                    }
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
                case PlayerState.BOW_AIM:
                    animator.SetBool("Move", false);
                    animator.SetBool("Crouch", false);
                    animator.SetBool("Inventory", false);
                    animator.SetBool("Bow", true);
                    SetBowAngle();
                    animator.SetBool("Attack", false);
                    animator.SetBool("Jump", true);
                    animator.SetBool("Jump2", false);
                    animator.SetBool("Land", false);
                    animator.SetBool("Climb", false);
                    break;
            }
        }
    }
    void UpdateState(bool jump, bool landed, bool fell, bool crouching, bool attack, bool climb, bool bowaim)
    {
        switch (currentState)
        {
            case PlayerState.STAND:
                if (jump) EnterState(PlayerState.JUMP);
                else if (bowaim) EnterState(PlayerState.BOW_AIM);
                else if (crouching) EnterState(PlayerState.CROUCH);
                else if (fell) EnterState(PlayerState.FALL);
                else if (attack) EnterState(PlayerState.ATK);
                else if (climb) EnterState(PlayerState.CLIMB);
                break;
            case PlayerState.JUMP:
                if (landed) EnterState(PlayerState.STAND);
                else if (climb) EnterState(PlayerState.CLIMB);
                break;
            case PlayerState.FALL:
                if (landed) EnterState(PlayerState.STAND);
                else if (climb) EnterState(PlayerState.CLIMB);
                break;
            case PlayerState.CROUCH:
                if (!crouching) EnterState(PlayerState.STAND);
                else if (attack) EnterState(PlayerState.CROUCH_ATK);
                break;
            case PlayerState.ATK:
                if (stateTimer > 0.4f) EnterState(PlayerState.STAND);
                break;
            case PlayerState.CROUCH_ATK:
                if (stateTimer > 0.4f) EnterState(PlayerState.CROUCH);
                break;
            case PlayerState.CLIMB:
                if (jump) EnterState(PlayerState.JUMP);
                else if (!climb && touchesGround) EnterState(PlayerState.STAND);
                else if (!climb && !touchesGround) EnterState(PlayerState.FALL);
                break;
            case PlayerState.BOW_AIM:
                if (!bowaim) EnterState(PlayerState.STAND);
                else SetBowAngle();
                break;
        }
    }

    float GetMovementMult()
    {
        float speed = pes.MoveSpeed;
        
        switch (currentState)
        {
            case PlayerState.STAND: return speed;
            case PlayerState.CROUCH: return speed * 0.33f;
            case PlayerState.CROUCH_ATK: return speed * 0.33f;
            case PlayerState.CROUCH_MOVE: return speed * 0.33f;
            case PlayerState.ATK: return speed;
            case PlayerState.ITEM: return speed;
            case PlayerState.BOW_AIM: return speed * 0.1f;
            case PlayerState.JUMP: return speed;
            case PlayerState.CLIMB: return speed;
            case PlayerState.FALL: return speed;
        }

        return speed;
    }

    public void ApplyJumpForce()
    {
        spdY = JumpForce * jmpMult * Mathf.Sqrt(pes.JumpHeight);
    }
    public void IncreaseJumpHeight()
    {
        jmpMult += 0.05f;
    }
    float dmgMult = 1.0f;
    float jmpMult = 1.0f;
    public void IncreaseAttackMult()
    {
        dmgMult+=0.2001f;
        damagerR.DealtDamage = (int)(30 * dmgMult);
        damagerL.DealtDamage = (int)(30 * dmgMult);
    }

    string heldItem = "";
    public void SelectInventoryItem(string itemType)
    {
        if (inventory.GetCount(itemType) <= 0) heldItem = "";
        else heldItem = itemType;
        inventoryVisible = false;
        SetHeldItemType();

        hii.Set(heldItem);
    }
    void SetHeldItemType()
    {
        switch (heldItem)
        {
            case "BOMB": heldItemType = HeldItemType.THROW; break;

            case "PT_R": heldItemType = HeldItemType.POTION; break;
            case "PT_O": heldItemType = HeldItemType.POTION; break;
            case "PT_V": heldItemType = HeldItemType.THROW_POTION; break;
            case "PT_G": heldItemType = HeldItemType.POTION; break;
            case "PT_B": heldItemType = HeldItemType.POTION; break;

            case "AR_R": heldItemType = HeldItemType.BOW; break;
            case "AR_H": heldItemType = HeldItemType.BOW; break;
            case "AR_F": heldItemType = HeldItemType.BOW; break;
            case "AR_P": heldItemType = HeldItemType.BOW; break;
            case "AR_M": heldItemType = HeldItemType.BOW; break;

            case "AF_H": heldItemType = HeldItemType.THROW_POTION; break;
            case "AF_F": heldItemType = HeldItemType.THROW_POTION; break;
            case "AF_A": heldItemType = HeldItemType.THROW_POTION; break;
            case "AF_P": heldItemType = HeldItemType.THROW; break;
            case "AF_M": heldItemType = HeldItemType.THROW; break;

            default: heldItemType = HeldItemType.NONE; break;
        }
    }

    public GameObject Thrower_Bomb;
    public GameObject Thrower_AF_P;
    public GameObject Thrower_AF_M;
    public GameObject Thrower_AF_F;
    public GameObject Thrower_AF_A;
    public GameObject Thrower_AF_H;
    public GameObject Thrower_PT_V;
    public GameObject Thrower_AR_R;
    public GameObject Thrower_AR_H;
    public GameObject Thrower_AR_F;
    public GameObject Thrower_AR_P;
    public GameObject Thrower_AR_M;
    Dictionary<string, PlayerThrowScript> throwers = new Dictionary<string, PlayerThrowScript>();

    void ShootProjectile(string itemType)
    {
        Vector3 playerScreen = cam.WorldToScreenPoint(gameObject.transform.position);
        Vector3 mouse = Input.mousePosition;
        Vector3 diff = new Vector3(mouse.x - playerScreen.x, mouse.y - playerScreen.y, 0);
        diff = diff.normalized;

        float power = 500;

        throwers[itemType].Throw(gameObject,500 * diff.x, 500 * diff.y);
    }

    public bool InSameRoom(Vector3 position)
    {
        int pRoomX = (int)Math.Floor((gameObject.transform.position.x + RoomSizeX / 2) / RoomSizeX);
        int pRoomY = (int)Math.Floor((gameObject.transform.position.y + RoomSizeY / 2) / RoomSizeY);
        int eRoomX = (int)Math.Floor((position.x + RoomSizeX / 2) / RoomSizeX);
        int eRoomY = (int)Math.Floor((position.y + RoomSizeY / 2) / RoomSizeY);

        return pRoomX == eRoomX && pRoomY == eRoomY;
    }
    public bool InSameRoom1(Vector3 position)
    {
        int eRoomX = (int)Math.Floor((position.x + RoomSizeX / 2) / RoomSizeX);
        int eRoomY = (int)Math.Floor((position.y + RoomSizeY / 2) / RoomSizeY);

        return MaxOneRoomAway(eRoomX, eRoomY);
    }
    public bool MaxOneRoomAway(int roomX, int roomY)
    {
        int pRoomX = (int)Math.Floor((gameObject.transform.position.x + RoomSizeX / 2) / RoomSizeX);
        int pRoomY = (int)Math.Floor((gameObject.transform.position.y + RoomSizeY / 2) / RoomSizeY);

        return Math.Abs(pRoomX - roomX) <= 1 && Math.Abs(pRoomY - roomY) <= 1;
    }

    void SetBowAngle()
    {
        Vector3 playerScreen = cam.WorldToScreenPoint(gameObject.transform.position);
        Vector3 mouse = Input.mousePosition;
        Vector3 diff = new Vector3(mouse.x - playerScreen.x, mouse.y - playerScreen.y, 0);

        float x = diff.x;
        float y = diff.y;

        renderer.flipX = x < 0;

        x = Mathf.Abs(x);

        if (y > 2 * x) animator.SetInteger("BowAngle", 2);
        else if (2 * y > x) animator.SetInteger("BowAngle", 1);
        else animator.SetInteger("BowAngle", 0);
    }

    public bool Visible => pes.Visible;
}
