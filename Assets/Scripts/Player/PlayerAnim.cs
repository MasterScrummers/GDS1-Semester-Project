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
    private float invincibilityTimer = 3f;
    public bool invincible;
    private float deathTimer;
    private float restartTimer;

    private Vector3 pRot;
    
    public GameObject Cutter;

    void Start()
    {
        anim = GetComponent<Animator>();
        ic = DoStatic.GetGameController<InputController>();
        rb = GetComponentInParent<Rigidbody2D>();
        pi = GetComponentInParent<PlayerInput>();
        pRot = DoStatic.GetPlayer().transform.eulerAngles;
    }

    void Update()
    {
        if (animState != AnimState.Damage && animState != AnimState.Death)
        {
            LightAttackCheck();
            CheckWalking();
            CheckJumping();
        } else if (animState == AnimState.Damage)
        {
            CheckDamage();
            Debug.Log("Checking dmg");
        } else if (animState == AnimState.Death)
        {
            CheckDeath();
        }

        Debug.Log(animState);
    }

    private void LightAttackCheck()
    {
        if (animState != AnimState.LightAttack)
        {
            return;
        }

        if (ic.buttonDowns["Light"])
        {
            anim.SetTrigger("FollowUp");
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

    /// <summary>
    /// Used in the animation.
    /// </summary>
    private void SetAnimState(AnimState state)
    {
        Debug.Log("Setting animation state");
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
        anim.SetBool("IsWalking", ic.axisRawValues["Horizontal"] != 0);
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
            
            if (!ic.inputLock)
            {
                ic.ToggleInputLock();
            }

            anim.Play("Base Layer.KirbyHurt.KirbyHurtAir");
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(xDirectionForce*0.8f, 2) * 100f);
        }  
    }

    private void CheckDamage()
    {
        Debug.Log("Checking dmg");
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
        if (ic.inputLock) {
            ic.ToggleInputLock();
        }
        StartCoroutine("FlashingKirby");
    }

    private IEnumerator FlashingKirby()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        for (int i = 0; i < invincibilityTimer / 0.2f; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().enabled = true;
        }
        invincible = false;
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    public void Death()
    {
        if (!ic.inputLock)
        {
            ic.ToggleInputLock();
        }
        anim.Play("Base Layer.KirbyHurt.KirbyDeath");
    }

    private void CheckDeath()
    {
        if (deathTimer >= 1.0f)
        {
            rb.AddForce(transform.up * 400f);
            Physics2D.IgnoreLayerCollision(6, 4, false);
            deathTimer = -1;
        } else if (deathTimer != -1) 
        {
            deathTimer += Time.deltaTime;
        }
    }

    public void DeathRotate()
    {
        Vector3 clockwiseRot = pRot;
        clockwiseRot.y += 90;
        if (clockwiseRot.y == 360)
        {
            clockwiseRot.y = 0;
        }
        pRot = clockwiseRot;
    }

    //For Cutter Heavy Attack //
    //Used to move the Kirby Up//
    private void CutterHeavyJump()
    {
        rb.AddForce(transform.up * 400f);
    }

    //For Cutter Special Attack //
    //Use to Activate Cutter //
    private void CutterActivate()
    {
        Cutter.SetActive(true);
    }
}
