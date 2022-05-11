using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy
{
    private WormAnim wa; // WormAnim script 
    public enum State { Idle, Attack, Hiding, Death };
    public State state = State.Idle;

    public float stateTimer;

    public float idleTimeMin, idleTimeMax, attackTimeMin, attackTimeMax, hidingTimeMin, hidingTimeMax;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        wa = GetComponentInChildren<WormAnim>();

        stateTimer = Random.Range(idleTimeMin, idleTimeMax);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            switch(state) {

                case State.Idle:
                    state = DoStatic.RandomBool() == true ? State.Attack : State.Hiding;
                    break;
                case State.Attack:
                    state = DoStatic.RandomBool() == true ? State.Idle : State.Hiding;
                    
                    break;
                case State.Hiding:
                    state = DoStatic.RandomBool() == true ? State.Attack : State.Idle;
                    stateTimer = Random.Range(idleTimeMin, idleTimeMax);
                    break;
            }

            switch(state) {

                case State.Idle:
                    stateTimer = Random.Range(idleTimeMin, idleTimeMax);
                    break;
                case State.Attack:
                    stateTimer = Random.Range(attackTimeMin, attackTimeMax);
                    break;
                case State.Hiding:
                    stateTimer = Random.Range(hidingTimeMin, hidingTimeMax);
                    break;
            }

        wa.updateState();
        }
    }

    protected override void Attack()
    {
        
    }

    protected override void Death()
    {
        wa.Death();
    }

    public void FinishDeath()
    {
        base.Death();
    }
}
