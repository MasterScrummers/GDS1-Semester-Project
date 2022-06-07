using UnityEngine;

public class PlayerAnim : MonoBehaviour, IAttackReceiver
{
    [SerializeField] private Animator anim; //The player's animation
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

    [SerializeField] private float restartTimer = 5f;
    [SerializeField] private float hurtTimer = 0.5f;
    private float hurtTick = 0f;

    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
        sc = ic.GetComponent<SceneController>();

        rb = GetComponentInParent<Rigidbody2D>();
        jump = rb.GetComponent<JumpComponent>();
        col = jump.GetComponent<Collider2D>();
        health = jump.GetComponent<HealthComponent>();
    }

    void Update()
    {
        void LightAttackCheck()
        {
            if (miscAnim.animState != AnimState.LightAttack)
            {
                return;
            }

            if (ic.GetButtonDown("Attack", "Light"))
            {
                anim.SetTrigger("FollowUp");
            }
        }

        void CheckWalking()
        {
            anim.SetBool("IsWalking", ic.GetAxisRawValues("Movement", "Horizontal") != 0);
        }

        void CheckJumping()
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

            switch(miscAnim.jumpState)
            {
                case JumpState.StartJump:
                    ConditionalTriggerCheck("Jump", rb.velocity.y < 3); //Takes you to peak
                    return;


                case JumpState.Descending:
                    ConditionalTriggerCheck("Jump", !jump.isFalling);
                    return;
            }
        }

        Physics2D.IgnoreLayerCollision(6, 7, invincibility.invincible);
        switch (miscAnim.animState)
        {
            case AnimState.Damage:
                if ((hurtTick -= Time.deltaTime) < 0)
                {
                    anim.SetTrigger("Recover");
                    ic.SetInputLock(false);
                    miscAnim.SetReasonUnlock("Movement");
                }
                return;

            case AnimState.Death:
                if ((restartTimer -= Time.deltaTime) <= 0f)
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

    private void Restart()
    {
        Physics2D.IgnoreLayerCollision(6, 7, false);
        ic.SetInputLock(false);
        col.enabled = true;
        anim.SetTrigger("Restart");
        health.SetHP();
        rb.transform.eulerAngles = Vector3.zero;
        restartTimer = 5f;
        ic.GetComponent<VariableController>().SetLevel(1);
        rb.GetComponent<PlayerInput>().Restart();
    }

    public Animator GetAnimator()
    {
        return anim;
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
        hurtTick = hurtTimer;

        invincibility.StartInvincible(isAlive ? invincibilityTime : 0);
        anim.Play(isAlive ? "KirbyHurt" : "Base Layer.KirbyDeath.KirbyDeathIntro");
        rb.velocity = isAlive ? attackerPos.position.x > transform.position.x ? -knockback: knockback : Vector2.zero;
    }
}
