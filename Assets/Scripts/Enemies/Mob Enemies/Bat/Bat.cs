using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    protected GameObject player; // Player
    
    protected Rigidbody2D rb; // Bat Rigidbody
    private CircleCollider2D cc; // Bat CircleCollider
    private BatAnim ba; // BatAnim script

    public enum State { Move, Attack, Death, DeathEnd }; // Bat states
    public State state = State.Move; // Tracks bat current state
    
    [SerializeField] private float movementSpeed = 0.01f; //The movement of the enemy.
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        player = DoStatic.GetPlayer();
        
        rb = GetComponent<Rigidbody2D>();
        ba = GetComponentInChildren<BatAnim>();
        cc = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (state == State.Move)
        {
            Move();
            base.Update();
        }
    }

    // Bat movement
    protected override void Move()
    {
        if (rb) 
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }   
    }

    // Bat death
    protected override void Death()
    {
        cc.enabled = false;
        ba.Death();
    }
    /// <summary>
    /// Bat death base logic - Shouldn't need to call this unless in BatAnim
    /// </summary>
    public void FinishDeath()
    {
        base.Death();
    }

    /// <summary>
    /// For bat taking damage. Should be called through Enemy class. Calls animation and then
    /// base Enemy code.
    /// </summary>
    public override void TakeDamage(int damage)
    {
        ba.TakeDamage();
        base.TakeDamage(damage);
    }
}
