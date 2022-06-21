using UnityEngine;

[RequireComponent(typeof(JumpComponent))]
public class PlayerInput : MonoBehaviour
{
    private InputController ic; //Input controller to check inputs.
    private readonly Timer cooldownTimer = new(10f); //The cooldown timer

    private PlayerAnim playerAnim; //Kirby's animation for the attack
    private AttackDealer dealer; //The attack hitbox when attacking.
    private Rigidbody2D rb; //Kirby Rigidbody2D for the movement

    public bool isSliding = false;
    public OriginalValue<float> speed = new(5);
    public bool allowMovement = true; //Can be manipulated by the animator.

    private JumpComponent jump; //The main jump process.

    [HideInInspector] public bool canInteract = false;
    [HideInInspector] public PlayerWeaponBase lightWeapon; //The assigned light weapon
    [HideInInspector] public PlayerWeaponBase heavyWeapon; //The assigned heavy weapon
    [HideInInspector] public PlayerWeaponBase specialWeapon; //The assigned special weapon

    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();

        playerAnim = GetComponent<PlayerAnim>();
        rb = GetComponent<Rigidbody2D>();
        jump = GetComponent<JumpComponent>();
        dealer = GetComponentInChildren<AttackDealer>();
        Restart();
    }

    public void Restart()
    {
        lightWeapon = PlayerWeaponBase.RandomWeapon(speed);
        heavyWeapon = PlayerWeaponBase.RandomWeapon(speed);
        specialWeapon = PlayerWeaponBase.RandomWeapon(speed);
        cooldownTimer.Finish();

        lightWeapon = heavyWeapon = specialWeapon = new Hammer(speed);
    }

    void Update()
    {
        ic.SetInputReason("Movement", allowMovement);
        if (ic.isInputLocked) //Allows knockback when taking damage.
        {
            rb.velocity = Vector2.zero;
            return;
        }

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
        return 1 - Mathf.Clamp(cooldownTimer.tick / cooldownTimer.timer, 0, 1);
    }

    private void HorizontalMovement()
    {
        if (!isSliding)
        {
            rb.velocity = new(speed.value * ic.GetAxisRawValues("Movement", "Horizontal"), rb.velocity.y);
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
        if (playerAnim.IsAttacking())
        {
            return;
        }

        if (ic.GetButtonDown("Attack", "Light"))
        {
            lightWeapon.LightAttack(playerAnim.anim);
            dealer.SetWeapon(lightWeapon);
        }

        if (ic.GetButtonDown("Attack", "Heavy"))
        {
            heavyWeapon.HeavyAttack(playerAnim.anim);
            dealer.SetWeapon(heavyWeapon);
        }

        cooldownTimer.Update(Time.deltaTime);
        if (cooldownTimer.tick == 0 && ic.GetButtonDown("Attack", "Special"))
        {
            cooldownTimer.SetTimer(specialWeapon.specialCooldown);
            specialWeapon.SpecialAttack(playerAnim.anim);
            dealer.SetWeapon(specialWeapon);
        }
    }
}
