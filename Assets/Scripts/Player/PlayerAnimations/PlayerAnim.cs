using UnityEngine;

public class PlayerAnim : MonoBehaviour, IAttackReceiver
{
    [SerializeField] private Animator anim; //The player's animation
    [SerializeField] private PlayerMiscAnim miscAnim;

    private InputController ic; // Input Controller
    private Rigidbody2D rb; //The rigidbody of the player
    private PlayerInput pi; //The update the animation according to player input.
    private Collider2D col; //The collider of the player. Is disabled upon death.
    private HealthComponent health; //To track the player's health.

    public enum AnimState { Idle, Run, Jump, LightAttack, HeavyAttack, SpecialAttack, Damage, Death };
    public enum JumpState { Waiting, StartJump, Peak, Descending }

    private float damageTimer;
    [SerializeField] private float restartTimer = 5f;


    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();

        rb = GetComponentInParent<Rigidbody2D>();
        pi = rb.GetComponent<PlayerInput>();
        col = pi.GetComponent<Collider2D>();
        health = pi.GetComponent<HealthComponent>();
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

            anim.SetBool("Shield", ic.GetButtonStates("Attack", "Light"));
        }

        void HeavyAttackCheck()
        {
            if(miscAnim.animState != AnimState.HeavyAttack)
            {
                return;
            }

            anim.SetBool("Spin", ic.GetButtonStates("Attack", "Heavy"));
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

            anim.SetBool("IsFalling", pi.isFalling);
            ConditionalTriggerCheck("Jump", pi.hasJumped && (miscAnim.animState == AnimState.Idle || miscAnim.animState == AnimState.Run));
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
                    ConditionalTriggerCheck("Jump", !pi.isFalling);
                    return;
            }
        }

        switch(miscAnim.animState)
        {
            case AnimState.Damage:
                CheckFallenDuringDamage();
                return;

            case AnimState.Death:
                if ((restartTimer -= Time.deltaTime) <= 0f)
                {
                    DoStatic.LoadScene(0, false);
                    Physics2D.IgnoreLayerCollision(6, 7, false);
                    ic.SetInputLock(false);
                    col.enabled = true;
                    rb.transform.position = new Vector2(4.91f, -0.5f); //VERY HARD CODED, CHANGE LATER!
                    rb.velocity = Vector2.zero;
                    anim.SetTrigger("Restart");
                    health.Restart();
                    rb.transform.eulerAngles = Vector3.zero;
                    restartTimer = 5f;
                }
                return;

            default:
                LightAttackCheck();
                HeavyAttackCheck();
                CheckWalking();
                CheckJumping();
                return;
        }
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

    private void CheckFallenDuringDamage()
    {
        damageTimer += Time.deltaTime;
        if (damageTimer > 0.5f && pi.OnGround())
        {
            rb.velocity = Vector2.zero;
            anim.SetTrigger("HurtFallenToGround");
        }
    }

    public void RecieveAttack(Transform attackerPos, int strength, float knockbackStr, float invincibilityTime, WeaponBase.Affinity typing)
    {
        if (miscAnim.animState == AnimState.Damage)
        {
            return;
        }

        damageTimer = 0;
        health.TakeDamage(strength);
        ic.SetInputLock(true);
        Physics2D.IgnoreLayerCollision(6, 7, true);

        anim.Play("Base Layer.Kirby" + (health.health > 0 ? "Hurt.KirbyHurtAir" : "Death.KirbyDeathIntro"));
        rb.velocity = health.health > 0 ? new Vector2(attackerPos.position.x > transform.position.x ? -knockbackStr : knockbackStr, 2) * 2f : Vector2.zero;
    }
}
