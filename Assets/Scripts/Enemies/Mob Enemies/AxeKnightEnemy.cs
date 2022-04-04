using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeKnightEnemy : Enemy
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField] public int direction = 1;
    [SerializeField] public float movementSpeed = 3;
    [SerializeField] public Vector2 patrolLeft;
    [SerializeField] public Vector2 patrolRight;

    // Dictionary<jumpPoint, directionToApproach>
    [SerializeField] Dictionary<Vector2, float> jumpPoints;

    
    private bool canTurn;

    // Start is called before the first frame update
    protected override void Start()
    {
        health = 1;
        type = WeaponBase.Affinity.fire;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        canTurn = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        Move();
        base.Update();
    }

    // To be implemented
    protected override void Move()
    {
        if (rb)
        {
            if ((transform.position.x < patrolLeft.x || transform.position.x > patrolRight.x) && canTurn)
            {
                direction *= -1;
                canTurn = false;
                transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
            } else if (transform.position.x > patrolLeft.x && transform.position.x < patrolRight.x)
            {
                canTurn = true;
            }
            Vector2 vel = rb.velocity;
            vel.x = direction * movementSpeed;
            rb.velocity = vel;
        }
    }

    // To be implemented
    protected override void Attack()
    {
    }

    // To be implemented
    protected override void Death()
    {
        base.Death();
    }
}
