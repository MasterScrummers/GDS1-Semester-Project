using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    public Animator anim; // Animator
    private Rigidbody2D rb; // Bat Rigidbody
    private SpriteRenderer sr; // Bat Spriterenderer
    private Transform pt; // Player transform
    bool deathState;
    bool deathLanded;
    float facingDirection;
    [SerializeField] private float movementSpeed = 0.01f; //The movement of the enemy.
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        pt = DoStatic.GetPlayer().transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!deathState)
        {
            Move();
            // UpdateSpriteDirection();
            base.Update();
        } else {
            if (deathLanded)
            {
                anim.SetTrigger("Landed");
            }
        }
    }

    protected override void Move()
    {
        if (rb) 
        {
            facingDirection = transform.position.x < pt.position.x ? 1 : -1;
            transform.position = Vector3.MoveTowards(transform.position, DoStatic.GetPlayer().transform.position, movementSpeed * Time.deltaTime);
        }
    }

    private void UpdateSpriteDirection()
    {
        // Vector3 rot = transform.eulerAngles;
        // rot.y = direction == 1 ? 0 : 180;
        // transform.eulerAngles = rot;
    }

    protected override void Death()
    {
        anim.SetTrigger("Death");
        rb.gravityScale = 0.5f;
        base.Death();
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log("Taking damage in bat");
        base.TakeDamage(damage);
        sr.color = Color.red;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (deathState)
        {
            if (other.gameObject.layer == 3)
            {
                deathLanded = true;
            }
        }
    }
}
