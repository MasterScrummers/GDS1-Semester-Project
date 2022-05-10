using UnityEngine;

public class HurtAnim : MonoBehaviour
{
    private InputController ic; // Input Controller
    private Rigidbody2D rb; //The rigidbody of the player
    private PlayerInput pi; //The update the animation according to player input.
    private Collider2D col; //The collider of the player. Is disabled upon death.
    private PlayerInvincibility invincibility; //To start the player's invincibility when they take damage.


    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
        rb = GetComponentInParent<Rigidbody2D>();
        pi = rb.GetComponent<PlayerInput>();
        col = pi.GetComponent<Collider2D>();
        invincibility = GetComponent<PlayerInvincibility>();
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
}
