using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnim : MonoBehaviour
{
    public Animator anim { get; private set; } //The player's animation
    private InputController ic; //The Input Controller.
    private Rigidbody2D rb;
    private PlayerInput pi;

    private enum AnimState { Idle, Run, Jump, LightAttack, HeavyAttack, SpecialAttack };
    private AnimState animState = AnimState.Idle;

    private enum JumpState { Waiting, StartJump, Peak, Descending }
    private JumpState jumpState = JumpState.Waiting;

    public GameObject Cutter;

    void Start()
    {
        anim = GetComponent<Animator>();
        ic = DoStatic.GetGameController<InputController>();
        rb = GetComponentInParent<Rigidbody2D>();
        pi = GetComponentInParent<PlayerInput>();
    }

    void Update()
    {
        LightAttackCheck();
        CheckWalking();
        CheckJumping();
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
