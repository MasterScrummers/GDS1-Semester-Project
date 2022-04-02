using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(HitBoxAdjustor))]
public class PlayerInput : MonoBehaviour
{
    private InputController ic; //InputController
    public float cooldownTimer = 10f; //The cooldown timer
    private float currCooldownTimer; //Current cooldown tick

    private Animator anim; //Kirby's animation
    private Rigidbody2D rb; //Kirby Rigidbody2D
    private Vector2 vel; //Kirby velocity 

    public float jumpForce; //How High Player Jump
    public float radius; //the float groundCheckRadius allows you to set a radius for the groundCheck, to adjust the way you interact with the ground
    public float speed = 5f; //Speed of the character
    public bool isOnGround; //boolean to define if the player is on the ground or not
    public bool stoppedJumping = true; //Boolean to define if the player stops jumping
    public Transform feet; //Mario's feet, to check if it is colliding with the ground
    public LayerMask Ground; //A LayerMask which defines what is ground object

    [HideInInspector] public WeaponBase lightWeapon; //The assigned light weapon
    [HideInInspector] public WeaponBase heavyWeapon; //The assigned heavy weapon
    [HideInInspector] public WeaponBase specialWeapon; //The assigned special weapon

    void Start()
    {
        ic = DoStatic.GetGameController().GetComponent<InputController>();
        currCooldownTimer = cooldownTimer;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        vel = rb.velocity;
    }

    void Update()
    {
        /* Tips for Ayush.
         * 
         * Probably use Rigidbody2D for movement.
         * 
         * Vector3 var = rb.velocity;
         * variable.x *= speed * ic.axisRawValues["Horizontal"];
         * 
         * Use ic.buttonDowns["Jump"] for the jump.
         */

        Jump();

        //Horizontal Movement
        if (ic.axisRawValues["Horizontal"] != 0)
        {
            vel = new Vector2(speed * ic.axisRawValues["Horizontal"], rb.velocity.y);
            rb.velocity = vel;
        }
        else
        {
            vel = new Vector2(0, rb.velocity.y);
            rb.velocity = vel;
        }

        if (ic.buttonDowns["Light"] && lightWeapon)
        {
            lightWeapon.LightAttack(anim);
        }

        if (ic.buttonDowns["Heavy"] && heavyWeapon)
        {
            heavyWeapon.HeavyAttack(anim);
        }

        if (currCooldownTimer < 0 && ic.buttonDowns["Special"] && specialWeapon)
        {
            currCooldownTimer = cooldownTimer;
            specialWeapon.SpecialAttack(anim);
        } else
        {
            currCooldownTimer -= Time.deltaTime;
        }
    }

    void Jump()
    {
        isOnGround = Physics2D.OverlapCircle(feet.position, radius, Ground); //Physics2D.OverlapCircle returns true if the bottom circle collide with the ground layerMask.
        feet.gameObject.SetActive(!isOnGround); //Disable Feet so Kirby is not permenantly "on ground"

        // If (Player Press the space bar or w) and Kirby is on the ground
        if ((ic.buttonDowns["Jump"]) && isOnGround && stoppedJumping)
        {
            //Then Jump
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            stoppedJumping = false;
        }

        // If Player release space bar
        if (!ic.buttonDowns["Jump"] && rb.velocity.y == 0)
        {
            //Stop Jumping 
            stoppedJumping = true;
        }

        //Drop Kirby Faster
        rb.gravityScale = 1;
        if (!isOnGround)
        {
            rb.gravityScale = rb.velocity.y > 1 ? 1 : 5;
        }
    }
}
