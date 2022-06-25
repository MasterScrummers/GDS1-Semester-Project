using UnityEngine;

public class BatAnim : MonoBehaviour
{
    private HealthComponent health;
    private Rigidbody2D rb; // Bat rigidbody
    [SerializeField] private Collider2D body;
    private Collider2D platforms;

    private Animator anim; // Bat sprite Animator
    private bool isDead = false;

    void Start()
    {
        health = GetComponent<HealthComponent>();
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead)
        {
            if (rb.velocity.y == 0)
            {
                anim.Play("BatDeathGround");
                enabled = false;
            }
        } else if (health.health == 0)
        {
            isDead = true;
            anim.Play("BatDeath");
            rb.velocity = Vector2.zero;
            rb.gravityScale = 5;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            if (platforms)
            {
                Physics2D.IgnoreCollision(platforms, body, false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (!platforms && other.gameObject.name.Equals("Platforms"))
        {
            platforms = other.collider;
            Physics2D.IgnoreCollision(platforms, body, true);
        }
    }
}
