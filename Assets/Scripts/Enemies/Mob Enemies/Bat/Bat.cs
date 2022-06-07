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
    
    [Header("Bat Parameters")]
    [SerializeField] private float movementSpeed = 1; //The movement of the enemy
    [SerializeField] private float attackSpeed = 1;
    [SerializeField] private float attackRadius = 2.0f;

    [SerializeField] private float idleTimer = 0.5f;
    [SerializeField] private float anticipationTimer = 0.5f;
    [SerializeField] private float attackTimer = 0.5f;
    [SerializeField] private float deathTimer = 5;
    private Timer aiTimer;

    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        ba = GetComponentInChildren<BatAnim>();
        cc = GetComponent<CircleCollider2D>();
        player = DoStatic.GetPlayer<Transform>();
        playerPointer = GetComponentInChildren<PlayerPointer>().transform;
        aiTimer = new(attackTimer);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        aiTimer.Update(Time.deltaTime);
        if (isStunned)
        {
            if (state == State.Attack)
            {
                InterruptAttack();
            }
            return;
        }

        DoStatic.SimpleDelegate simple = state switch
        {
            State.Move => Move,
            State.Attack => Attack,
            State.Death => aiTimer.tick < 0 ? RemoveEnemy : null,
            _ => null
        };
        simple?.Invoke();
    }

    // Bat movement
    protected void Move()
    {
        rb.velocity = movementSpeed * playerPointer.right;
        if (Vector2.Distance(transform.position, player.position) < attackRadius && aiTimer.tick <= 0f)
        {
            state = State.Attack;
            aiTimer.SetTimer(anticipationTimer);
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
                if (aiTimer.tick <= 0f)
                {
                    attackState = AttackState.Attacking;
                    aiTimer.SetTimer(attackTimer);
                }
                break;

            case AttackState.Attacking:
                rb.velocity = vel;
                if (aiTimer.tick <= 0f)
                {
                    attackState = AttackState.AttackEnd;
                }
                break;

            case AttackState.AttackEnd:
                state = State.Move;
                attackState = AttackState.AttackStart;
                aiTimer.SetTimer(idleTimer);
                break;
        }
    }

    // Bat death
    protected override void Death()
    {
        state = State.Death;
        cc.enabled = false;
        ba.Death();
        aiTimer.SetTimer(deathTimer);
    }

    private void InterruptAttack()
    {
        state = State.Move;
        attackState = AttackState.AttackStart;
        aiTimer.Finish();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
