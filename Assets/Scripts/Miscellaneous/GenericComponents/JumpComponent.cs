#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpComponent : MonoBehaviour
{
    private Rigidbody2D rb; //The body to do the calculations.
    public bool isFalling { get; private set; } = false;//Is player falling?
    public bool hasJumped { get; private set; } = false; //Was the jump pressed?
    public bool canJump { get; private set; } = true; //Can the body jump?
    public bool isJumpHeld { get; private set; } = false; //Was the jump button held after inital jump?

    [SerializeField] private float baseJumpForce = 8; //The initial jump force
    [SerializeField] private OriginalValue<int> numberOfJumps = new(1);
    [SerializeField] private Timer jumpHeldTimer = new(0.25f); //The timer for jumping.
    [SerializeField] private Timer coyoteTimer = new(0.2f); //The timer for jumping after walking off edge.

    [SerializeField] private OriginalValue<float> normalGravity = new(5); //The original gravity
    [field: SerializeField] public OriginalValue<float> fallGravity { get; private set; } = new(6); //Gravity when falling

    [SerializeField] private Transform feet; //Kirby's feet, to check if it is colliding with the ground
    [SerializeField] private float radius; //the float groundCheckRadius allows you to set a radius for the groundCheck, to adjust the way you interact with the ground
    [SerializeField] private LayerMask ground;

    private AudioController ac;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ac = DoStatic.GetGameController<AudioController>();
    }

    private void Update()
    {
        if (isFalling)
        {
            coyoteTimer.Update(Time.deltaTime);
            if (!hasJumped && coyoteTimer.tick == 0)
            {
                hasJumped = true;
                numberOfJumps.value--;
            }

            if (OnGround())
            {
                coyoteTimer.Reset();
                jumpHeldTimer.Reset();
                numberOfJumps.Reset();
                hasJumped = false;
                isJumpHeld = false;
            }
        }

        rb.gravityScale = rb.velocity.y < -0.1f ? fallGravity.value : normalGravity.value;
        isFalling = rb.gravityScale == fallGravity.value;
        canJump = numberOfJumps.value > 0;
    }

    public void Jump()
    {
        if (!canJump)
        {
            return;
        }

        ac.PlaySound("Jump");
        hasJumped = true;
        isJumpHeld = true;
        numberOfJumps.value--;
        coyoteTimer.Finish();

        Vector2 vel = rb.velocity;
        vel.y = baseJumpForce;
        rb.velocity = vel;
    }

    public void JumpHeld(bool heldCheck, float deltaTime)
    {
        isJumpHeld = isJumpHeld && heldCheck && jumpHeldTimer.tick > 0;
        if (!isJumpHeld)
        {
            return;
        }
        jumpHeldTimer.Update(deltaTime);

        Vector2 vel = rb.velocity;
        vel.y = baseJumpForce;
        rb.velocity = vel;
    }

    private bool OnGround()
    {
        return Physics2D.OverlapCircle(feet.position, radius, ground);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(feet.position, radius);
    }
}
