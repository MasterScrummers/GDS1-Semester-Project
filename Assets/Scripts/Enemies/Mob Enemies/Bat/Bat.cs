using UnityEngine;

public class Bat : Enemy
{
    private CircleCollider2D cc; // Bat CircleCollider
    private BatAnim ba; // BatAnim script

    private Transform player;
    private Transform playerPointer;
    private Vector2 vel;

    public enum State { Move, Attack, Death, DeathEnd }; // Bat states
    public State state { get; private set; } = State.Move; // Tracks bat current state
    private enum AttackState { AttackStart, Attacking, AttackEnd };
    private AttackState attackState = AttackState.AttackStart;
    
    [SerializeField] private float movementSpeed = 1; //The movement of the enemy
    [SerializeField] private float attackSpeed = 1;
    [SerializeField] private float attackRadius = 2.0f;

    [SerializeField] private float idleTimer = 0.5f;
    [SerializeField] private float anticipationTimer = 0.5f;
    [SerializeField] private float attackTimer = 0.5f;
    [SerializeField] private float deathTimer = 5;
    private float tick;

    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        ba = GetComponentInChildren<BatAnim>();
        cc = GetComponent<CircleCollider2D>();
        player = DoStatic.GetPlayer<Transform>();
        playerPointer = GetComponentInChildren<PlayerPointer>().transform;
        tick = 0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        float delta = Time.deltaTime;

        tick -= tick <= 0 ? 0 : delta;
        if (isStunned)
        {
            return;
        }

        DoStatic.SimpleDelegate simple = state switch
        {
            State.Move => Move,
            State.Attack => hurt ? InterruptAttack : Attack,
            State.Death => tick < 0 ? RemoveEnemy : null,
            _ => null
        };
        simple?.Invoke();
    }

    // Bat movement
    protected void Move()
    {
        rb.velocity = movementSpeed * playerPointer.right;
        if (Vector2.Distance(transform.position, player.position) < attackRadius && tick <= 0f && !hurt)
        {
            state = State.Attack;
            tick = anticipationTimer;
            vel = playerPointer.right * attackSpeed;
        }
        Vector3 rot = transform.eulerAngles;
        rot.y = transform.position.x < player.transform.position.x ? 0 : 180;
        transform.eulerAngles = rot;
    }

    protected void Attack()
    {
        switch(attackState) {

            case AttackState.AttackStart:
                rb.velocity = Vector2.zero;
                if (tick <= 0f)
                {
                    attackState = AttackState.Attacking;
                    tick = attackTimer;
                }
                break;

            case AttackState.Attacking:
                rb.velocity = vel;
                if (tick <= 0f)
                {
                    attackState = AttackState.AttackEnd;
                }
                break;

            case AttackState.AttackEnd:
                state = State.Move;
                attackState = AttackState.AttackStart;
                tick = idleTimer;
                break;            
        }
    }

    // Bat death
    protected override void Death()
    {
        state = State.Death;
        cc.enabled = false;
        ba.Death();
        tick = deathTimer;
    }

    private void InterruptAttack()
    {
        state = State.Move;
        attackState = AttackState.AttackStart;
        // rb.velocity = Vector2.zero;
        tick = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
