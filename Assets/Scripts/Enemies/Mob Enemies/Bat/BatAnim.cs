using UnityEngine;

public class BatAnim : MonoBehaviour
{
    private Bat bat; // Bat parent
    private Rigidbody2D rb; // Bat rigidbody
    private Collider2D body;
    private Collider2D platforms;

    private Animator anim; // Bat sprite Animator

    void Start()
    {
        bat = GetComponentInParent<Bat>();
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        body = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (bat.state == Bat.State.Death && rb.velocity.y == 0)
        {
            anim.Play("BatDeathGround");
            enabled = false;
        }
    }

    /// <summary>
    /// For bat death. Should be called through Bat class when death sqeuence is initiated.
    /// </summary>
    public void Death()
    {
        anim.Play("BatDeath");
        if (platforms)
        {
            Physics2D.IgnoreCollision(platforms, body, false);
        }

        rb.gravityScale = 0.5f;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        rb.drag = 0f;
    }

    // Checks for ground contact after death initiated
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && (bat.state == Bat.State.Death || bat.state == Bat.State.DeathEnd))
        {
            Physics2D.IgnoreCollision(other.collider, body);
        }

        if (!platforms && other.gameObject.name.Equals("Platforms"))
        {
            platforms = other.collider;
            Physics2D.IgnoreCollision(platforms, body);
        }
    }
}
