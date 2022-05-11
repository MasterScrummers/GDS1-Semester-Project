using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAnim : MonoBehaviour
{
    private Worm worm; // Worm parent
    private Animator anim; // Worm sprite Animator
    private Worm.State state; // Tracks worm current state
    private Worm.State prevState;

    // Start is called before the first frame update
    void Start()
    {
        worm = GetComponentInParent<Worm>();
        anim = GetComponent<Animator>();

        state = worm.state;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != worm.state)
        {
            updateState();
        }
    }

    public void updateState() {
        prevState = state;
        state = worm.state;

        if (prevState == Worm.State.Hiding)
        {
            anim.Play("Base Layer.WormEmerge");
        }

        switch(state) {

            case Worm.State.Idle:
                anim.SetTrigger("Idle");
                break;
            case Worm.State.Attack:
                anim.SetTrigger("Attack");
                break;
            case Worm.State.Hiding:
                anim.SetTrigger("Burrow");
                break;
        }
    }

    public void Death()
    {
        worm.state = Worm.State.Death;
        anim.Play("Base Layer.WormDeath");
    }

    void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.CompareTag("Player"))
        {
            if (worm.state == Worm.State.Death)
            {
                Physics2D.IgnoreCollision(other.collider, GetComponent<BoxCollider2D>());
            }
        }
    }

    private void FinishDeath()
    {
        worm.FinishDeath();
    }
}
