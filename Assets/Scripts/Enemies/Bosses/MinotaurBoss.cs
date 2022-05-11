using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurBoss : Enemy
{
    [SerializeField] private Animator anim;

    [SerializeField] private int direction = 1; //The direction of the enemy.
    [SerializeField] private float movementSpeed = 3; //The movement of the enemy.
    [SerializeField] private float leftBoundary = -1; //The right boundary?
    [SerializeField] private float rightBoundary = 1; //The left boundary?
    [SerializeField] private float attackRadius = 2.0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        player = DoStatic.GetPlayer();

        rb = GetComponent<Rigidbody2D>();

        if (leftBoundary > rightBoundary)
        {
            float temp = leftBoundary;
            leftBoundary = rightBoundary;
            rightBoundary = temp;
        }

        float oX = transform.position.x;
        leftBoundary += oX;
        rightBoundary += oX;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        anim.SetBool("Attack", Vector2.Distance(transform.position, player.transform.position) < attackRadius);
        if (!anim.GetBool("Attack") && !anim.GetBool("Dead"))
        {
            Move();
        }

    }

    // To be implemented
    protected override void Move()
    {
        if (!rb)
        {
            return;
        }

        Vector3 sca = transform.localScale;
        if (sca.x < 0 && transform.position.x < leftBoundary || sca.x > 0 && transform.position.x > rightBoundary)
        {
            sca.x *= -1;
            direction *= -1;
            transform.localScale = sca;
        }

        Vector2 vel = rb.velocity;
        vel.x = direction * movementSpeed;
        rb.velocity = vel;
    }

    // To be implemented
    protected override void Attack()
    {
    }

    void OnDrawGizmosSelected()
    {
        float posX = rb ? 0 : transform.position.x;
        Gizmos.DrawLine(new Vector2(posX + leftBoundary, int.MinValue), new Vector2(posX + leftBoundary, int.MaxValue));
        Gizmos.DrawLine(new Vector2(posX + rightBoundary, int.MinValue), new Vector2(posX + rightBoundary, int.MaxValue));
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("Attack", true);
            Debug.Log("in range " + anim.GetBool("Attack"));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("Attack", false);
            Debug.Log("Player gone " + anim.GetBool("Attack"));
        }
    }*/

    protected override void Death()
    {
        anim.SetBool("Dead", true);
    }

    public void FinishDeath()
    {
        base.Death();
    }
}
