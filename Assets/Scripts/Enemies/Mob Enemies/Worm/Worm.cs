using UnityEngine;

public class Worm : Enemy
{
    private WormAnim wa; // WormAnim script
    private BoxCollider2D bc; // Worm BoxCollider
    public enum State { Idle, Attack, Hiding, Death };
    public State state = State.Idle;

    public float stateTimer;

    public float idleTimeMin, idleTimeMax, attackTimeMin, attackTimeMax, hidingTimeMin, hidingTimeMax;

    protected override void Start()
    {
        base.Start();
        wa = GetComponentInChildren<WormAnim>();
        bc = GetComponent<BoxCollider2D>();
        stateTimer = Random.Range(idleTimeMin, idleTimeMax);
    }

    protected override void DoAction()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            switch(state) {

                case State.Idle:
                    state = DoStatic.RandomBool() ? State.Attack : State.Hiding;
                    stateTimer = Random.Range(idleTimeMin, idleTimeMax);
                    break;
                case State.Attack:
                    state = DoStatic.RandomBool() ? State.Idle : State.Hiding;
                    stateTimer = Random.Range(attackTimeMin, attackTimeMax);
                    break;
                case State.Hiding:
                    state = DoStatic.RandomBool() ? State.Attack : State.Idle;
                    stateTimer = Random.Range(hidingTimeMin, hidingTimeMax);
                    break;
            }
            wa.updateState();

            switch (state) {
                case State.Hiding:
                case State.Death:
                    bc.enabled = false;
                    break;
                case State.Idle:
                case State.Attack:
                    bc.enabled = true;
                    break;
            }
        }
    }

    protected override void Death()
    {
        wa.Death();
    }
}
