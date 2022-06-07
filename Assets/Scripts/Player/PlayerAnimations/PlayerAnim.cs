using UnityEngine;

public class PlayerAnim : MonoBehaviour, IAttackReceiver
{
    public Animator anim { get; private set; } //The player's animation
    [SerializeField] private PlayerMiscAnim miscAnim;
    [SerializeField] private PlayerInvincibility invincibility;

    private InputController ic; // Input Controller
    private SceneController sc; // Scene Controller
    private Rigidbody2D rb; //The rigidbody of the player
    private JumpComponent jump; //The update the animation according to player input.
    private Collider2D col; //The collider of the player. Is disabled upon death.
    private HealthComponent health; //To track the player's health.

    public enum AnimState { Idle, Run, Jump, LightAttack, HeavyAttack, SpecialAttack, Damage, Death };
    public enum JumpState { Waiting, StartJump, Peak, Descending }

    [SerializeField] private Timer restartTimer = new(5f);
    [SerializeField] private Timer hurtTimer = new(0.5f);

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        ic = DoStatic.GetGameController<InputController>();
        sc = ic.GetComponent<SceneController>();

        rb = GetComponentInParent<Rigidbody2D>();
        jump = rb.GetComponent<JumpComponent>();
        col = jump.GetComponent<Collider2D>();
        health = jump.GetComponent<HealthComponent>();
    }

    private void Update()
    {
        Physics2D.IgnoreLayerCollision(6, 7, invincibility.invincible);
        float delta = Time.deltaTime;
        switch (miscAnim.animState)
        {
            case AnimState.Damage:
                hurtTimer.Update(delta);
                if (hurtTimer.tick == 0)
                {
                    anim.SetTrigger("Recover");
                    ic.SetInputLock(false);
                    miscAnim.SetReasonUnlock("Movement");
                }
                return;

            case AnimState.Death:
                restartTimer.Update(delta);
                if (restartTimer.tick == 0)
                {
                    sc.RestartScene(Restart);
                }
                return;

            default:
                LightAttackCheck();
                CheckWalking();
                CheckJumping();
                return;
        }
    }

    private void LightAttackCheck()
    {
        if (ic.GetButtonDown("Attack", "Light") && miscAnim.animState == AnimState.LightAttack)
        {
            anim.SetTrigger("FollowUp");
        } else if (miscAnim.animState != AnimState.LightAttack)
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
        Physics2D.IgnoreLayerCollision(6, 7, false);
        ic.SetInputLock(false);
        col.enabled = true;
        anim.SetTrigger("Restart");
        health.SetHP();
        restartTimer.Reset();
        ic.GetComponent<VariableController>().SetLevel(1);
        rb.GetComponent<PlayerInput>().Restart();
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

    public void RecieveAttack(Transform attackerPos, int strength, Vector2 knockback, float invincibilityTime, float stunTime)
    {
        if (invincibility.invincible)
        {
            return;
        }

        ic.SetInputLock(true);
        health.OffsetHP(-strength);

        bool isAlive = health.health > 0;
        hurtTimer.Reset();

        invincibility.StartInvincible(isAlive ? invincibilityTime : 0);
        anim.Play(isAlive ? "KirbyHurt" : "Base Layer.KirbyDeath.KirbyDeathIntro");
        rb.velocity = isAlive ? attackerPos.position.x > transform.position.x ? -knockback : knockback : Vector2.zero;
    }
}
