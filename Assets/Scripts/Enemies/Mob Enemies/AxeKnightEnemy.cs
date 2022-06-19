using UnityEngine;

public class AxeKnightEnemy : Enemy
{
    [Header("Axe Knight Parameters")]
    [SerializeField] private bool startGoingRight = true; //The direction of the enemy.

    [SerializeField] private float movementSpeed = 3; //The movement of the enemy.
    [SerializeField] private float leftBoundary = -1; //The right boundary?
    [SerializeField] private float rightBoundary = 1; //The left boundary?
    [SerializeField] private SpriteRenderer sprite;

    protected override void Start()
    {
        base.Start();
        if (leftBoundary > rightBoundary)
        {
            DoStatic.Swap(ref leftBoundary, ref rightBoundary);
        }

        float oX = transform.position.x;
        leftBoundary += oX;
        rightBoundary += oX;

        Vector3 sca = transform.localScale;
        sca.x = startGoingRight ? 1 : -1;
        transform.localScale = sca;
    }

    protected override void DoAction()
    {
        Move();
    }

    protected void Move()
    {
        Bounds spriteBoundary = sprite.bounds;
        Vector2 min = spriteBoundary.min;
        Vector2 max = spriteBoundary.max;

        Vector3 sca = transform.localScale;
        switch (sca.x)
        {
            case < 0 when min.x < leftBoundary:
            case > 0 when max.x > rightBoundary:
                sca.x *= -1;
                transform.localScale = sca;
                break;
        }

        Vector2 vel = rb.velocity;
        vel.x = sca.x * movementSpeed;
        rb.velocity = vel;
    }

    void OnDrawGizmosSelected()
    {
        float posX = rb ? 0 : transform.position.x;
        Gizmos.DrawLine(new Vector2(posX + leftBoundary, int.MinValue), new Vector2(posX + leftBoundary, int.MaxValue));
        Gizmos.DrawLine(new Vector2(posX + rightBoundary, int.MinValue), new Vector2(posX + rightBoundary, int.MaxValue));
    }
}
