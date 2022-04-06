using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnim : MonoBehaviour
{
    public Animator anim { get; private set; } //The player's animation
    private InputController ic; //The Input Controller.
    private Rigidbody2D rb;

    private enum AnimState { Idle, Run, Jump, LightAttack, HeavyAttack, SpecialAttack };
    private AnimState animState = AnimState.Idle;

    void Start()
    {
        anim = GetComponent<Animator>();
        ic = DoStatic.GetGameController<InputController>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        LightAttackCheck();
        IsMoving();
        IsJumping();
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

    //Check if the player is moving or not
    private void IsMoving()
    {
        anim.SetBool("IsMoving", ic.axisRawValues["Horizontal"] != 0);
    }

    //Check if the player is Jumping or not
    private void IsJumping()
    {
        anim.SetBool("IsJumping", rb.velocity.y > 0f);
    }
}
