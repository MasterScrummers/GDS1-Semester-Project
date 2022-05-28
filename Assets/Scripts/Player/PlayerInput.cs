using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputController ic; //Input controller to check inputs.
    private float cooldownTimer = 10f; //The cooldown timer
    private float currCooldownTimer; //Current cooldown tick

    private PlayerAnim playerAnim; //Kirby's animation for the attack
    private AttackDealer dealer; //The attack hitbox when attacking.
    private Rigidbody2D rb; //Kirby Rigidbody2D for the movement

    public float speed = 5f; //Speed of the character
    public float orignalspeed; //Orignal Speed

    public bool hasJumped { private set; get; } = false;//Was the jump pressed?
    public bool isFalling { private set; get; } = false;//Is player falling?
    public bool isSliding = false;

    private bool isJumpHeld = false; //Was the jump button held after inital jump?
    public bool canInteract = false;

    [SerializeField] private float baseJumpForce = 5; //The initial jump force
    [SerializeField] private float jumpHoldTimer = 0.75f; //The timer for extra height
    private float holdTimer; //Timer of the jumpHoldTimer;
    private float prevYVel; //Previous Highest Y Velocity
    public float originalGravity { private set; get; } //The original gravity
    public float gravityMultiplier = 3.0f; //Multiplies the gravity when falling
    public float originalGravityMultiplier;

    public float radius; //the float groundCheckRadius allows you to set a radius for the groundCheck, to adjust the way you interact with the ground
    public Transform feet; //Kirby's feet, to check if it is colliding with the ground
    public LayerMask Ground; //A LayerMask which defines what is ground object
    public Transform firePoint; // Fire Point for all sort of range weapon

    [HideInInspector] public WeaponBase lightWeapon; //The assigned light weapon
    [HideInInspector] public WeaponBase heavyWeapon; //The assigned heavy weapon
    [HideInInspector] public WeaponBase specialWeapon; //The assigned special weapon

    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();

        playerAnim = GetComponentInChildren<PlayerAnim>();
        dealer = GetComponentInChildren<AttackDealer>();
        rb = GetComponent<Rigidbody2D>();

        originalGravity = rb.gravityScale;
        orignalspeed = speed;
        originalGravityMultiplier = gravityMultiplier;

        lightWeapon = new Cutter();
        heavyWeapon = new Cutter();
        specialWeapon = new Cutter();
    }

    void Update()
    {
        if (ic.lockedInput)
        {
            return;
        }

        VerticalMovement();
        HorizontalMovement();
        AttackChecks();
    }

    /// <summary>
    /// Get the cooldown percentage.
    /// </summary>
    /// <returns>1 for finished cooldown.</returns>
    public float CooldownPercentage()
    {
        return 1 - Mathf.Clamp(currCooldownTimer / cooldownTimer, 0, 1);
    }

    private void VerticalMovement()
    {
        rb.gravityScale = rb.velocity.y < 0 ? originalGravity * gravityMultiplier : originalGravity;
        isFalling = rb.velocity.y < -1f;

        if (canInteract && ic.GetButtonDown("Movement", "Interact"))
        {
            canInteract = false;
            return;
        }

        if (ic.GetButtonDown("Movement", "Jump") && !hasJumped)
        {
            rb.AddForce(new Vector2(0, baseJumpForce), ForceMode2D.Impulse);
            prevYVel = 0;
            holdTimer = jumpHoldTimer;
            hasJumped = true;
            isJumpHeld = true;
        }
        else if (Mathf.Abs(rb.velocity.y) < 0.1f && Physics2D.OverlapCircle(feet.position, radius, Ground))
        {
            hasJumped = false;
        }

        if (!hasJumped || !isJumpHeld)
        {
            return;
        }

        isJumpHeld = ic.GetButtonStates("Movement", "Jump") && holdTimer > 0;
        if (!isJumpHeld)
        {
            return;
        }

        holdTimer -= Time.deltaTime;
        Vector2 vel = rb.velocity;
        if (vel.y > prevYVel)
        {
            prevYVel = vel.y;
        }
        else
        {
            vel.y = prevYVel;
            rb.velocity = vel;
        }
    }

    private void HorizontalMovement()
    {
        if (!isSliding)
        {
            Vector2 vel = new(speed * ic.GetAxisRawValues("Movement", "Horizontal"), rb.velocity.y);
            rb.velocity = vel;
        }
    }

    private void AttackChecks()
    {
        currCooldownTimer -= Time.deltaTime;
        if (playerAnim.IsAttacking())
        {
            return;
        }

        if (ic.GetButtonDown("Attack", "Light") && lightWeapon != null)
        {
            lightWeapon.LightAttack(playerAnim.GetAnimator());
            dealer.UpdateAttackDealer(lightWeapon);
        }

        if (ic.GetButtonDown("Attack", "Heavy") && heavyWeapon != null)
        {
            heavyWeapon.HeavyAttack(playerAnim.GetAnimator());
            dealer.UpdateAttackDealer(heavyWeapon);
        }

        if (currCooldownTimer < 0 && ic.GetButtonDown("Attack", "Special") && specialWeapon != null)
        {
            cooldownTimer = specialWeapon.specialCooldown;
            currCooldownTimer = cooldownTimer;
            specialWeapon.SpecialAttack(playerAnim.GetAnimator());
            dealer.UpdateAttackDealer(specialWeapon);
        }
    }

    public bool OnGround()
    {
        return Physics2D.OverlapCircle(feet.position, radius, Ground);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(feet.position, radius);
    }
}
