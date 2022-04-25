using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerAnim : MonoBehaviour
{
    public Animator anim { get; private set; } //The player's animation
    private InputController ic; //The Input Controller.
    private Rigidbody2D rb;
    private PlayerInput pi;

    private enum AnimState { Idle, Run, Jump, LightAttack, HeavyAttack, SpecialAttack, Damage, Death };
    private AnimState animState = AnimState.Idle;

    private enum JumpState { Waiting, StartJump, Peak, Descending }
    private JumpState jumpState = JumpState.Waiting;

    private float damageTimer;
    private bool damageLanded;
    private float invincibilityTimer = 1.5f;
    public bool invincible;
    private float deathTimer;
    private float restartTimer = 5f;
    private bool deathJumped;

    private Vector3 pRot;
    public GameObject cutter; //Cutter Game Object
    public int numCutters; //Number of Cutter Spawn
    private GameObject[] cutters; //Array of Cutters

    void Start()
    {
        anim = GetComponent<Animator>();
        ic = DoStatic.GetGameController<InputController>();
        rb = GetComponentInParent<Rigidbody2D>();
        pi = GetComponentInParent<PlayerInput>();
        pRot = DoStatic.GetPlayer().transform.eulerAngles;

        cutters = new GameObject[numCutters];
        for (int i  = 0; i < numCutters; i++)
        {
            cutters[i] = cutter;
        }
    }

    void Update()
    {
        if (animState != AnimState.Damage && animState != AnimState.Death)
        {
            LightAttackCheck();
            HeavyAttackCheck();
            CheckWalking();
            CheckJumping();
        } else if (animState == AnimState.Damage)
        {
            CheckFallenDuringDamage();
        } else if (animState == AnimState.Death)
        {
            if (transform.parent.localScale.x < 0.01f)
            {
                Destroy(transform.parent.gameObject);
            }
            transform.parent.localScale = transform.parent.localScale * 0.993f;
            
            if (restartTimer <= 0f)
            {
                Physics2D.IgnoreLayerCollision(6, 7, false);
                Physics2D.IgnoreLayerCollision(3, 6, false);
                DoStatic.LoadScene(0, false);
            } else {
                restartTimer -= Time.deltaTime;
            }
        }
    }

    private void LightAttackCheck()
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


    //Need fix in the Inpuut Controller
    private void HeavyAttackCheck()
    {
        if(animState != AnimState.HeavyAttack)
        {
            return;
        }

        anim.SetBool("Spin", ic.GetButtonStates("Attack", "Heavy"));
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

    /// <summary>
    /// Used in the animation.
    /// </summary>
    private void SetAnimState(AnimState state)
    {
        animState = state;
    }

    /// <summary>
    /// Used in the animation
    /// </summary>
    private void SetJumpState(JumpState state)
    {
        jumpState = state;
    }

    private void CheckWalking()
    {
        anim.SetBool("IsWalking", ic.GetAxisRawValues("Movement", "Horizontal") != 0);
    }

    private void CheckJumping()
    {
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
                ConditionalTriggerCheck("Jump", !pi.isFalling); //Takes you to peak
                return;
        }
    }

    private void ConditionalTriggerCheck(string animParameter, bool condition)
    {
        if (condition)
        {
            anim.SetTrigger(animParameter);
        }
    }

    public void TakeDamage(int xDirectionForce)
    {
        if (animState != AnimState.Damage)
        {
            invincible = true;
            damageTimer = 0;
            damageLanded = false;
            
            ic.SetInputLock(true);

            anim.Play("Base Layer.KirbyHurt.KirbyHurtAir");
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(xDirectionForce*0.8f, 2) * 100f);
        }  
    }

    private void CheckFallenDuringDamage()
    {
        damageTimer += Time.deltaTime;

        if (damageTimer > 0.5f && pi.OnGround() && !damageLanded)
            {
                damageLanded = true;
                rb.velocity = Vector2.zero;
                anim.SetTrigger("HurtFallenToGround");
            }
    }

    public void ReenableInputAfterDamage()
    {
        ic.SetInputLock(false);
        StartCoroutine("InvincibilityFlashing");
    }

    private IEnumerator InvincibilityFlashing()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        Physics2D.IgnoreLayerCollision(6, 7, true);
        for (int i = 0; i < invincibilityTimer / 0.2f; i++)
        {
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = !sprite.enabled;
        }
        invincible = false;
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    public void Death()
    {
        ic.SetInputLock(true);
        anim.Play("Base Layer.KirbyDeath.KirbyDeathIntro");
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        Physics2D.IgnoreLayerCollision(6, 7, true);
        Physics2D.IgnoreLayerCollision(3, 6, true);
    }

    private void DeathJump()
    {
        if (!deathJumped)
        {
            // rb.constraints = RigidbodyConstraints2D.None;
            // rb.AddRelativeForce(transform.up * 400f);
            deathJumped = true;
        }
    }

    public void DeathRotate()
    {
        Vector3 clockwiseRot = pi.gameObject.transform.eulerAngles;
        clockwiseRot.z -= 90;
        if (clockwiseRot.z == 360)
        {
            clockwiseRot.z = 0;
        }
        pi.gameObject.transform.eulerAngles = clockwiseRot;
    }

    private void SetReasonLock(string ID)
    {
        ic.SetID(ID, false);
    }

    private void SetReasonUnlock(string ID)
    {
        ic.SetID(ID, true);
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
}
