using UnityEngine;

public class AxeKnightEnemy : Enemy
{
    private Rigidbody2D rb;

    [SerializeField] private int direction = 1; //The direction of the enemy.
    [SerializeField] private float movementSpeed = 3; //The movement of the enemy.
    [SerializeField] private float leftBoundary = -1; //The right boundary?
    [SerializeField] private float rightBoundary = 1; //The left boundary?

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

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
        Move();
        base.Update();
    }

    // To be implemented
    protected override void Move()
    {
        if (!rb)
        {
            return;
        }

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

    // To be implemented
    protected override void Attack()
    {
    }

    void OnDrawGizmosSelected()
    {
        float posX = rb ? 0 : transform.position.x;
        Gizmos.DrawLine(new Vector2(posX + leftBoundary, int.MinValue), new Vector2(posX + leftBoundary, int.MaxValue));
        Gizmos.DrawLine(new Vector2(posX + rightBoundary, int.MinValue), new Vector2(posX + rightBoundary, int.MaxValue));
    }
}
