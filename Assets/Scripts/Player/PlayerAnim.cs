using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInvincibility))]
public class PlayerAnim : MonoBehaviour
{
    public Animator anim { get; private set; } //The player's animation
    private InputController ic; // Input Controller
    private AudioController ac; // Audio Controller
    private Rigidbody2D rb; //The rigidbody of the player
    private PlayerInput pi; //The update the animation according to player input.
    private Collider2D col; //The collider of the player. Is disabled upon death.
    private PlayerInvincibility invincibility; //To start the player's invincibility when they take damage.
    private HealthComponent health; //To track the player's health.

    private enum AnimState { Idle, Run, Jump, LightAttack, HeavyAttack, SpecialAttack, Damage, Death };
    private AnimState animState = AnimState.Idle;

    private enum JumpState { Waiting, StartJump, Peak, Descending }
    private JumpState jumpState = JumpState.Waiting;

    private float damageTimer;
    [SerializeField] private float restartTimer = 5f;

    public GameObject cutter; //Cutter Game Object
    public int numCutters; //Number of Cutter Spawn
    private GameObject[] cutters; //Array of Cutters

    void Start()
    {
        anim = GetComponent<Animator>();
        
        ic = DoStatic.GetGameController<InputController>();
        ac = ic.GetComponent<AudioController>();

        rb = GetComponentInParent<Rigidbody2D>();
        pi = GetComponentInParent<PlayerInput>();
        col = pi.GetComponent<Collider2D>();
        invincibility = GetComponent<PlayerInvincibility>();
        health = pi.GetComponent<HealthComponent>();

        cutters = new GameObject[numCutters];
        for (int i  = 0; i < numCutters; i++)
        {
            cutters[i] = cutter;
        }
    }

    void Update()
    {
        void LightAttackCheck()
        {
            if (animState != AnimState.LightAttack)
            {
                return;
            }

            if (ic.GetButtonDown("Attack", "Light"))
            {
                anim.SetTrigger("FollowUp");
            }
        }

        void HeavyAttackCheck()
    {
        if(animState != AnimState.HeavyAttack)
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
            ConditionalTriggerCheck("Jump", pi.hasJumped && (animState == AnimState.Idle || animState == AnimState.Run));
            if (animState != AnimState.Jump)
            {
                return;
            }

            switch(jumpState)
            {
                case JumpState.StartJump:
                    ConditionalTriggerCheck("Jump", rb.velocity.y < 3); //Takes you to peak
                    return;


                case JumpState.Descending:
                    ConditionalTriggerCheck("Jump", !pi.isFalling);
                    return;
            }
        }

        switch(animState)
        {
            case AnimState.Damage:
                CheckFallenDuringDamage();
                return;

            case AnimState.Death:
                if ((restartTimer -= Time.deltaTime) <= 0f)
                {
                    DoStatic.LoadScene(0, false);
                    Physics2D.IgnoreLayerCollision(6, 7, false);
                    col.enabled = true;
                    ic.SetInputLock(false);
                    rb.transform.position = new Vector2(4.91f, 5);
                    rb.velocity = Vector2.zero;
                    anim.SetTrigger("Restart");
                    health.Restart();
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

    /// <summary>
    /// A simple check if the player is in the middle of an attack animation.
    /// </summary>
    /// <returns>True of the player is in the middle of an attack animation.</returns>
    public bool IsAttacking()
    {
        foreach (AnimState animation in new AnimState[] { AnimState.LightAttack, AnimState.HeavyAttack, AnimState.SpecialAttack })
        {
            if (animState == animation)
            {
                return true;
            }
        }
        return false;
    }

    public void TakeDamage(int xDirectionForce)
    {
        if (animState == AnimState.Damage)
        {
            return;
        }

        damageTimer = 0;

        ic.SetInputLock(true);
        Physics2D.IgnoreLayerCollision(6, 7, true);

        anim.Play(health.health > 0 ? "Base Layer.KirbyHurt.KirbyHurtAir" : "Base Layer.KirbyDeath.KirbyDeathIntro");
        rb.velocity = health.health > 0 ? new Vector2(xDirectionForce, 2) * 2f : Vector2.zero;
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

    #region Called through animation methods.
    private void SetAnimState(AnimState state)
    {
        animState = state;
    }

    private void SetJumpState(JumpState state)
    {
        jumpState = state;
    }

    private void ReenableInputAfterDamage()
    {
        ic.SetInputLock(false);
        invincibility.StartInvincible();
    }

    private void DeathJump()
    {
        col.enabled = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(transform.up * 10f, ForceMode2D.Impulse);
    }

    private void DeathRotate()
    {
        Vector3 rot = pi.transform.eulerAngles;
        rot.z -= 90;
        pi.transform.eulerAngles = rot;
    }

    private void SetReasonLock(string ID)
    {
        ic.SetID(ID, false);
    }

    private void SetReasonUnlock(string ID)
    {
        ic.SetID(ID, true);
    }

    private void PlaySound(string clipName)
    {
        ac.PlaySound(clipName);
    }

    //For Cutter Heavy Attack //
    //Used to move the Kirby Up//
    private void CutterHeavyJump()
    {
        rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        pi.gravityMultiplier = 2.5f;
    }

    private void ResetGravityMultiplier()
    {
        pi.gravityMultiplier = 1.2f;
    }

    //For Cutter Special Attack //
    //Use to Activate Cutter //
    private void CutterActivate()
    {
        foreach (GameObject cutter in cutters)
        {
            Instantiate(cutter, pi.firePoint.position, Quaternion.identity);
        }
    }
    #endregion Called through animation methods.
}
