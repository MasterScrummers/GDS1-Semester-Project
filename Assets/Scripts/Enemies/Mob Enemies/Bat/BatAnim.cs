using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAnim : MonoBehaviour
{
    private Bat bat; // Bat parent
    private Rigidbody2D rb; // Bat rigidbody
    private GameObject player; // Player

    private Animator anim; // Bat sprite Animator
    private BoxCollider2D bc; // Bat sprite Collider
    private SpriteRenderer sr; // Bat sprite SpriteRenderer

    private int facingDirection;

    private bool deathLanded; // To track ground contact after death
    public bool hurt;
    public const float HurtTime = 0.2f;
    private float hurtColourTimer = HurtTime;

    // Start is called before the first frame update
    void Start()
    {
        bat = GetComponentInParent<Bat>();
        rb = GetComponentInParent<Rigidbody2D>();
        player = DoStatic.GetPlayer();
        
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpriteDirection();

        // Checks for landing after death sequence is initiated
        if (deathLanded && bat.state != Bat.State.DeathEnd)
        {
            anim.Play("Base Layer.BatDeathGround");
            bat.state = Bat.State.DeathEnd;
        }

        if (hurt)
        {
            if (hurtColourTimer < 0f)
            {
                sr.color = Color.white;
                hurt = false;
                hurtColourTimer = HurtTime;
            } else {
                hurtColourTimer -= Time.deltaTime;
            }
        }
    }

    // Flips bat sprite according to direction of movement
    private void UpdateSpriteDirection()
    {
        if (bat.state == Bat.State.Attack)
        {
            facingDirection = transform.position.x < bat.attackTargetPos.x ? 1 : -1;
        } else {
            facingDirection = transform.position.x < player.transform.position.x ? 1 : -1;
        }
        
        Vector3 rot = transform.eulerAngles;
        rot.y = facingDirection > 0 ? 0 : 180;
        transform.eulerAngles = rot;
    }

    /// <summary>
    /// For bat taking damage. Should be called through Bat class when damage is taken.
    /// </summary>
    public void TakeDamage()
    {
        sr.color = Color.red;
        hurt = true;
        rb.AddForce(Vector3.Normalize(transform.position - player.transform.position) * 500f);
    }

    /// <summary>
    /// For bat death. Should be called through Bat class when death sqeuence is initiated.
    /// </summary>
    public void Death()
    {
        bat.state = Bat.State.Death;
        bc.enabled = true;
        anim.Play("Base Layer.BatDeath");
        rb.gravityScale = 0.5f;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        rb.drag = 0f;
    }

    // Checks for ground contact after death initiated
    void OnCollisionEnter2D(Collision2D other) {
        if (bat.state == Bat.State.Death)
        {
            if (other.gameObject.layer == 3)
            {
                deathLanded = true;
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (bat.state == Bat.State.Death || bat.state == Bat.State.DeathEnd)
            {
                Physics2D.IgnoreCollision(other.collider, GetComponent<BoxCollider2D>());
            }
        }
    }

    // Automatically called by deathGround animation to start base death logic
    private void FinishDeath()
    {
        bat.FinishDeath();
    }
}
