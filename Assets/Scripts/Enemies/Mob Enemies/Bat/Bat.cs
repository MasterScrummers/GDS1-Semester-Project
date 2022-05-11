using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    
    // protected Rigidbody2D rb; // Bat Rigidbody
    private CircleCollider2D cc; // Bat CircleCollider
    private BatAnim ba; // BatAnim script

    public enum State { Move, Attack, Death, DeathEnd }; // Bat states
    public State state = State.Move; // Tracks bat current state
    public enum AttackState { AttackStart, Attacking, AttackEnd };
    public AttackState attackState = AttackState.AttackStart;
    
    [SerializeField] private float movementSpeed = 0.01f; //The movement of the enemy
    [SerializeField] private float attackRadius = 2.0f;
    public Vector3 attackTargetPos;
    [SerializeField] private float attackPauseTime = 0.5f;
    private float attackPauseTimer;
    [SerializeField] private float attackCooldown = 2f;
    private float attackCooldownTimer;

    [SerializeField] private float deathTimer = 5;
    private float deathTick;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        ba = GetComponentInChildren<BatAnim>();
        cc = GetComponent<CircleCollider2D>();

        attackPauseTimer = 0f;
        attackCooldownTimer = 0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (state == State.Death && (deathTick -= deathTick > 0 ? Time.deltaTime : 0) < 0)
        {
            FinishDeath();
        }

        if (!isStunned)
        {
            switch(state) {

                case State.Move:
                    Move();
                    break;
                
                case State.Attack:
                    if (hurt)
                    {
                        InterruptAttack();
                    }
                    Attack();
                    break;
            }
        }
          
        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    // Bat movement
    protected override void Move()
    {
        if (rb) 
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < attackRadius && attackCooldownTimer <= 0f && hurt)
            {
                state = State.Attack;
                attackTargetPos = player.transform.position;
                attackPauseTimer = attackPauseTime;
            }
        }
    }

    protected override void Attack()
    {
        switch(attackState) {

            case AttackState.AttackStart:
                if (attackPauseTimer <= 0f)
                {
                    attackState = AttackState.Attacking;
                } else {
                    attackPauseTimer -= Time.deltaTime;
                }
                break;

            case AttackState.Attacking:
                rb.AddForce(Vector3.Normalize(attackTargetPos - transform.position) * 1000f);
                attackState = AttackState.AttackEnd;
                break;

            case AttackState.AttackEnd:
                if (Vector2.Distance(transform.position, attackTargetPos) < 0.01f || Vector2.Distance(rb.velocity, Vector2.zero) <= 0.01f)
                {
                    state = State.Move;
                    attackState = AttackState.AttackStart;
                    attackCooldownTimer = attackCooldown;
                }
                break;            
        }
    }

    // Bat death
    protected override void Death()
    {
        cc.enabled = false;
        ba.Death();
        deathTick = deathTimer;
    }
    /// <summary>
    /// Bat death base logic - Shouldn't need to call this unless in BatAnim
    /// </summary>
    public void FinishDeath()
    {
        base.Death();
    }

    private void InterruptAttack()
    {
        state = State.Move;
        attackState = AttackState.AttackStart;
        attackCooldownTimer = 0f;
        attackPauseTimer = 0f;
    }

    // public override void RecieveAttack(Transform attackPos, int strength, float knockbackStr, float invincibilityTime, WeaponBase.Affinity typing)
    // {
    //     base.RecieveAttack(attackPos, strength, knockbackStr, invincibilityTime, typing);
    //     ba.TakeDamage();
    // }
}
