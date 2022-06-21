#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public class PlayerAnim : MonoBehaviour, IAttackReceiver
{
    public Animator anim { get; private set; } //The player's animation
    private SpriteRenderer sprite;
    [SerializeField] private GameObject deathEffect;
    private PlayerMiscAnim miscAnim;
    private PlayerInvincibility invincibility;

    private InputController ic; // Input Controller
    private SceneController sc; // Scene Controller
    private Rigidbody2D rb; //The rigidbody of the player
    private JumpComponent jump; //The update the animation according to player input.
    private HealthComponent health; //To track the player's health.

    public enum AnimState { Idle, Run, Jump, LightAttack, HeavyAttack, SpecialAttack };
    public enum JumpState { Waiting, StartJump, Peak, Descending }

    [SerializeField] private Timer restartTimer = new(5f);
    [SerializeField] private float invincibilityBuffer = 0.5f;
    private readonly Timer hurtTimer = new(0.5f);
    private bool isHurt = false;
    private bool isDead = false;

    private void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
        sc = ic.GetComponent<SceneController>();

        anim = GetComponent<Animator>();
        miscAnim = GetComponent<PlayerMiscAnim>();
        rb = GetComponent<Rigidbody2D>();
        jump = GetComponent<JumpComponent>();
        health = GetComponent<HealthComponent>();
        invincibility = GetComponent<PlayerInvincibility>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        restartTimer.Reset();
        hurtTimer.Reset();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        if (isDead)
        {
            restartTimer.Update(delta);
            if (restartTimer.tick == 0)
            {
                sc.RestartScene(Restart);
            }
            return;
        }

        if (isHurt)
        {
            hurtTimer.Update(delta);
            if (ic.isInputLocked && hurtTimer.tick < invincibilityBuffer)
            {
                anim.SetTrigger("Recover");
                ic.SetInputLock(false);
            }
            else if (hurtTimer.tick == 0)
            {
                isHurt = false;
            }
        }

        LightAttackCheck();
        CheckWalking();
        CheckJumping();
    }

    private void LateUpdate()
    {
        if (isHurt)
        {
            invincibility.SetPlayerInvincible(true);
        }
    }

    private void LightAttackCheck()
    {
        if (ic.GetButtonDown("Attack", "Light") && miscAnim.animState == AnimState.LightAttack)
        {
            anim.SetTrigger("FollowUp");
        }
        else if (miscAnim.animState != AnimState.LightAttack)
        {
            anim.ResetTrigger("FollowUp");
        }
    }

    private void CheckWalking()
    {
        anim.SetBool("IsWalking", ic.GetAxisRawValues("Movement", "Horizontal") != 0);
    }

    private void CheckJumping()
    {
        void ConditionalTriggerCheck(string animParameter, bool condition)
        {
            if (condition)
            {
                anim.SetTrigger(animParameter);
            }
        }

        anim.SetBool("IsFalling", jump.isFalling);
        ConditionalTriggerCheck("Jump", jump.hasJumped && (miscAnim.animState == AnimState.Idle || miscAnim.animState == AnimState.Run));
        if (miscAnim.animState != AnimState.Jump)
        {
            return;
        }

        switch (miscAnim.jumpState)
        {
            case JumpState.StartJump:
                ConditionalTriggerCheck("Jump", rb.velocity.y < 3); //Takes you to peak
                return;


            case JumpState.Descending:
                ConditionalTriggerCheck("Jump", !jump.isFalling);
                return;
        }
    }

    private void Restart()
    {
        isDead = false;
        invincibility.enabled = true;
        invincibility.SetPlayerInvincible(false);
        ic.SetInputLock(false);
        health.SetHP();
        restartTimer.Reset();
        ic.GetComponent<VariableController>().SetLevel(1);
        rb.GetComponent<PlayerInput>().Restart();
        anim.enabled = true;
    }

    /// <summary>
    /// A simple check if the player is in the middle of an attack animation.
    /// </summary>
    /// <returns>True of the player is in the middle of an attack animation.</returns>
    public bool IsAttacking()
    {
        foreach (AnimState animation in new AnimState[] { AnimState.LightAttack, AnimState.HeavyAttack, AnimState.SpecialAttack })
        {
            if (miscAnim.animState == animation)
            {
                return true;
            }
        }
        return false;
    }

    public void RecieveAttack(Transform attackerPos, WeaponBase weapon)
    {
        if (invincibility.invincible || isDead)
        {
            return;
        }

        ic.SetInputLock(true);
        health.OffsetHP(-weapon.damage);

        isDead = health.health == 0;
        hurtTimer.SetTimer(weapon.hitInterval + invincibilityBuffer);

        if (isDead)
        {
            anim.enabled = false;
            invincibility.enabled = false;
            sprite.enabled = false;
            miscAnim.Death();
            Instantiate(deathEffect).transform.position = transform.position;
        }
        else
        {
            isHurt = true;
            anim.Play("KirbyHurt");
            rb.velocity = attackerPos.position.x > transform.position.x ? -weapon.knockback : weapon.knockback;
        }
    }
}
