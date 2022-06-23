using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : Enemy
{
    public enum State { Move, AttackLine, AttackCircle, Death, DeathEnd };
    public State state = State.Move;    
    private SorcererAnim sa; // SorcererAnim script
    private CapsuleCollider2D cc; // Sorcerer CapsuleCollider
    public float stateTimer;
    public float moveTimeMin, moveTimeMax, attackTimeMin, attackTimeMax;

    [SerializeField]private float upperBoundary = 0;
    [SerializeField]private float lowerBoundary = -6;

    [SerializeField] private float movementSpeed = 1;
    private float direction = 1; // Up, -1 for down

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        sa = GetComponentInChildren<SorcererAnim>();
        cc = GetComponent<CapsuleCollider2D>();

        float oY = transform.position.y;
        upperBoundary += oY;
        lowerBoundary += oY;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (state == State.Move)
        {
            Move();
        }

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            switch(state) {

                case State.Move:
                    state = DoStatic.RandomBool() ? State.AttackLine : State.AttackCircle;
                    rb.velocity = Vector3.zero;
                    stateTimer = Random.Range(attackTimeMin, attackTimeMax);
                    break;
                case State.AttackLine:
                case State.AttackCircle:
                    state = State.Move;
                    stateTimer = Random.Range(moveTimeMin, moveTimeMax);
                    break;
            }
        sa.UpdateState();
        }
    }

    protected void Move() 
    {
        if (transform.position.y < lowerBoundary)
        {
            direction = 1;
        } else if (transform.position.y > upperBoundary)
        {
            direction = -1;
        }

        Vector2 vel = rb.velocity;
        vel.y = direction * movementSpeed;
        rb.velocity = vel;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 3)
        {
            Physics2D.IgnoreCollision(other.collider, cc);
        }
    }

    void OnDrawGizmosSelected()
    {
        float posY = rb ? 0 : transform.position.y;
        Gizmos.DrawLine(new Vector2(int.MinValue, posY + upperBoundary), new Vector2(int.MaxValue, posY + upperBoundary));
        Gizmos.DrawLine(new Vector2(int.MinValue, posY + lowerBoundary), new Vector2(int.MaxValue, posY + lowerBoundary));
    }
}
