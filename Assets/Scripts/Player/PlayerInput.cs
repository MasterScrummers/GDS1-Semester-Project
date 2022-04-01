using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputController ic; //InputController
    public float cooldownTimer = 10f; //The cooldown timer
    private float currCooldownTimer; //Current cooldown tick
    private Rigidbody2D rb; //Kirby Rigidbody2D
    private Vector2 vel; //Kirby velocity 

    public float speed = 5f; //Speed of the character

    [HideInInspector] public WeaponBase lightWeapon; //The assigned light weapon
    [HideInInspector] public WeaponBase heavyWeapon; //The assigned heavy weapon
    [HideInInspector] public WeaponBase specialWeapon; //The assigned special weapon

    void Start()
    {
        ic = DoStatic.GetGameController().GetComponent<InputController>();
        currCooldownTimer = cooldownTimer;
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
            lightWeapon.LightAttack();
        }

        if (ic.buttonDowns["Heavy"] && heavyWeapon)
        {
            heavyWeapon.HeavyAttack();
        }

        if (currCooldownTimer < 0 && ic.buttonDowns["Special"] && specialWeapon)
        {
            currCooldownTimer = cooldownTimer;
            specialWeapon.SpecialAttack();
        } else
        {
            currCooldownTimer -= Time.deltaTime;
        }
    }
}
