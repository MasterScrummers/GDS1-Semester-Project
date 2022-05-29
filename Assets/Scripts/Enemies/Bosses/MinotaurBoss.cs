using UnityEngine;

public class MinotaurBoss : Enemy
{
    [SerializeField] private Animator anim;

    [SerializeField] private int direction = 1; //The direction of the enemy.
    [SerializeField] private float movementSpeed = 2; //The movement of the enemy.
    [SerializeField] private float leftBoundary = -1; //The right boundary?
    [SerializeField] private float rightBoundary = 1; //The left boundary?
    [SerializeField] private float attackRadius = 2.0f;
    private GameObject player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        player = DoStatic.GetPlayer();

        rb = GetComponent<Rigidbody2D>();

        if (leftBoundary > rightBoundary)
        {
            float temp = leftBoundary;
            leftBoundary = rightBoundary;
            rightBoundary = temp;
        }

        float oX = transform.position.x;
        leftBoundary += oX;
        rightBoundary += oX;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Vector3 dir = transform.position - player.transform.position; //to check if player is to the right or left of player
        if (direction == 1)
        {
            anim.SetBool("Attack", Vector2.Distance(transform.position, player.transform.position) < attackRadius && dir.x < 0); 
        }
        else
        {
            anim.SetBool("Attack", Vector2.Distance(transform.position, player.transform.position)  < attackRadius && dir.x > 0);
        }
        
        if (!anim.GetBool("Attack") && !anim.GetBool("Dead"))
        {
            Move();
        }

    }

    protected void Move()
    {
        Vector3 sca = transform.localScale;
        if (sca.x < 0 && transform.position.x < leftBoundary || sca.x > 0 && transform.position.x > rightBoundary)
        {
            sca.x *= -1;
            direction *= -1;
            transform.localScale = sca;
        }

        Vector2 vel = rb.velocity;
        vel.x = direction * movementSpeed;
        rb.velocity = vel;
    }

    void OnDrawGizmosSelected()
    {
        float posX = rb ? 0 : transform.position.x;
        Gizmos.DrawLine(new Vector2(posX + leftBoundary, int.MinValue), new Vector2(posX + leftBoundary, int.MaxValue));
        Gizmos.DrawLine(new Vector2(posX + rightBoundary, int.MinValue), new Vector2(posX + rightBoundary, int.MaxValue));

    }

    protected override void Death()
    {
        anim.SetBool("Dead", true);
        
        BoxCollider2D[] colChildren = GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D collider in colChildren)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), collider);
        }
    }
}
