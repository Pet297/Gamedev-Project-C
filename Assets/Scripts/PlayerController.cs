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

    //custom gravity impl
    float spdY = -0.2f;

    public GameObject AttackL;
    public GameObject AttackR;
    public GameObject InventoryObject;

    private Rigidbody2D rigidbody2D;
    private SpriteRenderer renderer;
    private Animator animator;
    private DamagerScript damagerR;
    private DamagerScript damagerL;
    private PlayerInventoryScript inventory;
    private HealthPointsScript hps;
    private PotionEffectsScript pes;

    private TemporalEnablerScript attackR;
    private TemporalEnablerScript attackL;
    bool touchesGround = false;
    bool inventoryVisible = false;
    PlayerState currentState = PlayerState.STAND;
    HeldItemType heldItemType = HeldItemType.NONE;

    private void Awake()
    {
        damagerR = AttackR.GetComponent<DamagerScript>();
        damagerL = AttackL.GetComponent<DamagerScript>();
        attackR = AttackR.GetComponent<TemporalEnablerScript>();
        attackL = AttackL.GetComponent<TemporalEnablerScript>();

        renderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<PlayerInventoryScript>();
        hps = GetComponent<HealthPointsScript>();
        pes = GetComponent<PotionEffectsScript>();

        throwers.Clear();
        throwers.Add("BOMB", Thrower_Bomb.GetComponent<PlayerThrowScript>());
        throwers.Add("AF_P", Thrower_AF_P.GetComponent<PlayerThrowScript>());
        throwers.Add("AF_M", Thrower_AF_M.GetComponent<PlayerThrowScript>());
        throwers.Add("AF_F", Thrower_AF_F.GetComponent<PlayerThrowScript>());
        throwers.Add("AF_A", Thrower_AF_A.GetComponent<PlayerThrowScript>());
        throwers.Add("AF_H", Thrower_AF_H.GetComponent<PlayerThrowScript>());
        throwers.Add("PT_V", Thrower_PT_V.GetComponent<PlayerThrowScript>());
        throwers.Add("AR_R", Thrower_AR_R.GetComponent<PlayerThrowScript>());
        throwers.Add("AR_H", Thrower_AR_R.GetComponent<PlayerThrowScript>());
        throwers.Add("AR_F", Thrower_AR_R.GetComponent<PlayerThrowScript>());
        throwers.Add("AR_P", Thrower_AR_R.GetComponent<PlayerThrowScript>());
        throwers.Add("AR_M", Thrower_AR_R.GetComponent<PlayerThrowScript>());
    }

    bool jump = false;
    bool attack = false;
    bool item = false;
    bool bow = false;
    float stateTimer = 0f;
    void Update()
    {

        if (Input.GetMouseButtonDown(1)) inventoryVisible = !inventoryVisible;
        InventoryObject?.SetActive(inventoryVisible);

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
                heldItem = "";
                heldItemType = HeldItemType.NONE;
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
                    heldItem = "";
                    heldItemType = HeldItemType.NONE;
                }
            }
            else if (heldItemType == HeldItemType.BOW)
            {
                switch (heldItem)
                {
                    case "AR_R": ShootProjectile(heldItem); break;
                    case "AR_H": ShootProjectile(heldItem); break;
                    case "AR_F": ShootProjectile(heldItem); break;
                    case "AR_P": ShootProjectile(heldItem); break;
                    case "AR_M": ShootProjectile(heldItem); break;
                }
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
            }
        }


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
    public enum HeldItemType { NONE, THROW, POTION, THROW_POTION, BOW }

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
                    ApplyJumpForce();
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

    public void ApplyJumpForce()
    {
        spdY = JumpForce * jmpMult;
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
        throwers[itemType].Throw(5, 0);
    }
}
