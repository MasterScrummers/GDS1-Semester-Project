#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

[RequireComponent(typeof(JumpComponent))]
public class PlayerInput : MonoBehaviour
{
    private InputController ic; //Input controller to check inputs.
    private readonly Timer cooldownTimer = new(10f); //The cooldown timer

    private PlayerAnim playerAnim; //Kirby's animation for the attack
    private AttackDealer dealer; //The attack hitbox when attacking.
    private Rigidbody2D rb; //Kirby Rigidbody2D for the movement

    [HideInInspector] public bool isSliding = false;
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
#if UNITY_EDITOR
        lightWeapon = heavyWeapon = specialWeapon = new Hammer();
#endif
        cooldownTimer.Finish();
    }

    void Update()
    {
        if (ic.lockedInput) //Allows knockback when taking damage.
        {
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

        if (ic.GetButtonDown("Attack", "Light") && lightWeapon != null)
        {
            lightWeapon.LightAttack(playerAnim.anim);
            dealer.UpdateAttackDealer(lightWeapon);
        }

        if (ic.GetButtonDown("Attack", "Heavy") && heavyWeapon != null)
        {
            heavyWeapon.HeavyAttack(playerAnim.anim);
            dealer.UpdateAttackDealer(heavyWeapon);
        }

        cooldownTimer.Update(Time.deltaTime);
        if (cooldownTimer.tick == 0 && ic.GetButtonDown("Attack", "Special") && specialWeapon != null)
        {
            cooldownTimer.SetTimer(specialWeapon.specialCooldown);
            specialWeapon.SpecialAttack(playerAnim.anim);
            dealer.UpdateAttackDealer(specialWeapon);
        }
    }
}
