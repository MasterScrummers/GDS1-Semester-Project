#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public class Bat : Enemy
{
    private Transform player;
    private Transform playerPointer;
    private Vector2 vel;

    public enum State { Move, Attack, Death, DeathEnd }; // Bat states
    private enum AttackState { AttackStart, Attacking, AttackEnd };
    private AttackState attackState = AttackState.AttackStart;

    [Header("Bat Parameters")]
    [SerializeField] private float movementSpeed = 2; //The movement of the enemy
    [SerializeField] private float attackSpeed = 10;
    [SerializeField] private float attackRadius = 3f;

    [SerializeField] private float idleTimer = 3f;
    [SerializeField] private float anticipationTimer = 0.5f;
    [SerializeField] private float attackTimer = 0.5f;
    [SerializeField] public State state { get; private set; } = State.Move; // Tracks bat current state
    private Timer aiTimer;

    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = DoStatic.GetPlayer<Transform>();
        playerPointer = GetComponentInChildren<PlayerPointer>().transform;
        aiTimer = new(attackTimer);
    }

    // Update is called once per frame
    protected override void DoAction()
    {
        aiTimer.Update(Time.deltaTime);
        DoStatic.SimpleDelegate simple = state switch
        {
            State.Move => Move,
            State.Attack => Attack,
            _ => Move
        };
        simple?.Invoke();
    }

    protected override void DoStunnedAction()
    {
        if (state == State.Attack)
        {
            InterruptAttack();
        }
    }

    private void Move()
    {
        rb.velocity = movementSpeed * playerPointer.right;
        if (aiTimer.tick == 0f && Vector2.Distance(transform.position, player.position) < attackRadius)
        {
            state = State.Attack;
            aiTimer.SetTimer(anticipationTimer);
            vel = playerPointer.right * attackSpeed;
        }

        Vector3 rot = transform.eulerAngles;
        rot.x = transform.position.x < player.transform.position.x ? -1 : 1;
        transform.eulerAngles = rot;
    }

    private void Attack()
    {
        switch(attackState) {
            case AttackState.AttackStart:
                rb.velocity = Vector2.zero;
                if (aiTimer.tick <= 0f)
                {
                    attackState++;
                    aiTimer.SetTimer(attackTimer);
                }
                break;

            case AttackState.Attacking:
                rb.velocity = vel;
                attackState += aiTimer.tick == 0 ? 1 : 0;
                break;

            case AttackState.AttackEnd:
                state = State.Move;
                attackState = AttackState.AttackStart;
                aiTimer.SetTimer(idleTimer);
                break;
        }
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
