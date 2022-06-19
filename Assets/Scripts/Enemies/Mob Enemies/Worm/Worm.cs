using UnityEngine;

public class Worm : Enemy
{
    private WormAnim wa; // WormAnim script

    public enum State { Idle, Attack, Hiding };

    private Timer stateTimer;
    [field: Header("Worm Parameters"), SerializeField] public State wormState { get; private set; } = State.Idle;

    [Header("Worm Parameters (Min(x) / Max(y)) Timers")]
    [SerializeField] private Vector2 idleTime;
    [SerializeField] private Vector2 attackTime;
    [SerializeField] private Vector2 hidingTime;

    protected override void Start()
    {
        base.Start();
        wa = GetComponent<WormAnim>();
        stateTimer = new(RandomTime(wormState));
    }

    protected override void DoAction()
    {
        stateTimer.Update(Time.deltaTime);
        if (stateTimer.tick > 0f)
        {
            return;
        }
        UpdateState();
    }

    private void UpdateState()
    {
        switch (wormState)
        {
            case State.Idle:
                wormState = DoStatic.RandomBool() ? State.Attack : State.Hiding;
                break;

            case State.Attack:
                wormState = DoStatic.RandomBool() ? State.Idle : State.Hiding;
                break;

            case State.Hiding:
                wormState = DoStatic.RandomBool() ? State.Idle : State.Attack;
                break;
        }
        stateTimer.SetTimer(RandomTime(wormState));
        wa.UpdateState(wormState);
    }

    private float RandomTime(State state)
    {
        Vector2 range = state switch
        {
            State.Idle => idleTime,
            State.Attack => attackTime,
            State.Hiding => hidingTime,
            _ => idleTime,
        };

        return Random.Range(range.x, range.y);
    }

    protected override void Death()
    {
        wa.Death();
    }
}
