using UnityEngine;

[RequireComponent(typeof(JumpComponent))]
public class PlayerInput : MonoBehaviour
{
    private InputController ic; //Input controller to check inputs.
    private float cooldownTimer = 10f; //The cooldown timer
    private float currCooldownTimer; //Current cooldown tick

    private PlayerAnim playerAnim; //Kirby's animation for the attack
    private AttackDealer dealer; //The attack hitbox when attacking.
    private Rigidbody2D rb; //Kirby Rigidbody2D for the movement

    public bool isSliding { get; private set; } = false;
    public OriginalValue<float> speed = new(5);

    private JumpComponent jump; //The main jump process.

    [HideInInspector] public bool canInteract = false;
    [field: SerializeField] public Transform firePoint { get; private set; } // Fire Point for all sort of range weapon

    [HideInInspector] public WeaponBase lightWeapon; //The assigned light weapon
    [HideInInspector] public WeaponBase heavyWeapon; //The assigned heavy weapon
    [HideInInspector] public WeaponBase specialWeapon; //The assigned special weapon

    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();

        playerAnim = GetComponentInChildren<PlayerAnim>();
        dealer = GetComponentInChildren<AttackDealer>();
        rb = GetComponent<Rigidbody2D>();
        jump = GetComponent<JumpComponent>();

        Restart();
    }

    public void Restart()
    {
        lightWeapon = WeaponBase.RandomWeapon();
        heavyWeapon =  WeaponBase.RandomWeapon();
        specialWeapon = WeaponBase.RandomWeapon(); 
    }

    void Update()
    {
        HorizontalMovement();
        VerticalMovement();
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

    private void HorizontalMovement()
    {
        if (!isSliding)
        {
            Vector2 vel = new(speed.value * ic.GetAxisRawValues("Movement", "Horizontal"), rb.velocity.y);
            rb.velocity = vel;
        }
    }

    private void VerticalMovement()
    {
        if (canInteract && ic.GetButtonDown("Movement", "Interact"))
        {
            canInteract = false;
            return;
        }

        bool isJumpHeld = ic.GetButtonStates("Movement", "Jump");
        if (ic.GetButtonDown("Movement", "Jump"))
        {
            jump.Jump();
            
        }
        jump.JumpHeld(isJumpHeld, Time.deltaTime);
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
}
