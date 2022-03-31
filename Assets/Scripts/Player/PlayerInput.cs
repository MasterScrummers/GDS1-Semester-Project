using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputController ic; //InputController
    public float cooldownTimer = 10f; //The cooldown timer
    private float currCooldownTimer; //Current cooldown tick

    public float speed = 1f; //Speed of the character

    [HideInInspector] public WeaponBase lightWeapon; //The assigned light weapon
    [HideInInspector] public WeaponBase heavyWeapon; //The assigned heavy weapon
    [HideInInspector] public WeaponBase specialWeapon; //The assigned special weapon

    void Start()
    {
        ic = DoStatic.GetGameController().GetComponent<InputController>();
        currCooldownTimer = cooldownTimer;
    }

    void Update()
    {
        /* Tips for Ayush.
         * 
         * Probably use Rigidbody2D for movement.
         * 
         * Vector3 var = rb.velocity;
         * variable.x *= speed * ic.ic.axisRawValues["Horizontal"];
         * 
         * Use ic.buttonDowns["Jump"] for the jump.
         */

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
