#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public class MinotaurBoss : Enemy
{
    public enum MinotaurState { Idle, Walking, Attacking, Death };
    [field: SerializeField, Header("Minotaur Parameters")] public MinotaurState state { get; private set; } = MinotaurState.Idle;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float idleTime = 3f;
    [SerializeField] private float walkTime = 5f;
    [SerializeField] private GameObject[] SpawnableEnemies;

    [SerializeField] private Transform minMeleeBound;
    [SerializeField] private Transform maxMeleeBound;

    private Transform player;
    private Timer aiTimer;

    protected override void Start()
    {
        base.Start();

        player = DoStatic.GetPlayer<Transform>();
        aiTimer = new(idleTime);
    }

    protected override void DoAction()
    {
        aiTimer.Update(Time.deltaTime);
        LookAtPlayer();
        Walk();

        if (aiTimer.tick != 0)
        {
            return;
        }

        switch (state)
        {
            case MinotaurState.Idle:
                anim.SetTrigger("Attacking");
                state = MinotaurState.Walking;
                aiTimer.SetTimer(walkTime);
                break;

            case MinotaurState.Walking:
                anim.SetInteger("AttackType", 3);
                anim.SetBool("InRange", true);
                break;
        }
    }

    protected override void DoStunnedAction()
    {
        if (state == MinotaurState.Idle)
        {
            aiTimer.Finish();
        }
    }

    private void LookAtPlayer()
    {
        Vector3 sca = transform.localScale;
        sca.x = Mathf.Abs(sca.x);
        sca.x = transform.position.x < player.position.x ? sca.x : -sca.x;
        transform.localScale = sca;
    }

    private void Walk()
    {
        if (state == MinotaurState.Walking)
        {
            bool inRange = InMeleeBounds(player.position);
            anim.SetBool("InRange", inRange);

            bool isWaiting = Mathf.Abs(player.position.x - transform.position.x) < 1f;
            anim.SetBool("Waiting", isWaiting);

            rb.velocity = isWaiting ? Vector2.zero : new(transform.localScale.x < 0 ? -speed : speed, 0);
            state = inRange ? MinotaurState.Attacking : state;

            anim.SetInteger("AttackType", player.position.y > transform.position.y ? 0 : Random.Range(1, 3));
        } else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private bool InMeleeBounds(Vector2 pos)
    {
        bool facingRight = transform.localScale.x > 0;
        Vector2 min = facingRight ? minMeleeBound.position : maxMeleeBound.position; //BottomLeft
        Vector2 max = facingRight ? maxMeleeBound.position : minMeleeBound.position; //TopRight
        bool inRange = pos.x > min.x && pos.x < max.x;
        return inRange && pos.y > min.y && pos.y < max.y;
    }

    protected override void Death()
    {
        anim.Play("MinotaurDeath");
    }

    private void LoopToIdle()
    {
        anim.Play("MinotaurIdle");
        state = MinotaurState.Idle;
        aiTimer.SetTimer(idleTime);
    }

    private void SummonMinon()
    {
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            Transform parent = transform.parent;
            GameObject minion = Instantiate(SpawnableEnemies[Random.Range(0, SpawnableEnemies.Length)], parent);
            minion.transform.localPosition = new Vector2(Random.Range(-6.5f, 6.5f), Random.Range(-5.5f, 5.5f));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 min = minMeleeBound.position; //BottomLeft
        Vector2 max = maxMeleeBound.position; //TopRight
        Vector2 bottomRight = new(max.x, min.y);
        Vector2 topLeft = new(min.x, max.y);

        Gizmos.DrawLine(min, bottomRight);
        Gizmos.DrawLine(max, bottomRight);
        Gizmos.DrawLine(min, topLeft);
        Gizmos.DrawLine(max, topLeft);
    }
}
